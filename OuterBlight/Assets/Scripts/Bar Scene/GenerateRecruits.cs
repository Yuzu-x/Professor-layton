using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateRecruits : MonoBehaviour
{
    public GameObject[] emptyRecruitSlot;

    List<GameObject> recruitData = new List<GameObject>();

    public string recruitNameString;
    public string recruitClassString;
    public string recruitTraitString;
    public Text recruitName;
    public Text recruitClass;
    public Text recruitTrait;

    void Start()
    {
        emptyRecruitSlot = GameObject.FindGameObjectsWithTag("Unemployed");

        foreach(GameObject recSlot in emptyRecruitSlot)
        {
            GenerateRecruit();
            Text[] recTexts = recSlot.GetComponentsInChildren<Text>();

            foreach(Text recTextBox in recTexts)
            {
                if(recTextBox.tag == "ClassTag")
                {
                    recTextBox.text = recruitClassString;
                }
                else if(recTextBox.tag == "TraitTag")
                {
                    recTextBox.text = recruitTraitString;
                }
            }

        }
    }

    void Update()
    {
    }

    void GenerateRecruit()
    {
        float newRecClass = Random.Range(1, 7);
        float newRecTrait = Random.Range(1, 12);

        switch(newRecClass)
        {
            case (1):
                recruitClassString = "Addict";
                break;

            case (2):
                recruitClassString = "Mule";
                break;

            case (3):
                recruitClassString = "Gangster";
                break;

            case (4):
                recruitClassString = "Revo";
                break;

            case (5):
                recruitClassString = "Castaway";
                break;

            case (6):
                recruitClassString = "Junker";
                break;
        }

        switch (newRecTrait)
        {
            case 1:
                recruitTraitString = " ";
                break;

            case 2:
                recruitTraitString = "Veteran";
                break;

            case 3:
                recruitTraitString = "Compassionate";
                break;

            case 4:
                recruitTraitString = "Destructive";
                break;

            case 5:
                recruitTraitString = "Crazed";
                break;

            case 6:
                recruitTraitString = "Wired Weird";
                break;

            case 7:
                recruitTraitString = "Altruist";
                break;

            case 8:
                recruitTraitString = "Egomaniac";
                break;

            case 9:
                recruitTraitString = "Nihilist";
                break;

            case 10:
                recruitTraitString = "Stubborn";
                break;

            case 11:
                recruitTraitString = "Loner";
                break;

        }

    }
}
