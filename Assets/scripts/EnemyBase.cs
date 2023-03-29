using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public struct FlockWeights
{
    public float separateAgents;
    public float separateStatic;
    public float moveToTarget;
    public float align;
    public float cohesive;
    public float random;
    public float leader;
     
}
public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] ENEMY_TYPE type;
   
    protected Animator anim;
    protected Vector2 movementDirection = Vector2.zero;
    protected Rigidbody2D rb;
    protected Root root;
    protected EnemyStats stats;
    protected FlockWeights flockweights;
    protected Transform buildingTarget;
    protected int flockID;
    private void FixedUpdate()
    {

        rb.velocity = movementDirection.normalized * stats.Speed * Time.fixedDeltaTime;
        //Debug.Log(stats.Speed);
        
    }
    public void SetTarget(Transform target) { buildingTarget = target; }
    public abstract void Init(AiGrid grid, int flockamount, int flockID, Transform player, bool flockLeader = false, Transform hidePoint = null,
        Transform movementRangePoint = null);

    public abstract void StartNightPhase(AiGrid grid);
    
    public Vector2 Movement
    {
        get { return movementDirection; }
    }
    public ENEMY_TYPE Enemytype
    {
        get { return type; }
    }
    
    public int FlockID
    {
        get => flockID;
        set => flockID = value;
    }
    public ref EnemyStats eStats()
    {
        return ref stats;
    }
    
    
}
