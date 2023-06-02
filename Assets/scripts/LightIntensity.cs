using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LightIntensity : MonoBehaviour
{
    /// <summary>
    /// Increases/Decreases light intensity at same rate as global volume, depending on day/night.
    /// 
    /// </summary>
    [SerializeField] Light2D lightIntensity;
    [SerializeField] float lowerSpeed;  // 0.005
    [SerializeField] float increaseSpeed; // 0.002
    bool dimLights = true;
    public Day_Night_Cycle script;
    void Start()
    {
        lightIntensity  = gameObject.GetComponent<Light2D>();
    }
    private void FixedUpdate()
    {

    }
    void Update()
    {
        if (script.daytime== true) 
        {
            dimLights = true;
        }
        else if (script.daytime==false && script.ppv.weight > 0.1) 
        {
            dimLights = false;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
         
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
           
        }
        if (lightIntensity.intensity <= 0f)
        {
            lightIntensity.intensity = 0f;
        }
        if (lightIntensity.intensity >= 5.4f)
        {
            lightIntensity.intensity = 5.4f;
        }
        if (dimLights)
        {
            lightIntensity.intensity -= lowerSpeed;
        }
        if (!dimLights)
        {
            lightIntensity.intensity += increaseSpeed;
        }
    }
}
