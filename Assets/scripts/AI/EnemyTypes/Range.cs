using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : EnemyBase
{
    AStar2D pathfinding;

    public override void Init(AiGrid grid, int flockamount, int flockID, Transform hidePoint, Transform movementRangePoint, Transform player)
    {
        pathfinding = new AStar2D(grid);
        root = new Root(new List<Node> { new Chase(), new AttackFast() });
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public override void StartNightPhase(AiGrid grid)
    {
        pathfinding.UpdateGrid(grid);
        pathfinding.AStarSearch((Vector2)transform.position, (Vector2)buildingTarget.position, int.MaxValue);
    }

    void Update()
    {
        
    }
}
