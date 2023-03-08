using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.AI;

public class Attack : State
{
    public Attack(GameObject npc, Animator anim, Transform player, Transform target,bool chasePlayer) : base(
        npc, anim, player, target)
    { name = STATE.ATTACK;
        this.chasePlayer = chasePlayer;
    }
    //float attackRange = 5;
    float minDistance = 10;
    float maxDistance = 30;
    //float attackInterval = 1.5f;
    //float time = 0;
    bool chasePlayer = false;
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        float distance = Vector2.Distance(npc.transform.position, player.transform.position);
        if (chasePlayer)
        {
            
            if (distance > maxDistance)
            {
                chasePlayer = false;
                nextState = new Chase(npc, anim, player, target, chasePlayer);
                stage = EVENT.EXIT;
            }
        }
        else
        {
           
            if (distance < minDistance)
            {
                chasePlayer = true;
                nextState = new Chase(npc, anim, player, target, chasePlayer);
                stage = EVENT.EXIT;
            }
        }

        
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}
/*
 * In the attack state, the npc attacks the target if it is within range, else it starts chasing, if the player is close it 
 * chases the player instead.
 */