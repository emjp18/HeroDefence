using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWave : MonoBehaviour
{
    public PlayerMovement player;
    private void Start()
    {

    }
    public Day_Night_Cycle dayNigScript;
  
    private int counter = 0;
    public GameObject startWaveUI;
    void Update()
    {
        if(player.alive<=0) 
        {
            if (dayNigScript.daytime && counter < 1 && player.alive <=0)
            {
                startWaveUI.SetActive(true);

            }
            else
            {
                StartNextWave();
            }
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
            counter++;
        }
        if(counter >= 1 )
        {
            startWaveUI.SetActive(false);
            counter=0;
        }
   
    }
}
