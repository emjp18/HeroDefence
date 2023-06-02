using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class StateNameController : MonoBehaviour
{
    /// <summary>
    /// This script gives the correct camera and class after selecting in the menu.
    /// Also creates fires when the Townhall building gets low HP. (This should be inside buildingHP script)
    /// Also starts music for day/night. (Should be inside Day/night Script, but it refuses to work in there so placed in in here)
    /// </summary>
    public static string ClassName= "Warrior";
    public Day_Night_Cycle script;
    [SerializeField] GameObject Ranger;
    [SerializeField] GameObject Warrior;
    [SerializeField] Camera cam;
    [SerializeField] Camera cam2;
    [SerializeField] List<GameObject> fireObjects= new List<GameObject>();
    private int musicCounter;
    private void Start()
    {
        // Selects the warrior class and correct camera, deactivates camera and ranger class.
        if (ClassName == "Warrior")
        {
            cam2.gameObject.SetActive(false);
            cam.gameObject.SetActive(true);
            Warrior.SetActive(true);
            Debug.Log("testingSelectingWarrior");

        }
        if(ClassName == "Ranger")
        {
            cam.gameObject.SetActive(false);
            cam2.gameObject.SetActive(true);
            Ranger.SetActive(true);
            Debug.Log("TestingSelectinRanger");
        }
    }
    private void Update()
    {

        // increases or decreases the light intensity depending on day/night.
        if (script.ppv.weight > 0.001f&& script.daytime==false)
        {
            fireObjects[0].GetComponent<Light2D>().intensity += 0.0025f;
            fireObjects[2].GetComponent<Light2D>().intensity += 0.0025f;
            fireObjects[1].GetComponent<Light2D>().intensity += 0.0025f;
            if (fireObjects[0].GetComponent<Light2D>().intensity >= 3.7f|| fireObjects[1].GetComponent<Light2D>().intensity >= 3.7f|| fireObjects[2].GetComponent<Light2D>().intensity >= 3.7f)
            {
                fireObjects[0].GetComponent<Light2D>().intensity = 3.7f;
                fireObjects[2].GetComponent<Light2D>().intensity = 3.7f;
                fireObjects[1].GetComponent<Light2D>().intensity = 3.7f;
            }
        }
        if (script.ppv.weight < 0.99f && script.daytime==true)
        {
            fireObjects[0].GetComponent<Light2D>().intensity -= 0.005f;
            fireObjects[2].GetComponent<Light2D>().intensity -= 0.005f;
            fireObjects[1].GetComponent<Light2D>().intensity -= 0.005f;
            if (fireObjects[0].GetComponent<Light2D>().intensity <= 0 || fireObjects[1].GetComponent<Light2D>().intensity <= 0||fireObjects[2].GetComponent<Light2D>().intensity <= 0)
            {
                fireObjects[0].GetComponent<Light2D>().intensity = 0f;
                fireObjects[2].GetComponent<Light2D>().intensity = 0f;
                fireObjects[1].GetComponent<Light2D>().intensity = 0f;
            }

        }

        if (script.daytime==false&& musicCounter <=0)
        {
            FindObjectOfType<AudioManager>().StopPlaying("DayMusic");
            if(script.ppv.weight > 0.3)
            {
            
                FindObjectOfType<AudioManager>().Play("NightMusic");
            musicCounter = 1;
            }

        }
        if (script.daytime == true && musicCounter > 0)
        {
            FindObjectOfType<AudioManager>().StopPlaying("NightMusic");
            if (script.ppv.weight < 0.7f)
            {
                FindObjectOfType<AudioManager>().Play("DayMusic");
                musicCounter = 0;
            }

        }


    }
}
