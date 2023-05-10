using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.Rendering; // used to access the volume component
using UnityEngine.Rendering.Universal;

public class Day_Night_Cycle : MonoBehaviour
{
    [SerializeField] EnemySpawner enemySpawn;
    public bool nightEndCondition = false;
    public bool nightStartCondition = false;


    public Volume ppv; // this is the post processing volume
    [SerializeField] GameObject[] lights; // all the lights we want on when its dark
    [SerializeField] float dawnLight;
    [SerializeField] float eveningLight;
    public bool daytime = true;
    public bool changeTime;
    public int musicCount;
    public int musicCount2;
    // Start is called before the first frame update
    void Start()
    {
        ppv = gameObject.GetComponent<Volume>();
        changeTime = true;
    }
    private void Update()
    {
        if (changeTime== false )
        {
            if (daytime)
            {
                ppv.weight -= 0.0005f;
                if(musicCount>= 0 )
                {
                FindObjectOfType<AudioManager>().StopPlaying("NightMusic");
                musicCount= 0;
                }
            }
            if (!daytime)
            {
                if (musicCount2 >= 0)
                {
                    FindObjectOfType<AudioManager>().StopPlaying("DayMusic");
                    musicCount2 = 0;
                }
                ppv.weight += 0.0005f;
            }
        }
        if (ppv.weight < 0)
        {
            ppv.weight = 0;
            changeTime = true;

        }
        if (ppv.weight > 1)
        {
            ppv.weight = 1;
            changeTime = true;
        }
        if(ppv.weight >=0.5f)
        {
            if (musicCount <= 0)
            {
                FindObjectOfType<AudioManager>().Play("NightMusic");
                musicCount++;
            }
        }
        if (ppv.weight <= 0.5f)
        {
            if (musicCount2 <= 0)
            {
                FindObjectOfType<AudioManager>().Play("DayMusic");
                musicCount2++;
            }
        }
        //if (ppv.weight > eveningLight)
        //{

            //    activateLights = true;
            //}
            //if (ppv.weight < dawnLight) 
            //{
            //    activateLights = false;
            //}
    }
}