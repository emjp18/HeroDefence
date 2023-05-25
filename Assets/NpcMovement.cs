using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject npc6;
    [SerializeField] GameObject npc5;
    [SerializeField] GameObject npc4;
    [SerializeField] GameObject npc3;
    [SerializeField] GameObject npc2;
    [SerializeField] GameObject npc1;
  
    enum introStates {MainMenu,Phase1,Phase2,Phase3 };

    public Rigidbody2D rb;
    float movespeed=200;
    Vector2 movement;
    float timer=5;
    [SerializeField] Animator ani;
    void Start()
    {
        rb.velocity = movement *movespeed *Time.fixedDeltaTime;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        npc5.transform.position -= new Vector3(1,0);
        timer-= Time.fixedDeltaTime;
        //Debug.Log("timerTest" + timer);
 
        if(timer<=0)
        {
            ani.SetTrigger("SideIdle");
            timer = 5;
        }
        else
        {
            ani.SetTrigger("SideWalk");
        }
    }
}
