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
    public float random;
     
}
public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] ENEMY_TYPE type;
    [SerializeField] float targetPower;
    [SerializeField] float separatePower;
    [SerializeField] float separateObjectPower;
    [SerializeField] float AlignPower;
    [SerializeField] float CohesionPower;
    [SerializeField] float randomDirPower;
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

        rb.velocity = movementDirection * stats.Speed * Time.fixedDeltaTime;
        
    }
    public void SetTarget(Transform target) { buildingTarget = target; }
    public abstract void Init(AiGrid grid, int flockamount, int flockID, Transform hidePoint,
        Transform movementRangePoint, Transform player);

    public abstract void StartNightPhase(AiGrid grid);
    
    public Vector2 Movement
    {
        get { return movementDirection; }
    }
    public ENEMY_TYPE Enemytype
    {
        get { return type; }
    }
    public float RandomW
    {
        get => randomDirPower;
        set => randomDirPower = value;
    }
    public float TargetW
    {
        get => targetPower;
        set => targetPower = value;
    }
    public float AlignW
    {
        get => AlignPower;
        set => AlignPower = value;
    }
    public float CohesiveW
    {
        get => CohesionPower;
        set => CohesionPower = value;
    }
    public float SeparateAgentW
    {
        get => separatePower;
        set => separatePower = value;
    }
    public float SeparateObstacleW
    {
        get => separateObjectPower;
        set => separateObjectPower = value;
    }
    public int FlockID
    {
        get => flockID;
        set => flockID = value;
    }
}
