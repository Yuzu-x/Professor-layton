using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedRecruit : MonoBehaviour
{
    public static GameObject selectedRecruit;
    void Update()
    {
        if(selectedRecruit != null)
        {
            Vector2 recruitPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            selectedRecruit.transform.position = recruitPos;
        }
        
    }
}
