using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartWave : MonoBehaviour
{
    /// <summary>
    /// Button to start the Waves. Creates the button and gives a pulsing effect of growing and shrinking.
    /// </summary>
    public Day_Night_Cycle dayNigScript;
    [SerializeField] TextMeshProUGUI textPart1;
    [SerializeField] GameObject labelBoard;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 pulseSpeedText;
    [SerializeField] private Vector3 pulseSpeedLabel;
    [SerializeField] private WaveManager waveScript;
    public int counter = 0;
    public GameObject startWaveUI;
    public PlayerMovement player;
    private bool grow;
    private bool shrink;
    [SerializeField]public enum STARTSTATES {START,NIGHT}
    public STARTSTATES states;
    private void Start()
    {
        states = STARTSTATES.START;
        shrink = false;
        grow= true;
    }
    private void FixedUpdate()
    {

        if (grow)
        {
            textPart1.transform.localScale += pulseSpeedText;
            labelBoard.transform.localScale += pulseSpeedLabel;
        }
        if (shrink)
        {
            labelBoard.transform.localScale -= pulseSpeedLabel;
            textPart1.transform.localScale -= pulseSpeedText;
        }
    }
    void Update()
    {
        // Controlls the pulse effect to make sure it does not get to big/small.
        if (labelBoard.transform.localScale.y >= 1.2f && textPart1.transform.localScale.y >= 1.2f && labelBoard.transform.localScale.x >= 1.2f && textPart1.transform.localScale.x >= 1.2f)
        {
            grow = false;
            shrink = true;   
        }
        if (labelBoard.transform.localScale.y <= 1f && textPart1.transform.localScale.y <= 1f)
        {
            grow = true;
            shrink = false;
        }

   
        if (states == STARTSTATES.START)
        {
            startWaveUI.SetActive(true);
        }
    }
    public void StartNextWave()
    {
        // Activates the night, removes the start button and starts the wave.
        if (counter == 0&& waveScript.currentRoundCount <= 4)
        {
            states= STARTSTATES.NIGHT;
            dayNigScript.changeTime = false;
            dayNigScript.daytime = false;
            startWaveUI.SetActive(false);
            waveScript.currentRoundCount++;
            counter++;
        }
        if (counter >= 1)
        {
            startWaveUI.SetActive(false);

        }

    }
}
