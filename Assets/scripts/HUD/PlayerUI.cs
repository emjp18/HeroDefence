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

        PlayerMovement.Instance.OnGoldAmountChanged += Instance_OnGoldAmountChanged;
        PlayerMovement.Instance.OnHealthPotionAmountChanged += Instance_OnHealthPotionAmountChanged;
    }

    private void Instance_OnHealthPotionAmountChanged(object sender, System.EventArgs e)
    {
        UpdateText();
    }
    private void Update()
    {
        UpdateText();
    }

    private void Instance_OnGoldAmountChanged(object sender, System.EventArgs e)
    {
        UpdateText();
    }

    private void UpdateText()
    {
        goldText.text = PlayerMovement.Instance.GetGoldAmount().ToString();
        healthPotionText.text = PlayerMovement.Instance.GetHealthPotionAmount().ToString();
    }
}
