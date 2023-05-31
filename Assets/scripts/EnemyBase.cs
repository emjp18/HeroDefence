using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;
//using static UnityEditor.PlayerSettings;

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
    GameObject hitBody;
    protected Animator anim;
    protected Vector2 movementDirection = Vector2.zero;
    protected Rigidbody2D rb;
    protected BoxCollider2D box;
    protected Root root;
    protected EnemyStats stats;
    protected Transform buildingTarget;
    protected int flockID;
    protected Vector2 avoidanceForce = Vector2.zero;
    protected Vector2 avoidanceForceEnemies = Vector2.zero;
    protected Transform player;
    protected AStar2D pathfinding;
    protected SpriteRenderer spriteRend;
    protected bool hit = false;
    protected float hitTime = 0;
    protected float hitTimeDelay = 1.1f;
    private void FixedUpdate()
    {
       if(this as Boss!=null)
        {
            rb.velocity = movementDirection.normalized * stats.MovementSpeed * Time.fixedDeltaTime
            + (avoidanceForce.normalized * Utility.GRID_CELL_SIZE_LARGE *2* stats.MovementSpeed * Time.fixedDeltaTime
             + (avoidanceForceEnemies.normalized * box.size.x * 0.3f * stats.MovementSpeed * Time.fixedDeltaTime));
        }
       else
        {
            rb.velocity = movementDirection.normalized * stats.MovementSpeed * Time.fixedDeltaTime
            + (avoidanceForce.normalized * Utility.GRID_CELL_SIZE * 2 * stats.MovementSpeed * Time.fixedDeltaTime
             + (avoidanceForceEnemies.normalized * box.size.x * 0.3f * stats.MovementSpeed * Time.fixedDeltaTime));
        }
        

        avoidanceForce = Vector2.zero;
        avoidanceForceEnemies = Vector2.zero;


        if(hit)
        {

            if(hitTime>hitTimeDelay)
            {
                hitTime = 0;
                if (!hitBody.activeSelf)
                {
                    hitBody.SetActive(true);
                    hitBody.transform.position = transform.position;

                }

                hitBody.GetComponent<disappearOnHit>().Vel
                    = (((Vector2)root.GetData("targetPosition") - (Vector2)transform.position).normalized * stats.MovementSpeed * 2.5f
                    );
            }
            else
            {
                hitTime += Time.fixedDeltaTime;
            }
            
            if((hitBody.transform.position-transform.position).magnitude>box.size.x)
                hitBody.SetActive(false);

        }
        else
        {
            hitBody.SetActive(false);
            hitBody.GetComponent<disappearOnHit>().Vel = Vector2.zero;
            
        }
    }

    public abstract void Init(AiGrid grid,  Transform player,Transform building,GameObject hitbody, int flockamount=0, int flockID = 0, bool flockLeader = false, Transform hidePoint = null,
        Transform movementRangePoint = null);

    public abstract void StartNightPhase(AiGrid grid);
    public Vector2 Avoidance
    {
        get { return avoidanceForce; }
        set { avoidanceForce = value; }
    }
    public Vector2 Movement
    {
        get { return movementDirection; }
    }
    public ENEMY_TYPE Enemytype
    {
        get { return type; }
    }
    public bool Hit
    {
        get => hit;
        set => hit = value;
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
    protected ref GameObject HitObject()
    {
        return ref hitBody;
    }
    protected void AvoidNearbyEnemies()
    {
        
        var colliders = Physics2D.OverlapCircleAll(transform.position, box.size.x*0.2f);
        Vector2 separate = Vector2.zero;
       foreach(Collider2D collision in colliders)
        {
            if (collision.gameObject.layer != 6)
                continue;

          
            separate += ((Vector2)transform.position - (Vector2)collision.gameObject.transform.position).normalized;
        }
     
        avoidanceForceEnemies = separate;
        Vector2 vel = rb.velocity;
        Vector2.SmoothDamp(avoidanceForceEnemies, separate, ref vel, 0.3f);
    }


}
