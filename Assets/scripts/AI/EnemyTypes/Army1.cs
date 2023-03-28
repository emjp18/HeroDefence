using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Army1 : EnemyBase
{
    Vector2 leader;
    Transform player;
    AStar2D pathfinding;
    FlockBehaviourChase flockingBehavior;
    [SerializeField] float targetPower;
    [SerializeField] float separatePower;
    [SerializeField] float separateObjectPower;
    [SerializeField] float AlignPower;
    [SerializeField] float CohesionPower;
    [SerializeField] float randomDirPower;
    [SerializeField] float followLeaderWeight;

    public override void Init(AiGrid grid, int flockamount, int flockID, Transform player, bool flockLeader =false, Transform hidePoint = null,
        Transform movementRangePoint = null)
    {
        pathfinding = new AStar2D(grid);
        this.player = player;
        this.flockID = flockID;
         flockweights = new FlockWeights();
        flockweights.random = randomDirPower;
        flockweights.align = AlignPower;
        flockweights.separateAgents = separatePower;
        flockweights.separateStatic = separateObjectPower;
        flockweights.cohesive = AlignPower;
        flockweights.moveToTarget = targetPower;
        flockweights.leader = followLeaderWeight;
        stats = new EnemyStats(Enemytype);
        var box = GetComponent<BoxCollider2D>();
        flockingBehavior = new FlockBehaviourChase(flockweights, grid, box,
            flockamount,gameObject.tag, flockID, flockLeader);
        root = new Root(new List<Node> { new ChaseFindPath(), new AttackFast()});
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats.AttackRange = player.GetComponent<BoxCollider2D>().size.x;


    }
    public void SetLeader(Vector2 pos)
    {
        leader = pos;
    }
    public override void StartNightPhase(AiGrid grid)
    {
        pathfinding.UpdateGrid(grid);
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
        root.SetData("newPath", false);
        root.SetData("index", new Vector2Int());
        root.SetData("isLeader", flockingBehavior.Leader);
        root.SetData("grid", grid);
  
    }
   
    private void Update()
    {
      
        root.SetData("leader", leader);
        root.SetData("velocity", rb.velocity);

        if (Vector2.Distance(transform.position, player.position) < stats.ChasePlayerRange)
        {
            root.SetData("targetPosition", (Vector2)player.position);
        }
        else
        {
            root.SetData("targetPosition", (Vector2)buildingTarget.position);
        }
        root.SetData("obstacleCell", flockingBehavior.ObstaclePos);

        root.SetData("newPath", flockingBehavior.NewPath);

        if(flockingBehavior.NewPath)
        {
            flockingBehavior.NewPath = (bool)root.GetData("newPath");
        }
        

        root.SetData("position", (Vector2)transform.position);
       
        movementDirection = (Vector2)root.GetData("movementDirection");

        if ((bool)root.GetData("deactivate"))
        {
            gameObject.SetActive(false);
        }

        root.SetData("dead", stats.Health <= 0);
        root.SetData("withinAttackRange",
         (Vector2.Distance((Vector2)root.GetData("targetPosition"), (Vector2)transform.position) < stats.AttackRange));

       

        if ((bool)root.GetData("withinAttackRange"))
        {
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
        root.Evaluate();
    }
   
}
