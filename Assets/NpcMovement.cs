using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject gameObject;
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
        gameObject.transform.position -= new Vector3(1,0);
        timer-= Time.fixedDeltaTime;
        Debug.Log("timerTest" + timer);
 
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
