using BehaviorTree;


using System.Collections.Generic;

using UnityEngine;

public class Bomb1 : EnemyBase
{
    Vector2 leader;
    Transform player;
    AStar2D pathfinding;
    FlockBehaviourAvoidance flockingBehavior;
   
    bool isAttacking = false;
    bool isAnyoneAttacking = false;
    public override void Init(AiGrid grid, int flockamount, int flockID, Transform player, bool flockLeader =false, Transform hidePoint = null,
        Transform movementRangePoint = null)
    {
        pathfinding = new AStar2D(grid);
        this.player = player;
        this.flockID = flockID;
         flockweights = new FlockWeights();
    
        stats = new EnemyStats(Enemytype);
        var box = GetComponent<BoxCollider2D>();
        flockingBehavior = new FlockBehaviourAvoidance(flockweights, grid, box,
            flockamount, gameObject.tag, flockID, flockLeader, grid.Getroot());
        root = new Root(new List<Node> { new ChaseFindPath(), new AttackFast()});
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats.AttackRange = player.GetComponent<BoxCollider2D>().size.x;


    }
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
        {

            root.SetData("newPath", true);

            root.SetData("obstacleCell", collision.GetContact(0).point);

        }
    }

    public void SetLeader(Vector2 pos)
    {
        leader = pos;
    }
    public override void StartNightPhase(AiGrid grid)
    {
        pathfinding.UpdateGrid(grid);
        flockingBehavior.UpdateGrid(grid);
        flockingBehavior.UpdateStaticCollision();
        movementDirection = Vector2.zero;
        root.SetData("targetPosition", (Vector2)buildingTarget.position);
        root.SetData("movementDirection", movementDirection);
        root.SetData("flockPattern", flockingBehavior);
        root.SetData("position", (Vector2)transform.position);
        root.SetData("dead", false);
        root.SetData("withinAttackRange", false);
        root.SetData("deactivate", false);
        root.SetData("attackHeavy", false);
        root.SetData("attacking", false);
        root.SetData("velocity", rb.velocity);
        root.SetData("type", Enemytype);
        root.SetData("pathIndex", 0);
        root.SetData("cellSize", grid.GetCellSize());
        root.SetData("aStar", pathfinding);
        root.SetData("index", new Vector2Int());
        root.SetData("isLeader", flockingBehavior.Leader);
        root.SetData("grid", grid);
        root.SetData("check", false);
        root.SetData("newPath", false);
        root.SetData("astarfail", false);
        root.SetData("avoidance", Vector2.zero);
        root.SetData("avoidanceTemp", Vector2.zero);
        root.SetData("targetChange", false);
    }
   
    private void Update()
    {
      
        root.SetData("leader", leader);
        root.SetData("velocity", rb.velocity);

        if (Vector2.Distance(transform.position, player.position) < stats.ChasePlayerRange)
        {
            if ((Vector2)root.GetData("targetPosition") != (Vector2)player.position)
            {
                root.SetData("targetChange", true);
            }
            else
            {
                root.SetData("targetChange", false);
            }
            root.SetData("targetPosition", (Vector2)player.position);
        }
        else
        {
            if ((Vector2)root.GetData("targetPosition") != (Vector2)buildingTarget.position)
            {
                root.SetData("targetChange", true);
            }
            else
            {
                root.SetData("targetChange", false);
            }
            root.SetData("targetPosition", (Vector2)buildingTarget.position);
        }







        root.SetData("position", (Vector2)transform.position);


        Vector2 avoid = (flockingBehavior.CalculateDirection(transform.position, Vector2.zero, Vector2.zero,
                Vector2.zero, Vector2.zero));
        root.SetData("avoidance", avoid);
        movementDirection = (Vector2)root.GetData("movementDirection");

        if ((bool)root.GetData("deactivate"))
        {
            gameObject.SetActive(false);
        }

        root.SetData("dead", stats.Health <= 0);
        root.SetData("withinAttackRange",
         (Vector2.Distance((Vector2)root.GetData("targetPosition"),
         (Vector2)transform.position) < stats.AttackRange));

       

        if ((bool)root.GetData("withinAttackRange"))
        {
            isAttacking = true;
            int r = Random.Range(0, 2);
            if (r == 0)
            {
                root.SetData("attackHeavy", true);
            }
            else
            {
                root.SetData("attackHeavy", false);
            }
            root.SetData("movementDirection", Vector2.zero);
         
        }
        else
        {
            
            isAttacking = false;
            
           

        }
        flockingBehavior.Attacking = isAnyoneAttacking;
        root.Evaluate();
    }
   public bool GetIsAttacking() { return isAttacking; }
    public void SetIsAttacking(bool attacking) { isAnyoneAttacking = attacking; }

    
}
