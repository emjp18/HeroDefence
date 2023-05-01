using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private TextMeshProUGUI goldText;
    private TextMeshProUGUI healthPotionText;
    

    private void Awake()
    {
        goldText = transform.Find("GoldTextAmount").GetComponent<TextMeshProUGUI>();
        healthPotionText = transform.Find("HealthTextAmount").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        UpdateText();

        PlayerInteract.Instance.OnGoldAmountChanged += Instance_OnGoldAmountChanged;
        PlayerInteract.Instance.OnHealthPotionAmountChanged += Instance_OnHealthPotionAmountChanged;
    }

    private void Instance_OnHealthPotionAmountChanged(object sender, System.EventArgs e)
    {
        UpdateText();
    }

    private void Instance_OnGoldAmountChanged(object sender, System.EventArgs e)
    {
        UpdateText();
    }

    private void UpdateText()
    {
        goldText.text = PlayerInteract.Instance.GetGoldAmount().ToString();
        healthPotionText.text = PlayerInteract.Instance.GetHealthPotionAmount().ToString();
    }
}
