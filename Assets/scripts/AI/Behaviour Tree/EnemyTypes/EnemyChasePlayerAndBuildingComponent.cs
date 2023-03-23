using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasePlayerAndBuildingComponent : EnemyBase
{
    [SerializeField] bool focusPlayer = false;

    [SerializeField] Transform player;
    [SerializeField] Transform buildingTarget;

    AStar2D AStarFunctionality;
    [SerializeField] float waitTime =1.0f;
    Transform currentTarget;

    public bool AggroPlayer
    {
        get { return focusPlayer; }
        set { focusPlayer = value; }
    }
   

    public override void InitA_STAR(AiGrid2 grid)
    {
        type = ENEMY_TYPE.CHASE_BOTH;
        stats = new EnemyStats(type);
        cellsize = grid.GetCellSize();
        AStarFunctionality = new AStar2D(grid);
        Node attack = new Sequence(new List<Node> { new AttackFast(), new AttackHeavy(), new Evade(), new Attack() });
        Node chase = new Selector(new List<Node> { new ChaseASTARGrid(), new ChaseTargetASTAR() });
        root = new Root(new List<Node> { chase, attack, new IdleASTAR()});
    }

    private void Update()
    {
        float d = Vector2.Distance(player.position, transform.position);
        if (d < stats.PlayerRange && currentTarget == buildingTarget)
        {
            if(focusPlayer)
            {
                currentTarget = player;
            }
            else if(d < Vector2.Distance(buildingTarget.position, transform.position))
            {
                currentTarget = player;
            }
            root.SetData("dynamicTarget", true);
            root.SetData("targetUpdated", true);
        }
        else if(currentTarget == player)
        {
            currentTarget = buildingTarget;
            root.SetData("dynamicTarget", false);
            root.SetData("targetUpdated", true);
        }

        root.SetData("withinGrid", AStarFunctionality.IsWithinGridBounds(transform.position));
        root.SetData("position", (Vector2)transform.position);
        root.SetData("targetPosition", (Vector2)currentTarget.position);
     
        if (root != null)
            root.Evaluate();
        if (!AStarFunctionality.GetPathFound() ||
            Vector2.Distance((Vector2)transform.position, (Vector2)currentTarget.position) <= stats.Range)
        {
            movementDirection = Vector2.zero;
            root.SetData("movementDirection", movementDirection);
        }


        movementDirection = (Vector2)root.GetData("movementDirection");


    }
    


    public override void StartNightPhaseA_STAR(AiGrid2 grid)
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentTarget = buildingTarget;
        root.SetData("attackRange", stats.Range);
        root.SetData("targetPosition", (Vector2)currentTarget.position);
        root.SetData("pathindex", 0);
        root.SetData("movementDirection", movementDirection);
        root.SetData("AStar2D", AStarFunctionality);
        root.SetData("time", 0.0f);
        root.SetData("gridCenter", grid.GetCenter());
        root.SetData("withinGrid", false);
        root.SetData("waitTime", waitTime);
        root.SetData("cellsize", cellsize);
        root.SetData("stats", stats);
        root.SetData("dynamicTarget", false);
        root.SetData("velocity", rb.velocity);
        root.SetData("position", (Vector2)transform.position);
        root.SetData("targetUpdated", true);

        currentTarget = buildingTarget;
        AStarFunctionality.UpdateGrid(grid);
        AStarFunctionality.ResetPath();
        AStarFunctionality.AStarSearch((Vector2)transform.position, (Vector2)currentTarget.position, int.MaxValue);
    }
}
