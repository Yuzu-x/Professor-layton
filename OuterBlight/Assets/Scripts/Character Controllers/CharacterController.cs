using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    //Determine the turn
    public bool myTurn = false;

    //Find tiles to move to
    List<Tile> selectableTiles = new List<Tile>();
    GameObject[] tiles;

    //Orientate Self
    Stack<Tile> path = new Stack<Tile>();
    Tile initialTile;

    //Variables of movement
    public bool isMoving = false;
    public bool hasSelectedMove = false;
    public bool isActive = false;
    public bool moveSelected = false;
    public bool runSelected = false;

    public static float moveActionsThisTurn = 0f;
    public static float activeCharacter = 0f;

    //How should Character travel
    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    //Can Character go there
    float halfHeight = 0;

    //Variables of Y axis
    bool fallingDown = false;
    bool jumpingUp = false;
    bool moveToEdge = false;
    Vector3 jumpTarget;

    //Enemies land next to the Player
    public Tile actualTargetTile;

    //Character Attributes
    public float moveRange;
    public float runRange;
    public float jumpHeight = 3;
    public float characterMoveSpeed = 2;
    public float jumpVelocity = 4.5f;
    public static float maxActionPoints;
    public float maxActionPointsNS = 4f;
    public static float currentActionPoints;
    public float currentActionPointsNS = 0f;
    public Image actionPointImage;
    public Image quickRefAPImage;
    public float pointSpent = 0f;
    public float maxHealth = 100f;
    public float currentHealth = 10f;
    public Image playerHealth;
    public Image quickRefHPImage;
    public static float fatigue = 0f;



    public enum TurnState
    {
        MOVING,
        CASTING,
        LONGCASTING,
        ATTACK,
        RUNNING,
        RUSHING,
        WAITING,
        DEAD
    }

    public TurnState currentState;

    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        halfHeight = GetComponent<Collider>().bounds.extents.y;

        TurnManager.AddUnit(this);

        maxActionPoints = maxActionPointsNS;

    }

    void Update()
    {
        actionPointImage.fillAmount = currentActionPoints / maxActionPoints;
        quickRefAPImage.fillAmount = currentActionPoints / maxActionPoints;
    }

    //Movement Handling
    public void GetInitialTile()
    {
        initialTile = GetTargetTile(gameObject);
        initialTile.current = true;

    }

    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;
        if(Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();

        }

        return tile;
    }

    public void ComputeAdjacencyLists(float jumpHeight, Tile target)
    {
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbours(jumpHeight, target);

        }
    }

    public void FindSelectableTiles()
    {
        ComputeAdjacencyLists(jumpHeight, null);
        GetInitialTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(initialTile);
        initialTile.visited = true;

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();
            selectableTiles.Add(t);
            t.selectable = true;

            if (t.distance < moveRange)
            {


                foreach (Tile tile in t.adjacencyList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);

                    }
                }
            }
        }
    }

    public void MoveToTile(Tile tile)
    {
        path.Clear();
        tile.target = true;
        isMoving = true;

        Tile next = tile;
        while(next != null)
        {
            path.Push(next);
            next = next.parent;
        }
    }

    public void Move()
    {
        if (path.Count > 0)
        {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

            if (Vector3.Distance(transform.position, target) >= 0.05f)
            {
                bool shouldJump = transform.position.y != target.y;

                if (shouldJump)
                {
                    MoveVertically(target);
                }
                else
                {
                    CalculateHeading(target);
                    SetHorizontalVelocity();
                }
                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;

            }
            else
            {
                transform.position = target;
                path.Pop();
            }

        }
        else
        {
            RemoveSelectableTiles();
            isMoving = false;
            MoveEnd();
          

        }
    }

    void MoveEnd()
    {
        SpendActionPoint(1f);

        if (moveSelected)
        {
            moveActionsThisTurn += 1f;
        }
        else if (runSelected)
        {
            Fatigued(1);

        }


        moveSelected = false;
        runSelected = false;
        currentState = TurnState.WAITING;
    }

    protected void RemoveSelectableTiles()
    {
        if(initialTile != null)
        {
            initialTile.current = false;
            initialTile = null;
        }
        foreach(Tile tile in selectableTiles)
        {
            tile.Reset();
        }

        selectableTiles.Clear();
    }

    void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }

    void SetHorizontalVelocity()
    {
        velocity = heading * characterMoveSpeed;

    }

    void MoveVertically(Vector3 target)
    {
        if(fallingDown)
        {
            FallDown(target);
        }
        else if(jumpingUp)
        {
            JumpUp(target);
        }
        else if(moveToEdge)
        {
            ApproachEdge();
        }
        else
        {
            PrepareJump(target);
        }
    }
    void PrepareJump(Vector3 target)
    {
        float targetY = target.y;

        target.y = transform.position.y;

        CalculateHeading(target);

        if(transform.position.y > targetY)
        {
            fallingDown = false;
            jumpingUp = false;
            moveToEdge = true;

            jumpTarget = transform.position + (target - transform.position) / 2f;

        }
        else
        {
            fallingDown = false;
            jumpingUp = true;
            moveToEdge = false;

            velocity = heading * characterMoveSpeed / 3f;

            float difference = targetY - transform.position.y;

            velocity.y = jumpVelocity * (0.5f + difference / 2f);

        }
    }
    void FallDown(Vector3 target)
    {
        velocity += Physics.gravity * Time.deltaTime;

        if(transform.position.y <= target.y)
        {
            fallingDown = false;

            Vector3 p = transform.position;
            p.y = target.y;
            transform.position = p;

            velocity = new Vector3();
        }
    }

    void JumpUp(Vector3 target)
    {
        velocity += Physics.gravity * Time.deltaTime;

        if (transform.position.y > target.y)
        {
            jumpingUp = false;
            fallingDown = true;
        }
    }

    void ApproachEdge()
    {
        if(Vector3.Distance(transform.position, jumpTarget) >= 0.05f)
        {
            SetHorizontalVelocity();
        }
        else
        {
            moveToEdge = false;
            fallingDown = true;

            velocity /= 3.0f;
            velocity.y = 1.5f;

        }
    }

    protected Tile FindLowestF(List<Tile> list)
    {
        Tile lowest = list[0];

        foreach(Tile t in list)
        {
            if(t.f < lowest.f)
            {
                lowest = t;
            }
        }

        list.Remove(lowest);


        return lowest;
    }

    protected Tile FindEndTile(Tile t)
    {
        Stack<Tile> tempPath = new Stack<Tile>();

        Tile next = t.parent;

        while(next != null)
        {
            tempPath.Push(next);
            next = next.parent;

        }

        if(tempPath.Count <= moveRange)
        {
            return t.parent;

        }

        Tile endTile = null;
        for(int i = 0; i <= moveRange; i++)
        {
            endTile = tempPath.Pop();

        }

        return endTile;
    }

    protected void FindPath(Tile target)
    {
        ComputeAdjacencyLists(jumpHeight, target);
        GetInitialTile();

        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(initialTile);
        initialTile.h = Vector3.Distance(initialTile.transform.position, target.transform.position);
        initialTile.f = initialTile.h;

        while(openList.Count > 0)
        {
            Tile t = FindLowestF(openList);

            closedList.Add(t);

            if(t == target)
            {
                actualTargetTile = FindEndTile(t);
                MoveToTile(actualTargetTile);

                return;

            }

            foreach(Tile tile in t.adjacencyList)
            {
                if (closedList.Contains(tile))
                    {

                     }

                else if (openList.Contains(tile))
                {
                    float tempG = t.g + Vector3.Distance(tile.transform.position, t.transform.position);

                    if(tempG < tile.g)
                    {
                        tile.parent = t;

                        tile.g = tempG;
                        tile.f = tile.g + tile.h;

                    }
                }
                else
                {
                    tile.parent = t;

                    tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                    tile.h = Vector3.Distance(tile.transform.position, target.transform.position);
                    tile.f = tile.g + tile.h;

                    openList.Add(tile);
                }
            }
        }
    }


    //Game Turn Handling
    public void TurnBegin()
    {
        myTurn = true;

        if (fatigue > 0)
        {
            fatigue = fatigue - 1;
        }
    }

    public void TurnEnd()
    {
        myTurn = false;
    }

    //Character Resources

     
    public void TakeDamage(float damageDealt)
    {
        currentHealth -= damageDealt;

        playerHealth.fillAmount = currentHealth / maxHealth;
        quickRefHPImage.fillAmount = currentHealth / maxHealth;

        if(currentHealth <= 0)
        {
            currentState = TurnState.DEAD;

        }
    }

    public void SpendActionPoint (float pointSpent)
    {
        currentActionPoints -=  pointSpent;
    }

    public void Fatigued(float isFatigued)
    {
        fatigue += isFatigued;
    }

    public void Healed(float healthReturned)
    {
        currentHealth += healthReturned;

        playerHealth.fillAmount = currentHealth / maxHealth;
        quickRefHPImage.fillAmount = currentHealth / maxHealth;
    }
}
