using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Min0taur : EnemyController
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void DeeRAM()
    {
        //resourcePoints = resourcePoints - 2f;
        FindBestTarget();
        CalculatePath();
        FindSelectableTiles();

    }
}
