using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    static Dictionary<string, List<CharacterController>> units = new Dictionary<string, List<CharacterController>>();
    static Queue<string> turnKey = new Queue<string>();
    static Queue<CharacterController> turnTeam = new Queue<CharacterController>();

    public static float turnCount = 0f;
    public Text turnCountText;

    public static bool playerTurn = false;
    public static bool enemyTurn = false;


    void Start()
    {

        TurnCountUpdate();

    }

    void Update()
    {
        if(turnTeam.Count == 0)
        {
            //if (!ZeroTurnBehaviour.turnZero)
            {
                InitTeamTurnQueue();
            }
        }

        TurnCountUpdate();

    }

    static void InitTeamTurnQueue()
    {
        List<CharacterController> teamList = units[turnKey.Peek()];

        foreach (CharacterController unit in teamList)
        {
            turnTeam.Enqueue(unit);
        }

        turnCount = turnCount + 1f;

        StartTurn();
    }

    static void StartTurn()
    {
        CharacterController.currentActionPoints = CharacterController.maxActionPoints;
        CharacterController.moveActionsThisTurn = 0f;




        if (turnTeam.Count > 0)
        {
            turnTeam.Peek().TurnBegin();

        }
    }

    void TurnCountUpdate()
    {
            turnCountText.text = "Turn " + turnCount;
    }

    public static void FinishTurn()
    {
        CharacterController unit = turnTeam.Dequeue();
        unit.TurnEnd();

        if (turnTeam.Count > 0)
        {
            StartTurn();

        }
        else
        {
            string team = turnKey.Dequeue();
            turnKey.Enqueue(team);
            InitTeamTurnQueue();
            turnCount = turnCount + 1f;

        }
    }

    public static void AddUnit(CharacterController unit)
    {
        List<CharacterController> list;

        if(!units.ContainsKey(unit.tag))
        {
            list = new List<CharacterController>();
            units[unit.tag] = list;

            if(!turnKey.Contains(unit.tag))
            {
                turnKey.Enqueue(unit.tag);

            }
        }
        else
        {
            list = units[unit.tag];

        }
        list.Add(unit);

    }
}
