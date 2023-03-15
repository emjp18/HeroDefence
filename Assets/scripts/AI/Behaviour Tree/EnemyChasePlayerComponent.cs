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
    bool isNight = false;
    Root root;
    [SerializeField] float waitTime = 4.0f;
    [SerializeField] float thresholdDistance = 1;
    public bool night
    {
        get { return isNight; }
        set { isNight= value; }
    }
   
    private void FixedUpdate()
    {
     
        rb.velocity = movementDirection * movementSpeed * Time.fixedDeltaTime;
 

    }
   
    private void Start()
    {
        Node attack = new Sequence(new List<Node> { new AttackFast(), new AttackHeavy(), new Evade(), new Attack() });
        Node chase = new Selector(new List<Node> { new ChaseGrid(), new ChaseTarget() });
        root = new Root(new List<Node> { chase,attack });
        AStarFunctionality = new AStar2D(AIPathGrid);
        root.SetData("attackRange", attackRange);
        root.SetData("targetPosition", (Vector2)player.position);
     
        root.SetData("movementDirection", movementDirection);
        root.SetData("AStar2D", AStarFunctionality);
    
        root.SetData("gridCenter", AIPathGrid.GetCenter());
        root.SetData("withinGrid", false);
        root.SetData("dynamicTarget", true);
        root.SetData("waitTime", waitTime);
        root.SetData("thresholdDistance", thresholdDistance);
        //anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        root.SetData("position", (Vector2)transform.position);
 
    }
    private void Update()
    {
        root.SetData("withinGrid", AStarFunctionality.IsWithinGridBounds(transform.position));
        root.SetData("position", (Vector2)transform.position);
        root.SetData("targetPosition", (Vector2)player.position);
        
        if (root != null)
            root.Evaluate();

        movementDirection = (Vector2)root.GetData("movementDirection");

    }
    
}
