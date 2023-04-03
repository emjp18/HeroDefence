using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNameController : MonoBehaviour
{

    public static string testnr;
    public Day_Night_Cycle script;
    private void Start()
    {
        
    }
    private void Update()
    {
        if (script.daytime==false)
        {
            FindObjectOfType<AudioManager>().Play("DayMusic");
        }

        if (testnr == "Warrior")
        {
            //script.daytime = true;
            FindObjectOfType<AudioManager>().Play("MenuMusic");
            
        }
    }
}
