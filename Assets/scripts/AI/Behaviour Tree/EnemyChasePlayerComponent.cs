using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;


public class EnemyChasePlayerComponent : MonoBehaviour
{
    [SerializeField] float attackRange = 4;
    [SerializeField] float movementSpeed = 100;
    [SerializeField] Transform player;
    [SerializeField] AiGrid2 AIPathGrid;
    AStar2D AStarFunctionality;
    Animator anim;
    Vector2 movementDirection = Vector2.zero;
    Rigidbody2D rb;

    Root root;
    [SerializeField] float waitTime = 4.0f;
    float cellsize;
    EnemyStats stats;
    
   
    private void FixedUpdate()
    {

        rb.velocity = movementDirection * movementSpeed * Time.fixedDeltaTime;
      
    }
   
  
    public void StartNightPhase() //This should be called without any other movement from the player. Otherwise it will lag.
    {
        cellsize = AIPathGrid.GetCellSize();
        stats = new EnemyStats(ENEMY_TYPE.CHASE_PLAYER);
        Node attack = new Sequence(new List<Node> { new AttackFast(), new AttackHeavy(), new Evade(), new Attack() });
        Node chase = new Selector(new List<Node> { new ChaseGrid(), new ChaseTarget(), new Death() });
        root = new Root(new List<Node> { chase, attack, new Idle() });
        AStarFunctionality = new AStar2D(AIPathGrid);
        root.SetData("attackRange", attackRange);
        root.SetData("targetPosition", (Vector2)player.position);
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
        AStarFunctionality.UpdateGrid(AIPathGrid);
        AStarFunctionality.ResetPath();
        AStarFunctionality.AStarSearch((Vector2)transform.position, (Vector2)player.position, 4096);
    }
    private void Update()
    {
        root.SetData("withinGrid", AStarFunctionality.IsWithinGridBounds(transform.position));
        root.SetData("position", (Vector2)transform.position);
        root.SetData("targetPosition", (Vector2)player.position);

        if (root != null)
            root.Evaluate();
        if (!AStarFunctionality.GetPathFound() ||
              Vector2.Distance((Vector2)transform.position, (Vector2)player.position) <= attackRange)
        {
            movementDirection = Vector2.zero;
            root.SetData("movementDirection", movementDirection);
        }



        movementDirection = (Vector2)root.GetData("movementDirection");


    }
    
}
