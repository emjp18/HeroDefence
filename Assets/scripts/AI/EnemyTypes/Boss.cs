using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class Boss : EnemyBase
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 vel = rb.velocity;
        Vector2 avoid = (Vector2)transform.position - collision.GetContact(0).point;
        Vector2.SmoothDamp(avoidanceForce, avoid, ref vel, 0.3f);
        if (collision.gameObject.layer == 7 && (bool)root.GetData("checkCollision"))
        {
            root.SetData("oldTarget", (Vector2)root.GetData("targetPosition"));
            root.SetData("newPath", true);
            root.SetData("checkCollision", false);
            root.SetData("movementDirection", Vector2.zero);

        }

    }
    //

    public override void StartNightPhase(AiGrid grid)
    {
        pathfinding.UpdateGrid(grid);
        movementDirection = Vector2.zero;
        root.SetData("targetPosition", (Vector2)buildingTarget.position);
        root.SetData("movementDirection", movementDirection);
        root.SetData("position", (Vector2)transform.position);
        root.SetData("withinAttackRange", false);
        root.SetData("cellSize", grid.GetCellSize());
        root.SetData("aStar", pathfinding);
        root.SetData("index", new Vector2Int());
        root.SetData("checkCollision", true);
        root.SetData("newPath", false);
        root.SetData("reset", false);
        root.SetData("grid", grid);
        root.SetData("boss", true);
    }

    private void Update()
    {
        AvoidNearbyEnemies();

        float pd = Vector2.Distance(transform.position, player.position);
        if (pd < stats.ChasePlayerRange)
        {


            root.SetData("targetPosition", (Vector2)player.position);
        }
        else
        {


            root.SetData("targetPosition", (Vector2)buildingTarget.position);
        }


        root.SetData("position", (Vector2)transform.position);


        movementDirection = (Vector2)root.GetData("movementDirection");
        if ((Vector2)root.GetData("targetPosition") == (Vector2)buildingTarget.transform.position)
        {
            root.SetData("withinAttackRange",
         (Vector2.Distance((Vector2)root.GetData("targetPosition"),
         (Vector2)transform.position) < stats.AttackBuildingRange));
        }
        else
        {
            root.SetData("withinAttackRange",
         (Vector2.Distance((Vector2)root.GetData("targetPosition"),
         (Vector2)transform.position) < stats.AttackPlayerRange));
        }

        root.Evaluate();
    }

    public override void Init(AiGrid grid, Transform player, Transform building,
        int flockamount = 0, int flockID = 0, bool flockLeader = false, Transform hidePoint = null, Transform movementRangePoint = null)
    {
        pathfinding = new AStar2D(grid);
        this.player = player;
        this.buildingTarget = building;
        stats = new EnemyStats(Enemytype);
        root = new Root(new List<Node> { new Chase() });
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats.AttackPlayerRange = player.GetComponent<BoxCollider2D>().size.x;
        stats.AttackBuildingRange = Utility.GRID_CELL_SIZE_LARGE;
        box = GetComponent<BoxCollider2D>();
    }
}
