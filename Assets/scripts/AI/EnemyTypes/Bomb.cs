using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : EnemyBase
{
    AStar2D pathfinding;

    public override void Init(AiGrid grid, int flockamount, int flockID, Transform player, bool flockLeader = false, Transform hidePoint = null,
        Transform movementRangePoint = null)
    {
        pathfinding = new AStar2D(grid);
        root = new Root(new List<Node> { new FindTarget(), new AttackHeavy() });
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public override void StartNightPhase(AiGrid grid)
    {
        stats = new EnemyStats(Enemytype);
        pathfinding.UpdateGrid(grid);
        pathfinding.AStarSearch((Vector2)transform.position, (Vector2)buildingTarget.position, int.MaxValue);
        movementDirection = Vector2.zero;
        root.SetData("targetPosition", (Vector2)buildingTarget.position);
        root.SetData("movementDirection", movementDirection);
        root.SetData("aStar", pathfinding);
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
    }

    void Update()
    {
        root.SetData("position", (Vector2)transform.position);
        root.Evaluate();
        movementDirection = (Vector2)root.GetData("movementDirection");

        if ((bool)root.GetData("deactivate"))
        {
            gameObject.SetActive(false);
        }
        root.SetData("withinAttackRange",
         (Vector2.Distance((Vector2)buildingTarget.position, (Vector2)transform.position) < stats.AttackRange));
    }
}
