using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : EnemyBase
{
    AStar2D pathfinding;

    public override void Init(AiGrid grid, int flockamount, int flockID, Transform hidePoint, Transform movementRangePoint, Transform player)
    {
        pathfinding = new AStar2D(grid);
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
