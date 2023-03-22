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
    public abstract void Init(AiGrid2 grid);
   
    public abstract void StartNightPhase(AiGrid2 grid);
    
}
