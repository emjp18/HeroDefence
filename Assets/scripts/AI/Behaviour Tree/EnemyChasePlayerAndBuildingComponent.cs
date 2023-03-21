using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasePlayerAndBuildingComponent : MonoBehaviour
{
    [SerializeField] bool focusPlayer = false;
    [SerializeField] float playerRange = 50;
    [SerializeField] float attackRange = 4;
    [SerializeField] float movementSpeed = 100;
    [SerializeField] Transform player;
    [SerializeField] Transform buildingTarget;
    [SerializeField] AiGrid2 AIPathGrid;
    AStar2D AStarFunctionality;
    Animator anim;
    Vector2 movementDirection = Vector2.zero;
    Rigidbody2D rb;

    Root root;
    [SerializeField] float waitTime = 4.0f;
    float cellsize;
    Transform currentTarget;
    EnemyStats stats;
   
    private void FixedUpdate()
    {

        rb.velocity = movementDirection * movementSpeed * Time.fixedDeltaTime;

    }

    
    public void StartNightPhase() //This should be called without any other movement from the player. Otherwise it will lag.
    {
        cellsize = AIPathGrid.GetCellSize();
        stats = new EnemyStats(ENEMY_TYPE.CHASE_BOTH);
        currentTarget = buildingTarget;
        Node attack = new Sequence(new List<Node> { new AttackFast(), new AttackHeavy(), new Evade(), new Attack() });
        Node chase = new Selector(new List<Node> { new ChaseGrid(), new ChaseTarget() });
        root = new Root(new List<Node> { chase, attack, new Idle(), new Death() });
        AStarFunctionality = new AStar2D(AIPathGrid);
        root.SetData("attackRange", attackRange);
        root.SetData("targetPosition", (Vector2)currentTarget.position);
        root.SetData("pathindex", 0);
        root.SetData("movementDirection", movementDirection);
        root.SetData("AStar2D", AStarFunctionality);
        root.SetData("time", 0.0f);
        root.SetData("gridCenter", AIPathGrid.GetCenter());
        root.SetData("withinGrid", false);
        root.SetData("waitTime", waitTime);
        root.SetData("cellsize", cellsize);
        root.SetData("stats", stats);
        //anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        root.SetData("velocity", rb.velocity);
        root.SetData("position", (Vector2)transform.position);
        currentTarget = buildingTarget;
        AStarFunctionality.UpdateGrid(AIPathGrid);
        AStarFunctionality.ResetPath();
        AStarFunctionality.AStarSearch((Vector2)transform.position, (Vector2)currentTarget.position, 4096);
    }
    private void Update()
    {
        float d = Vector2.Distance(player.position, transform.position);
        if (d < playerRange )
        {
            if(focusPlayer)
            {
                currentTarget = player;
            }
            else if(d < Vector2.Distance(buildingTarget.position, transform.position))
            {
                currentTarget = player;
            }
           
        }
        else
        {
            currentTarget = buildingTarget;
        }

        root.SetData("withinGrid", AStarFunctionality.IsWithinGridBounds(transform.position));
        root.SetData("position", (Vector2)transform.position);
        root.SetData("targetPosition", (Vector2)currentTarget.position);
     
        if (root != null)
            root.Evaluate();
        if (!AStarFunctionality.GetPathFound() ||
            Vector2.Distance((Vector2)transform.position, (Vector2)currentTarget.position) <= attackRange)
        {
            movementDirection = Vector2.zero;
            root.SetData("movementDirection", movementDirection);
        }


        movementDirection = (Vector2)root.GetData("movementDirection");


    }
}
