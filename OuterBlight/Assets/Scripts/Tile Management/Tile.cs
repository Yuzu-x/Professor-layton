using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    //Tile states
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public bool walkable = true;

    public List<Tile> adjacencyList = new List<Tile>();

    public bool visited = false;
    public Tile parent = null;
    public int distance = 0;

    public float f = 0;
    public float g = 0;
    public float h = 0;


    void Start()
    {

    }


    void Update()
    {
        //Colouring tiles based on their state
        if (current)
        {
            GetComponent<Renderer>().material.color = Color.black;

        }
        else if (target)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else if (selectable)
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }


    public void Reset()
    {
        //Resets tile states before functions
        adjacencyList.Clear();
        current = false;
        target = false;
        selectable = false;
        visited = false;
        parent = null;
        distance = 0;

        f = g = h = 0;
    }

    public void FindNeighbours(float jumpHeight, Tile target)
    {
        Reset();

        //Creates a list of tiles around the central, selected tile
        CheckTile(Vector3.forward, jumpHeight, target);
        CheckTile(-Vector3.forward, jumpHeight, target);
        CheckTile(Vector3.right, jumpHeight, target);
        CheckTile(-Vector3.right, jumpHeight, target);

    }

    public void CheckTile(Vector3 direction, float jumpHeight, Tile target)
    {
        //Determines whether a tile can be moved to
        Vector3 halfExtents = new Vector3(0.25f, (1 + jumpHeight) / 2.0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            if (tile != null && tile.walkable)
            {
                RaycastHit hit;

                if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1) || (tile == target))
                {
                    adjacencyList.Add(tile);
                }
            }
        }
    }
}
