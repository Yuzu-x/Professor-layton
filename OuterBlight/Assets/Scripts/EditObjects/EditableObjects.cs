using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Player/1")]
public class EditableObjects : ScriptableObject
{
    public EditablePlayer player;

    public float moveRange;
    public float runRange;
    public float resourcePoints;
    public float healthPoints;
    public float meleeSkill;
    public float rangeSkill;
    public float sanity;
    public float income;
    public float exertionPoints;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
