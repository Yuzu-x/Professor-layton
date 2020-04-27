using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : CharacterController
{
    public GameObject activePlayer = null;
    public bool isPlayer = true;
    public GameObject playerIndicator = null;
    public bool pIndicator = false;
    public float moveActual = 6f;
    public float movesMadeThisTurn = 0f;

    //inventory
    //public bool myInventory;
    //public GameObject playerInventory;
    //CharacterInventory myItems;

    private int everyActiveSlot;
    private int everyPassiveSlot;
    private GameObject[] actSlot;
    private GameObject[] pasSlot;
    public GameObject activeInventorySlotPanel;

    public float interestTime = 0f;


    void Start()
    {

        currentState = TurnState.WAITING;
        currentHealth = maxHealth;
        Init();


        everyActiveSlot = 4;
        everyPassiveSlot = 4;
        actSlot = new GameObject[everyActiveSlot];

        for (int i = 0; i < everyActiveSlot; i++)
        {
            actSlot[i] = activeInventorySlotPanel.transform.GetChild(i).gameObject;
        }

    }

    //State Machine
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10f);

        }

        MouseOverInteractable();

       // if (isActive)
       // {
       //     myInventory = true;
       //
        //}
     //   else
      //  {
      //      myInventory = false;
      //  }

      //  if (myInventory)
     //   {
      //      playerInventory.SetActive(true);

      //  }
      //  else
      //  {
      //      playerInventory.SetActive(false);
      //  }


        Debug.DrawRay(transform.position, transform.forward);

        switch (currentState)
        {
            case (TurnState.MOVING):
                MoveSelected();
                break;

            case (TurnState.CASTING):

                break;

            case (TurnState.LONGCASTING):

                break;

            case (TurnState.RUNNING):
                MoveSelected();
                break;

            case (TurnState.RUSHING):

                break;

            case (TurnState.WAITING):

                break;

            case (TurnState.DEAD):

                break;
        }

      /*  if(TurnManager.playerTurn)
        {
            myTurn = true;
        }
        else if(TurnManager.enemyTurn)
        {
            myTurn = false;
        }
        */
        if (!myTurn)
        {
            return;
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                Ray charay = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit charHit;

                if (Physics.Raycast(charay, out charHit))
                {
                    if (charHit.collider.tag == "Player")
                    {
                        SwapActivePlayer("ActivePlayer");
                        charHit.collider.tag = "ActivePlayer";
                        isActive = true;
                        activePlayer = GameObject.FindGameObjectWithTag("ActivePlayer");

                    }
                }

            }
        }


        if (isActive)
        {
            if (!pIndicator)
            {
                pIndicator = true;
               GameObject newIndicator =  Instantiate(playerIndicator, transform.position, Quaternion.identity) as GameObject;
                newIndicator.transform.SetParent(activePlayer.transform);
            }
        }
        else
        {
            pIndicator = false;
        }
        


        if (currentActionPoints == 0)
        {
            if(currentState != TurnState.LONGCASTING)
            {
                currentState = TurnState.WAITING;
                gameObject.tag = "Player";
            }
        }


    }

    //Movement Handling
    void MoveSelected()
    {
        if (isActive)
        {
            if (moveActionsThisTurn == 0)
            {
                if (!isMoving)
                {

                    FindSelectableTiles();
                    CheckMouse();

                }
                else
                {
                    Move();
                }
            }
            else
            {
                currentState = TurnState.WAITING;
            }
        }
    }



    void CheckMouse()
    {

        Ray pathVisRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit visHit;

        if(Physics.Raycast(pathVisRay, out visHit))
        {
            if(visHit.collider.tag == "Tile")
            {
                Tile t = visHit.collider.GetComponent<Tile>();

                if(t.selectable)
                {
                    t.GetComponent<Renderer>().material.color = Color.red;

                }
            }
        }

        if(Input.GetMouseButtonUp(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider.tag == "Tile")
                {
                    Tile t = hit.collider.GetComponent<Tile>();

                    if(t.selectable)
                    {
                        MoveToTile(t);

                    }
                }
            }
        }
    }

    //Character Resources
    void PlayerCharacterHealth()
    {

    }

    //Player UI Events
    public void MoveButton()
    {
        moveSelected = true;
        runSelected = false;

        if(moveSelected)
        {
            moveRange = moveActual;
        }

        if (movesMadeThisTurn < 1)
        {

            if (isActive)
            {
                if (moveActionsThisTurn == 0)
                {
                    movesMadeThisTurn += 1f;
                    currentState = TurnState.MOVING;
                }
                else
                {
                    moveSelected = false;
                    currentState = TurnState.WAITING;
                }
            }
        }
        else
        {
            moveSelected = false;
        }
    }

    public void RunButton()
    {
        runSelected = true;
        moveSelected = false;
        if (runSelected)
        {
            runRange -= fatigue;
            moveRange = runRange;
        }
        else
        {
            return;
        }

        if (isActive)
        {
            currentState = TurnState.RUNNING;
        }
    }

    public void EndTurnButton()
    {
        if (!isMoving)
        {
         //   TurnManager.FinishTurn();
            gameObject.tag = "Player";
            isActive = false;
         //   TurnManager.playerTurn = false;
          //  TurnManager.enemyTurn = true;
            movesMadeThisTurn = 0f;
        }
    }

    void InventoryUpdater()
    {
        Debug.Log("Inventory Updater running");
    }

    //Team Management
    void SwapActivePlayer(string _tag)
    {
        GameObject needSwap = GameObject.FindWithTag(_tag);

        if(needSwap)
        {
            gameObject.tag = "Player";
            isActive = false;

        }
    }

    //Interaction Handling
    void MouseOverInteractable()
    {
            Ray mouseSearch = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit canInteract;

            if (Physics.Raycast(mouseSearch, out canInteract))
            {
                if (canInteract.collider.tag == "NPC" || canInteract.collider.tag == "Player" || canInteract.collider.tag == "Objective")
                {
                //    Interact interactable = canInteract.collider.GetComponent<Interact>();
                    interestTime = interestTime + 1 * Time.deltaTime;

                    if (interestTime >= 3)
                    {
                        Debug.Log("The information about this object will be shown... One day...");
                    }
                    else
                    {
                        return;
                    }
                }
                else
            {
                interestTime = 0f;
            }
            }
    }
}
