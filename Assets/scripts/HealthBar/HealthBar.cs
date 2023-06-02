using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health; // set max healthbar health to max health
        slider.value = health; // set current health on healthbar to max health aswell

        fill.color = gradient.Evaluate(1f); //fill in with colour
    }

    public void SetHealth(float health)
    {
        slider.value = health; //set healthbar to current health

        fill.color = gradient.Evaluate(slider.normalizedValue); //adjust fill
    }

}
