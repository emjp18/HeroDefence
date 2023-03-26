using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FlockWeights
{
    public float separateAgents;
    public float separateStatic;
    public float moveToTarget;
    public float align;
    public float cohesive;
     
}
public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] ENEMY_TYPE type;
    protected Animator anim;
    protected Vector2 movementDirection = Vector2.zero;
    protected Rigidbody2D rb;
    protected Root root;
    protected EnemyStats stats;

  
    private void FixedUpdate()
    {

        rb.velocity = movementDirection * stats.Speed * Time.fixedDeltaTime;
        
    }
    public abstract void Init(FlockWeights flockweights, AiGrid grid);

    public abstract void StartNightPhase(AiGrid grid);
    
    public Vector2 Movement
    {
        get { return movementDirection; }
    }
    public ENEMY_TYPE Enemytype
    {
        get { return type; }
    }
}
