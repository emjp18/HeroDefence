using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNameController : MonoBehaviour
{

    public static string testnr= "Warrior";
    public Day_Night_Cycle script;
    [SerializeField] GameObject gameObject;
    [SerializeField] GameObject gameObject2;
    [SerializeField] Camera cam;
    [SerializeField] Camera cam2;
    private void Start()
    {

        if (testnr == "Warrior")
        {
            cam2.gameObject.SetActive(false);
            cam.gameObject.SetActive(true);
            gameObject2.SetActive(true);
            //script.daytime = true;
            //FindObjectOfType<AudioManager>().Play("MenuMusic");
            Debug.Log("testingSelectingWarrior");

        }
        if(testnr == "Ranger")
        {
            cam.gameObject.SetActive(false);
            cam2.gameObject.SetActive(true);
            gameObject.SetActive(true);
            Debug.Log("TestingSelectinRanger");
        }
    }
    private void Update()
    {
        if (script.daytime==false)
        {
            FindObjectOfType<AudioManager>().Play("DayMusic");
        }

    }
}
