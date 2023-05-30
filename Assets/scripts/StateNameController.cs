using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNameController : MonoBehaviour
{

    public static string testnr;
    public Day_Night_Cycle script;
    private void Start()
    {
        if (testnr == "Warrior")
        {
            //script.daytime = true;
            //FindObjectOfType<AudioManager>().Play("MenuMusic");
            Debug.Log("testingSelectingWarrior");

        }
        if(testnr == "Ranger")
        {
            Debug.Log("TestingSelectinRanger");
        }
    }
    private void Update()
    {
        if (script.daytime==false)
        {
            //FindObjectOfType<AudioManager>().Play("DayMusic");
        }

    }
}
