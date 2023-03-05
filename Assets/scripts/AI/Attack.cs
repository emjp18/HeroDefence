using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attack : State
{
    public Attack(GameObject npc, Animator anim, Transform player, Transform target) : base(
        npc, anim, player, target)
    { name = STATE.ATTACK; }


    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {

        
    }

    public override void Exit()
    {
        base.Exit();
    }
}
