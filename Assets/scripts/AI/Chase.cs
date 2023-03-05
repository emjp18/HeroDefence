using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chase : State
{


    bool reachedEndDestination = false;

    
   
    public bool ReachedEndDest
    {
        get { return reachedEndDestination; }
        set { reachedEndDestination = value; }
    }
    public Chase(GameObject npc, Animator anim, Transform player, Transform target) : base(npc
        , anim, player, target)
    {


        name = STATE.CHASE;
    }
    public override void Enter()
    {
        //anim.SetBool("moving", true);
     
        base.Enter();
    }
    public override void Update()
    {
        if(reachedEndDestination)
        {
            nextState = new Attack(npc, anim, player, target);
            stage = EVENT.EXIT;
            reachedEndDestination = false;
           
        }


    }
    public override void Exit()
    {

        //anim.SetBool("moving", false);
        base.Exit();
    }
}

