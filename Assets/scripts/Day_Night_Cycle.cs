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
    float nightTime = 0.0f;
    float dayTime = 0.0f;
    bool isDay = false;

    public Volume ppv; // this is the post processing volume
    [SerializeField] bool activateLights; // checks if lights are on
    [SerializeField] GameObject[] lights; // all the lights we want on when its dark
    [SerializeField] float dawnLight;
    [SerializeField] float eveningLight;
    public bool daytime = false;
    // Start is called before the first frame update
    void Start()
    {
        ppv = gameObject.GetComponent<Volume>();
        //lights = gameObject.GetComponents<GameObject>();
    }
    private void Update()
    {
        if (StateNameController.testnr == "Ranger")
        {
            daytime = true;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            daytime = true;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {

            daytime = false;
        }
        if (daytime)
        {
            ppv.weight -= 0.0005f;
        }
        if (!daytime)
        {
            ppv.weight += 0.0005f;
        }
        //if (isDay)
        //{
        //    dayTime += Time.deltaTime;
        //}
        //else
        //{
        //    nightTime += Time.deltaTime;
        //}
        //if (nightStartCondition) //Pause Game
        //{
        //    isDay = false;
        //    dayTime = 0.0f;
        //    nightStartCondition = false;
        //    enemySpawn.StartNightPhase();
        //}
        //if (nightEndCondition)//Pause Game
        //{
        //    isDay = true;
        //    nightTime = 0.0f;
        //    nightEndCondition = false;
        //    enemySpawn.EndNightPhase();
        //}
        //if (activateLights == false) // if lights are on
        //{
        //    for (int i = 0; i < lights.Length; i++)
        //    {
        //        lights[i].SetActive(false); // shut them off

        //    }
        //}
       

        //if (activateLights == true) // if lights havent been turned on
        //{
          
        //    for (int i = 0; i < lights.Length; i++)
        //    {
        //        lights[i].SetActive(true); // turn them all on

        //    }
            
        //}
        if (ppv.weight < 0)
        {
            ppv.weight = 0;
        }
        if (ppv.weight > 1)
        {
            ppv.weight = 1;
        }
        if (ppv.weight > eveningLight)
        {

            activateLights = true;
        }
        if (ppv.weight < dawnLight) 
        {
            activateLights = false;
        }
    }
}