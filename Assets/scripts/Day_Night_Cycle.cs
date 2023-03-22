using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering; // used to access the volume component

public class Day_Night_Cycle : MonoBehaviour
{

    public Volume ppv; // this is the post processing volume

    [SerializeField] float dawnLight;
    [SerializeField] float eveningLight;

    [SerializeField] bool activateLights; // checks if lights are on
    [SerializeField]GameObject[] lights; // all the lights we want on when its dark
    bool daytime = false;
    // Start is called before the first frame update
    void Start()
    {
        ppv = gameObject.GetComponent<Volume>();
    }

    private void Update()
    {
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

        if (ppv.weight < 0)
        {
            ppv.weight = 0;
        }
        if (ppv.weight > 1)
        {
            ppv.weight = 1;
        }
        if (ppv.weight >eveningLight)
        {
            activateLights= true;
        }
        if (ppv.weight<dawnLight)
        {
            activateLights= false;
        }
        if (activateLights)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].SetActive(true); // turn them all on
            }
        }
        if (!activateLights)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].SetActive(false); // turn them all off
            }
        }
    }

    void FixedUpdate() 
    {

   
    }


    public void ControlPPV() // used to adjust the post processing slider.
    {

        //ppv.weight = 0;
        //if (hours >= 21 && hours < 22) // dusk at 21:00 / 9pm    -   until 22:00 / 10pm
        //{
        //    ppv.weight = (float)mins / 60; // since dusk is 1 hr, we just divide the mins by 60 which will slowly increase from 0 - 1 

        //    if (activateLights == false) // if lights havent been turned on
        //    {
        //        if (mins > 45) // wait until pretty dark
        //        {
        //            for (int i = 0; i < lights.Length; i++)
        //            {
        //                lights[i].SetActive(true); // turn them all on
        //            }
        //            activateLights = true;
        //        }
        //    }
        //}


        //if (hours >= 6 && hours < 7) // Dawn at 6:00 / 6am    -   until 7:00 / 7am
        //{
        //    ppv.weight = 1 - (float)mins / (float)60; // we minus 1 because we want it to go from 1 - 0

        //    if (activateLights == true) // if lights are on
        //    {
        //        if (mins > 10) // wait until pretty bright
        //        {
        //            for (int i = 0; i < lights.Length; i++)
        //            {
        //                lights[i].SetActive(false); // shut them off
        //            }
        //            activateLights = false;
        //        }
        //    }
        //}
    }

}