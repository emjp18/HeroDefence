using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class hitIndicator : MonoBehaviour
{
    public Volume ppv;
    public PlayerMovement player;
    public bool playerHit;
    void Start()
    {
        ppv = gameObject.GetComponent<Volume>();
    }
    // intensity 0.3f
    // Update is called once per frame
    void Update()
    {
        if(player.currentHealth<100 && playerHit)
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
