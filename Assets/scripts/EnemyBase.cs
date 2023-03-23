using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    protected Animator anim;
    protected Vector2 movementDirection = Vector2.zero;
    protected Rigidbody2D rb;
    protected Root root;
    protected float cellsize;
    protected EnemyStats stats;
    protected ENEMY_TYPE type;
  
    private void FixedUpdate()
    {

        rb.velocity = movementDirection * stats.Speed * Time.fixedDeltaTime;
        
    }
    public virtual void InitA_STAR(AiGrid2 grid)
    {
        return;
    }
    public virtual void InitFlock(float sep, float sepOb, float tar, float align, float cohesive)
    {
        return;
    }

    public virtual void StartNightPhaseA_STAR(AiGrid2 grid)
    {
        return;
    }
    public virtual void Init()
    {
        return;
    }

    public virtual void StartNightPhase()
    {
        return;
    }
    public Vector2 Movement
    {
        get { return movementDirection; }
    }

}
