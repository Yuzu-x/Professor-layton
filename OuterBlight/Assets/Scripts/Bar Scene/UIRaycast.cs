using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIRaycast : MonoBehaviour
{
    GraphicRaycaster characterRaycast;
    PointerEventData mouseOverCharacter;
    EventSystem recruitAction;
    void Start()
    {
        characterRaycast = GetComponent<GraphicRaycaster>();
        recruitAction = GetComponent<EventSystem>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            mouseOverCharacter = new PointerEventData(recruitAction);
            mouseOverCharacter.position = Input.mousePosition;

            List<RaycastResult> recruits = new List<RaycastResult>();

            characterRaycast.Raycast(mouseOverCharacter, recruits);

            foreach(RaycastResult recruit in recruits)
            {
                if(recruit.gameObject.tag == "Unemployed")
                {
                    SelectedRecruit.selectedRecruit = recruit.gameObject;
                }
            }
        }
        
    }
}
