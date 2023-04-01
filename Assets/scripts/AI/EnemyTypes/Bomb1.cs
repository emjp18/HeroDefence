using BehaviorTree;


using System.Collections.Generic;

using UnityEngine;

public class Bomb1 : EnemyBase
{
    Vector2 leader;
    Transform player;
    AStar2D pathfinding;
    //FlockBehaviourAvoidance flockingBehavior;
    ContactPoint2D[] contacts = new ContactPoint2D[10];
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
        //flockingBehavior = new FlockBehaviourAvoidance(flockweights, grid, box,
        //    flockamount, gameObject.tag, flockID, flockLeader, grid.Getroot());
        root = new Root(new List<Node> { new ChaseFindPath(), new AttackFast()});
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats.AttackRange = player.GetComponent<BoxCollider2D>().size.x;


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7 && (bool)root.GetData("checkCollision"))
        {
            root.SetData("oldTarget", (Vector2)root.GetData("targetPosition"));
            root.SetData("newPath", true);
            root.SetData("checkCollision", false);
            root.SetData("movementDirection", Vector2.zero);




        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        avoidanceForce = (Vector2)transform.position - collision.GetContact(0).point;
        if (collision.gameObject.layer == 7 && (bool)root.GetData("checkCollision"))
        {
            root.SetData("oldTarget", (Vector2)root.GetData("targetPosition"));
            root.SetData("newPath", true);
            root.SetData("checkCollision", false);
            root.SetData("movementDirection", Vector2.zero);

        }
       
    }

    public void SetLeader(Vector2 pos)
    {
        leader = pos;
    }
    public override void StartNightPhase(AiGrid grid)
    {
        pathfinding.UpdateGrid(grid);
    
        Utility.UpdateStaticCollision(grid);
        movementDirection = Vector2.zero;
        root.SetData("targetPosition", (Vector2)buildingTarget.position);
        root.SetData("movementDirection", movementDirection);
        root.SetData("subGoal", Vector2.zero);
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
        root.SetData("checkCollision", true);
        root.SetData("grid", grid);
        root.SetData("check", false);
        root.SetData("newPath", false);
        root.SetData("astarfail", false);
        root.SetData("avoidance", Vector2.zero);
        root.SetData("avoidanceTemp", Vector2.zero);
        root.SetData("targetChange", false);
        root.SetData("searching", false);
        root.SetData("raycast", false);
        root.SetData("targetPlayer", false);
        root.SetData("playerDistance", 0);
        root.SetData("reset", false);
        root.SetData("playerTarget", false);
    }
   
    private void Update()
    {
      
        root.SetData("leader", leader);
        root.SetData("velocity", rb.velocity);
        float pd = Vector2.Distance(transform.position, player.position);
        if (pd < stats.ChasePlayerRange)
        {
            root.SetData("playerDistance", pd);
            root.SetData("targetPlayer", true);
            root.SetData("targetPosition", (Vector2)player.position);
        }
        else
        {

            root.SetData("targetPlayer", false);
            root.SetData("targetPosition", (Vector2)buildingTarget.position);
        }







        root.SetData("position", (Vector2)transform.position);

        //if ((bool)root.GetData("checkCollision"))
        //{
        //    avoidanceForce = Utility.Avoid(transform.position, pathfinding.Quadtree,
        //        (Vector2)root.GetData("movementDirection"), 1.5f);
        //    if (avoidanceForce != Vector2.zero)
        //    {
        //        root.SetData("oldTarget", (Vector2)root.GetData("targetPosition"));
        //        root.SetData("newPath", true);
        //        root.SetData("checkCollision", false);
        //        root.SetData("movementDirection", Vector2.zero);
        //    }


        //}
        //else
        //{
        //    avoidanceForce = Vector2.zero;
        //}

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
        //flockingBehavior.Attacking = isAnyoneAttacking;
        root.Evaluate();
    }
   public bool GetIsAttacking() { return isAttacking; }
    public void SetIsAttacking(bool attacking) { isAnyoneAttacking = attacking; }

   

}
