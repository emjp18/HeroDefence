using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWave : MonoBehaviour
{
    private void Start()
    {

    }
    public Day_Night_Cycle dayNigScript;
  
    private int counter = 0;
    public GameObject startWaveUI;
    void Update()
    {
        if (dayNigScript.daytime && counter <1)
        {
            startWaveUI.SetActive(true);
            Debug.Log("KNAPP TEST");
 
        }
        else
        {
            StartNextWave();
        }
        //StartNextWave();

    }
    public void StartNextWave()
    {
        if(counter == 0 )
        {
            dayNigScript.daytime = false;
            dayNigScript.changeTime = false;
            startWaveUI.SetActive(false);
            Debug.Log("KNAPP HÄST");
            counter++;
        }
        if(counter >= 1 )
        {
            startWaveUI.SetActive(false);
        }
   
    }
}
