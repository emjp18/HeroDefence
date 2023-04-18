using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWave : MonoBehaviour
{
    private void Start()
    {

    }
    public Day_Night_Cycle dayNigScript;
    public GameObject startWaveUI;
    void Update()
    {
        if (dayNigScript.daytime)
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
        dayNigScript.daytime = false;
        dayNigScript.changeTime= false;
        startWaveUI.SetActive(false);
        Debug.Log("KNAPP HÄST");
    }
}
