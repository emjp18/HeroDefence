using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartWave : MonoBehaviour
{
    public Day_Night_Cycle dayNigScript;
    [SerializeField] TextMeshProUGUI textPart1;
    [SerializeField] GameObject labelBoard;
    [SerializeField] private PlayerMovement movescript;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 pulseSpeedText;
    [SerializeField] private Vector3 pulseSpeedLabel;
    [SerializeField] private Camera cam;
    private int counter = 0;
    public GameObject startWaveUI;
    public PlayerMovement player;
    private bool grow;
    private bool shrink;
    private void Start()
    {
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
        Debug.Log(labelBoard.transform.position);
        //labelBoard.transform.position = movescript.transform.position + offset;
        //labelBoard.transform.position = cam.transform.position + offset;

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

            if (player.alive<=0) 
        {
            if (dayNigScript.daytime && counter < 1 && player.alive <= 0)
            {
                //startWaveUI.SetActive(true);

            }
            else
            {
                StartNextWave();
            }
        }

        //StartNextWave();

    }
    public void StartNextWave()
    {
        if(counter == 0 )
        {
            dayNigScript.daytime = false;
            dayNigScript.changeTime = false;
            startWaveUI.SetActive(false);
            counter++;
        }
        if(counter >= 1 )
        {
            startWaveUI.SetActive(false);
            counter=0;
        }
   
    }
}
