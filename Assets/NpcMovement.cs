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
    float movespeed = 200;
    Vector2 movement;
    float timer = 3;
    Vector3 npc5Movement;
    [SerializeField] Rigidbody2D rbNpc1;
    [SerializeField] Rigidbody2D rbNpc2;
    [SerializeField] Rigidbody2D rbNpc3;
    [SerializeField] Rigidbody2D rbNpc4;
    [SerializeField] Rigidbody2D rbNpc5;
    [SerializeField] Rigidbody2D rbNpc6;

    [SerializeField] Animator aniNpc1;
    [SerializeField] Animator aniNpc2;
    [SerializeField] Animator aniNpc3;
    [SerializeField] Animator aniNpc4;
    [SerializeField] Animator aniNpc5;
    [SerializeField] Animator aniNpc6;
    void Start()
    {
        rbNpc5.velocity = movement *movespeed *Time.fixedDeltaTime;
        npc5Movement = new Vector3(0.5f, 0);
        aniNpc5.SetTrigger("SideWalk");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        npc5.transform.position -= npc5Movement;
        timer-= Time.fixedDeltaTime;
        //Debug.Log("timerTest" + timer);
 
        if(timer<=0)
        {
            aniNpc5.SetTrigger("BackWalk");
            timer = 5;
            npc5Movement = new Vector3(0, -1);
        }
        else
        {
            
        }
    }
}
