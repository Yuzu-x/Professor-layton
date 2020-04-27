using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRendererController : MonoBehaviour
{
    public bool enabledRenderer = false;
    public GameObject[] findTiles;
    List<GameObject> tileList = new List<GameObject>();

    public void Start()
    {
        //Makes a list of all tile objects and removes their renderer so they aren't visible
        findTiles = GameObject.FindGameObjectsWithTag("Tile");


        foreach (GameObject gO in findTiles)
        {
            gO.GetComponent<MeshRenderer>().enabled = false;
            tileList.Add(gO);
        }
    }

   public void EnableOrDisableRenderer()
    {
        //The button click makes the renderers visible so the tiles can be selected
        if (!enabledRenderer)
        {
            foreach (GameObject tile in tileList)
            {
                tile.GetComponent<MeshRenderer>().enabled = true;
            }

            enabledRenderer = true;
        }
        else if (enabledRenderer)
        {
            //Turns the renderer back off when no longer needed
            foreach (GameObject tile in tileList)
            {
                tile.GetComponent<MeshRenderer>().enabled = false;
            }

            enabledRenderer = false;
        }
    }
}
