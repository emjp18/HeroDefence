using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering; 
using UnityEngine.Rendering.Universal;

public class Day_Night_Cycle : MonoBehaviour
{   
    /// <summary>
    /// Controlls a global volume with color adjustments override. 
    /// Creates the effect of night and day.
    /// </summary>
    public Volume ppv; 
    [SerializeField] float dawnLight;
    [SerializeField] float eveningLight;
    public bool daytime = true;
    public bool changeTime;

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
            }
            if (!daytime)
            {
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
    }
}