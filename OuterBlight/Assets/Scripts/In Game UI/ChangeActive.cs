using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeActive : MonoBehaviour
{
    public Text member1;
    public Text member2;
    public Text member3;
    public Text member4;
    public Text member5;

    public Image activeColour;

    public GameObject activeMember;
    public float activeNumber;

    public GameObject memberGO1;
    public GameObject memberGO2;
    public GameObject memberGO3;
    public GameObject memberGO4;
    public GameObject memberGO5;

    void Start()
    {
        activeNumber = 1f;
    }

    void Update()
    {
        switch (activeNumber)
        {
            case 1:
                activeMember = memberGO1;
                activeColour.color = Color.white;
                break;

            case 2:
                activeMember = memberGO2;
                activeColour.color = Color.green;
                break;
            case 3:
                activeMember = memberGO3;
                activeColour.color = Color.red;
                break;
            case 4:
                activeMember = memberGO4;
                activeColour.color = Color.cyan;
                break;
            case 5:
                activeMember = memberGO5;
                activeColour.color = Color.yellow;
                break;
        }


        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            activeNumber = 1f;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            activeNumber = 2f;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            activeNumber = 3f;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            activeNumber = 4f;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            activeNumber = 5f;
        }
    }
}