using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LightIntensity : MonoBehaviour
{
    [SerializeField] Light2D lightIntensity;
    [SerializeField] float lowerSpeed;  // 0.005
    [SerializeField] float increaseSpeed; // 0.002
    bool dimLights = false;
    void Start()
    {
        lightIntensity  = gameObject.GetComponent<Light2D>();
    }




    // Update is called once per frame
    private void FixedUpdate()
    {

    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F))
        {
            dimLights = true;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            dimLights = false;
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
