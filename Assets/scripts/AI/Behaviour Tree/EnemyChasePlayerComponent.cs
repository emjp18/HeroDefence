using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;


public class EnemyChasePlayerComponent : EnemyBase
{
 
    [SerializeField] Transform player;

    AStar2D AStarFunctionality;
    [SerializeField] float waitTime = 1.0f;

    public override void Init(AiGrid2 grid)
    {
        AStarFunctionality = new AStar2D(grid);
        cellsize = grid.GetCellSize();
        stats = new EnemyStats(ENEMY_TYPE.CHASE_PLAYER);
        Node attack = new Sequence(new List<Node> { new AttackFast(), new AttackHeavy(), new Evade(), new Attack() });
        Node chase = new Selector(new List<Node> { new ChaseGrid(), new ChaseTarget(), new Death() });
        root = new Root(new List<Node> { chase, attack, new Idle() });
    }


  
   
    private void Update()
    {
        root.SetData("withinGrid", AStarFunctionality.IsWithinGridBounds(transform.position));
        root.SetData("position", (Vector2)transform.position);
        root.SetData("targetPosition", (Vector2)player.position);
        float playerDistance = Vector2.Distance((Vector2)transform.position, (Vector2)player.position);
        if (root != null)
            root.Evaluate();
        if (!AStarFunctionality.GetPathFound() ||
              playerDistance <= stats.Range)
        {
            movementDirection = Vector2.zero;
            root.SetData("movementDirection", movementDirection);
        }

        root.SetData("dynamicTarget", playerDistance<=stats.PlayerRange);

        movementDirection = (Vector2)root.GetData("movementDirection");


    }
    


    public override void StartNightPhase(AiGrid2 grid)
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        root.SetData("attackRange", stats.Range);
        root.SetData("targetPosition", (Vector2)player.position);
        root.SetData("pathindex", 0);
        root.SetData("movementDirection", movementDirection);
        root.SetData("AStar2D", AStarFunctionality);
        root.SetData("time", 0.0f);
        root.SetData("gridCenter", grid.GetCenter());
        root.SetData("withinGrid", false);
        root.SetData("waitTime", waitTime);
        root.SetData("cellsize", cellsize);
        root.SetData("stats", stats);
        root.SetData("velocity", rb.velocity);
        root.SetData("position", (Vector2)transform.position);
        root.SetData("dynamicTarget", false);
        root.SetData("targetUpdated", true);
     
        AStarFunctionality.UpdateGrid(grid);
        AStarFunctionality.ResetPath();
        AStarFunctionality.AStarSearch((Vector2)transform.position, (Vector2)player.position, int.MaxValue);
    }
}
