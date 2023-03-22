using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering; // used to access the volume component

public class Day_Night_Cycle : MonoBehaviour
{
    [SerializeField] EnemySpawn enemySpawn;
    public bool nightEndCondition = false;
    public bool nightStartCondition = false;
    float nightTime = 0.0f;
    float dayTime = 0.0f;
    bool isDay = false;

    public Volume ppv; // this is the post processing volume

    [SerializeField] float tick = 600; 
    [SerializeField] float seconds;
    [SerializeField] int mins;
    [SerializeField] int hours;
    [SerializeField] int days = 1;

    [SerializeField] bool activateLights; // checks if lights are on
    [SerializeField]GameObject[] lights; // all the lights we want on when its dark
    // Start is called before the first frame update
    void Start()
    {
        ppv = gameObject.GetComponent<Volume>();
    }
    private void Update()
    {
        if (isDay)
        {
            dayTime += Time.deltaTime;
        }
        else
        {
            nightTime += Time.deltaTime;
        }
        if (nightStartCondition) //Pause Game
        {
            isDay = false;
            dayTime = 0.0f;
            nightStartCondition = false;
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
    // Update is called once per frame
    void FixedUpdate() // we used fixed update, since update is frame dependant. 
    {
        //CalcTime();
    }

    public void CalcTime() // Used to calculate sec, min and hours
    {
        seconds += Time.fixedDeltaTime * tick; // multiply time between fixed update by tick

        if (seconds >= 60) // 60 sec = 1 min
        {
            seconds = 0;
            mins += 1;
        }

        if (mins >= 60) //60 min = 1 hr
        {
            mins = 0;
            hours += 1;
        }

        if (hours >= 24) //24 hr = 1 day
        {
            hours = 0;
            days += 1;
        }
        ControlPPV(); // changes post processing volume after calculation
    }

    public void ControlPPV() // used to adjust the post processing slider.
    {
        //ppv.weight = 0;
        if (hours >= 21 && hours < 22) // dusk at 21:00 / 9pm    -   until 22:00 / 10pm
        {
            ppv.weight = (float)mins / 60; // since dusk is 1 hr, we just divide the mins by 60 which will slowly increase from 0 - 1 

            if (activateLights == false) // if lights havent been turned on
            {
                if (mins > 45) // wait until pretty dark
                {
                    for (int i = 0; i < lights.Length; i++)
                    {
                        lights[i].SetActive(true); // turn them all on
                    }
                    activateLights = true;
                }
            }
        }


        if (hours >= 6 && hours < 7) // Dawn at 6:00 / 6am    -   until 7:00 / 7am
        {
            ppv.weight = 1 - (float)mins / (float)60; // we minus 1 because we want it to go from 1 - 0

            if (activateLights == true) // if lights are on
            {
                if (mins > 10) // wait until pretty bright
                {
                    for (int i = 0; i < lights.Length; i++)
                    {
                        lights[i].SetActive(false); // shut them off
                    }
                    activateLights = false;
                }
            }
        }
    }

}