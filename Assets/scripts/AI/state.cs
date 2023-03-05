using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State
{
  public enum STATE { IDLE, PATROL, CHASE, ATTACK};
    public enum EVENT { ENTER, UPDATE, EXIT };

    public STATE name;
    protected EVENT stage;
    protected GameObject npc;
    protected Transform player;
    protected Animator anim;
    protected State nextState;
    protected Transform target;

    //float visDistance = 10.0f;
    //float visangle = 30.0f;
    //float dist = 7.0f;

    public State(GameObject npc, Animator anim, Transform player, Transform target)
    {
        this.npc = npc;
        this.player = player;
        this.anim = anim;
        this.target = target;
        stage = EVENT.ENTER;
    
    }
    public virtual void Enter()
    {
        stage = EVENT.UPDATE;
    }
    public virtual void Update()
    {
        stage = EVENT.UPDATE;
    }
    public virtual void Exit()
    {
        stage = EVENT.EXIT;
    }
    public State Process()
    {
        if (stage==EVENT.ENTER)
        {
            Enter();
        }
        if(stage==EVENT.UPDATE)
        {
            Update();
        }
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }
}
