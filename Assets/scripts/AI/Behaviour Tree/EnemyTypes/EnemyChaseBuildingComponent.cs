using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseBuildingComponent : EnemyBase
{
  
    [SerializeField] Transform targetBuilding;
    [SerializeField] float waitTime = 1.0f;
    AStar2D AStarFunctionality;


   

   


    public override void StartNightPhaseA_STAR(AiGrid2 grid) 
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        root.SetData("attackRange", stats.Range);
        root.SetData("targetPosition", (Vector2)targetBuilding.position);
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
        AStarFunctionality.AStarSearch((Vector2)transform.position, (Vector2)targetBuilding.position, int.MaxValue);
    }

    private void Update()
    {
        root.SetData("withinGrid", AStarFunctionality.IsWithinGridBounds(transform.position));
        root.SetData("position", (Vector2)transform.position);

  
        if (root != null)
            root.Evaluate();
        if (!AStarFunctionality.GetPathFound() ||
            Vector2.Distance((Vector2)transform.position, (Vector2)targetBuilding.position) <= stats.Range)
        {
           
            movementDirection = Vector2.zero;
            root.SetData("movementDirection", movementDirection);
        }

       
        movementDirection = (Vector2)root.GetData("movementDirection");

       


    }

    public override void InitA_STAR(AiGrid2 grid)
    {
        type = ENEMY_TYPE.CHASE_BUILDING;
        stats = new EnemyStats(type);

        AStarFunctionality = new AStar2D(grid);
        Node attack = new Sequence(new List<Node> { new AttackFast(), new AttackHeavy(), new Evade(), new Attack() });
        Node chase = new Selector(new List<Node> { new ChaseASTARGrid(), new ChaseTargetASTAR() });
        root = new Root(new List<Node> { chase, attack, new IdleASTAR() });

        cellsize = grid.GetCellSize();
    }
}
