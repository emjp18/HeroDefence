using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class hitIndicator : MonoBehaviour
{
    /// <summary>
    /// Changes a global volume with vingette Override
    /// vingette intensety is set to intensity 0.3f
    /// the global volume is set at 0 untill player gets HIT then in PlayerMovementScript the playerHit bool get activated and 
    /// sets volume intensity to 1 then it lowers in here
    /// <summary>
    public Volume ppv;
    public PlayerMovement player;
    public bool playerHit;
    void Start()
    {
        ppv = gameObject.GetComponent<Volume>();
    }
   
    void Update()
    {
        if(player.currentHealth<player.maxHealth && playerHit)
        {
           
            ppv.weight -= 0.001f;
        
           
        }
        if(ppv.weight<0 && playerHit)
        {
            playerHit = false;
            ppv.weight = 0;
        }
    }
}
