using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TeamManager : MonoBehaviour
{
    GameObject unemployed;
    GameObject[] available;
    public GameObject[] recruitSlot;
    public GameObject[] teamSlot;
    Transform emptyRecruitSlot;
    Transform emptyTeamSlot;
    public Image recruitList;
    Transform recruitListPos;
    List<Transform> unemployedSlots = new List<Transform>();
    List<GameObject> recruitReady = new List<GameObject>();

    List<Transform> onTeam = new List<Transform>();

    public bool overTeamRoster = false;
    public bool overRecruit = false;

    void Start()
    {
        unemployed = GameObject.FindGameObjectWithTag("Unemployed");
        available = GameObject.FindGameObjectsWithTag("Unemployed");
        recruitListPos = recruitList.transform;

        recruitSlot = GameObject.FindGameObjectsWithTag("RecruitSlot");
        teamSlot = GameObject.FindGameObjectsWithTag("OnTeam");
        foreach(GameObject rs in recruitSlot)
        {
            emptyRecruitSlot = rs.GetComponent<Transform>();
            unemployedSlots.Add(emptyRecruitSlot);
        }
        foreach(GameObject ot in teamSlot)
        {
            emptyTeamSlot = ot.GetComponent<Transform>();
            onTeam.Add(emptyTeamSlot);
        }
    }

    void Update()
    {
        PointerEventData mouse = new PointerEventData(EventSystem.current);
        mouse.position = Input.mousePosition;
        List<RaycastResult> findTeamRoster = new List<RaycastResult>();
        EventSystem.current.RaycastAll(mouse, findTeamRoster);
        int count = findTeamRoster.Count;

        foreach (GameObject recruit in available)
        {
            unemployed = recruit;
            recruitReady.Add(unemployed);
        }

        foreach(RaycastResult potential in findTeamRoster)
        {
            Debug.Log(potential);

            if(potential.gameObject.tag == "TeamRoster")
            {
                overTeamRoster = true;
                
            }
            else if(potential.gameObject.tag == "RecruitSlot")
            {
                overRecruit = true;
                Debug.Log("OverRecruit is true");
            }
            else
            {
                overTeamRoster = false;
                overRecruit = false;
            }

            if(SelectedRecruit.selectedRecruit)
            {
                foreach(Transform emptySlot in onTeam)
                {
                    if (Input.GetMouseButtonUp(0) && overTeamRoster)
                    {
                        SelectedRecruit.selectedRecruit.transform.SetParent(emptySlot);
                        SelectedRecruit.selectedRecruit.gameObject.tag = "OnTeam";
                        onTeam.Remove(emptySlot);

                        SelectedRecruit.selectedRecruit.transform.position = emptySlot.position;
                        SelectedRecruit.selectedRecruit.transform.rotation = emptySlot.rotation;

                        SelectedRecruit.selectedRecruit = null;
                    }
                    else if (Input.GetMouseButtonUp(0) && overTeamRoster == false)
                    {
                        SelectedRecruit.selectedRecruit.transform.SetParent(emptyRecruitSlot);
                        SelectedRecruit.selectedRecruit.transform.position = emptyRecruitSlot.position;
                        SelectedRecruit.selectedRecruit = null; 
                    }
                }
            }
        }
        
    }
}
