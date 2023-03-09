using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class AI : MonoBehaviour
{
   
    [SerializeField] Transform player;
    [SerializeField] Transform target;
    State currentState;
    Animator anim;
    AStar2D AStar;
    int pathElement = 0;
    Rigidbody2D rb;
    [SerializeField] float movementSpeed = 5;
    //[SerializeField] AIGrid grid;
    [SerializeField] AiGrid2 grid;
    float PathFindingDelay = 2.50f;
    //float PathFindingDelay2 = 6.0f;
    //float playerMinRange = 20;
    float time = 5.0f;
    //float time2 = 0.0f;
    Vector2 dir = Vector2.zero;
    //bool collisionHit = false;
    //bool switchTarget = false;
    Vector2 collisionDir;
    Vector2 destination;
    
    void Start()
    {
        anim = GetComponent<Animator>();
       
        rb = GetComponent<Rigidbody2D>();
        currentState = new Chase(gameObject, anim, player, target, false);
      
        AStar = new AStar2D(grid);
        destination = transform.position;
        

    }
    private void FixedUpdate()
    {
        rb.velocity = dir * movementSpeed * Time.fixedDeltaTime;
        

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.layer);
    }
    public AiGrid2 GetGrid() { return grid; }
    void Update()
    {
        currentState = currentState.Process();
        dir = Vector2.zero;

        if(currentState.name==State.STATE.CHASE)
        {
            if(Vector2.Distance(transform.position, target.position)>Vector2.Distance(transform.position, player.position))
            {
                (currentState as Chase).SetShouldChasePlayer(true);
            }
            else
            {
                (currentState as Chase).SetShouldChasePlayer(false);
            }
            if(AStar.IsWithinGridBounds(transform.position))
            {
                if (time > PathFindingDelay)
                {
                    //collisionHit = false;
                    time = 0;
                    if((currentState as Chase).GetShouldChasePlayer())
                    {
                        pathElement = 0;
                        AStar.ResetPath();
                        AStar.AStarSearch(transform.position, player.transform.position);
                    }
                    else
                    {
                        pathElement = 0;
                        AStar.ResetPath();
                        AStar.AStarSearch(transform.position, target.transform.position);
                    }
                    


                }
                else
                {
                    time += Time.deltaTime;
                }
                if (AStar.GetPathFound())
                {

                    destination = AStar.GetPath()[pathElement].pos;
                    dir = destination - new Vector2(transform.position.x, transform.position.y);
                    dir.Normalize();

                    if (Vector2.Distance(transform.position, destination) < 1)
                    {
                        pathElement++;
                        

                    }
                    if (pathElement >= AStar.GetPath().Count)
                    {

                        pathElement = 0;
                        AStar.ResetPath();
                        (currentState as Chase).ReachedEndDest = true;
                    }
                }
            }
            else
            {
                dir = grid.GetCenter() - new Vector2(transform.position.x, transform.position.y).normalized;
            }
            
            
        }
        
    }
    public void SetTarget(Transform target)
    {
        this.target = target;
        //switchTarget = true;


    }
}
