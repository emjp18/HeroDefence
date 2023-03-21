using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cycle : MonoBehaviour
{
    [SerializeField] EnemySpawn enemySpawn;
    public bool nightEndCondition = false;
    public bool nightStartCondition = false;
    float nightTime = 0.0f;
    float dayTime = 0.0f;
    bool isDay = true;
 


    void Update()
    {
        if(isDay)
        {
            dayTime += Time.deltaTime;
        }
        else
        {
            nightTime += Time.deltaTime;
        }
        if(nightStartCondition) //Pause Game
        {
            isDay = false;
            dayTime = 0.0f;
            nightStartCondition =false;
            enemySpawn.StartNightPhase();
        }
        if (nightEndCondition)//Pause Game
        {
            isDay = true;
            nightTime = 0.0f;
            nightEndCondition = false;
            enemySpawn.EndNightPhase();
        }
    }
}
