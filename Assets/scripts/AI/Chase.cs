using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chase : State
{

    bool chasePlayer = false;
    bool reachedEndDestination = false;

    float movementspeed;
    Vector2 movementDirection = Vector2.zero;
   
    public bool ReachedEndDest
    {
        get { return reachedEndDestination; }
        set { reachedEndDestination = value; }
    }
    public bool GetShouldChasePlayer() { return chasePlayer; }
    public void SetShouldChasePlayer(bool should) { chasePlayer = should; }
    public Chase(GameObject npc, Animator anim, Transform player, Transform target, bool chasePlayer) : base(npc
        , anim, player, target)
    {


        name = STATE.CHASE;
        this.chasePlayer = chasePlayer;
    }
    public override void Enter()
    {
        
        

        base.Enter();
    }
    public void SetMovement(float speed, Vector2 dir)
    {
        movementspeed = speed;
        movementDirection = dir;
    }
    public override void Update()
    {
        if(reachedEndDestination)
        {
            //nextState = new Attack(npc, anim, player, target,chasePlayer);
            //stage = EVENT.EXIT;
            reachedEndDestination = false;
            Debug.Log("reachedend");
           
        }
        else
        {
            anim.SetFloat("Horizontal", movementDirection.x);
            anim.SetFloat("Vertical", movementDirection.y);
            anim.SetFloat("Speed", movementspeed);
        }


    }
    public override void Exit()
    {

        
        base.Exit();
    }
}

