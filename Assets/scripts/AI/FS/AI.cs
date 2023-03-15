using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class AI : MonoBehaviour
{
    [SerializeField] ENEMY_TYPE type = ENEMY_TYPE.CHASE_PLAYER;
    [SerializeField] Transform player;
    [SerializeField] Transform target;
    State currentState;
    Animator anim;
    AStar2D AStar;
    int pathElement = 0;
    Rigidbody2D rb;
    [SerializeField] float movementSpeed = 5;
    float thresholdToSwitchDir = 1;
    [SerializeField] AiGrid2 grid;
    float PathFindingDelay = 5.0f;
    float time = 5.0f;

    Vector2 dir = Vector2.zero;
  

    
    void Start()
    {
        anim = GetComponent<Animator>();
       
        rb = GetComponent<Rigidbody2D>();
        currentState = new Chase(gameObject, anim, player, target, false);
      
        AStar = new AStar2D(grid);
   
        if(type == ENEMY_TYPE.CHASE_PLAYER)
        {
            (currentState as Chase).SetShouldChasePlayer(true);
        }
        else
        {
            (currentState as Chase).SetShouldChasePlayer(false);
        }

    }
    private void FixedUpdate()
    {
        rb.velocity = dir * movementSpeed * Time.fixedDeltaTime;
        

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
      
    }
    public AiGrid2 GetGrid() { return grid; }
    void Update()
    {
        currentState.Process();
        if (currentState is Chase)
        {
            if(AStar.IsWithinGridBounds(transform.position))
            {
                if ((currentState as Chase).GetShouldChasePlayer())
                {
                    if (time < PathFindingDelay)
                    {
                        time += Time.deltaTime;
                    }
                    else
                    {
                        time = 0.0f;
                        
                        AStar.ResetPath();
                        AStar.AStarSearch((Vector2)transform.position, (Vector2)player.transform.position);
                    }
                    if (AStar.GetPathFound())
                    {
                        if (pathElement >= AStar.GetPath().Count)
                        {
                            //switch to attack
                            //reset pathsss
                            (currentState as Chase).ReachedEndDest = true;
                            pathElement = 0;
                            dir = Vector2.zero;
                           
                            
                        }
                        else
                        {

                            dir = (AStar.GetPath()[pathElement].pos - (Vector2)transform.position).normalized;
                            if (Vector2.Distance(transform.position, AStar.GetPath()[pathElement].pos) < thresholdToSwitchDir)
                            {
                                pathElement++;
                            }
                        }
                    }
                }
                else
                {
                    if(!AStar.GetPathFound())
                    {
                        if (time < PathFindingDelay)
                        {
                            time += Time.deltaTime;
                        }
                        else
                        {
                            time = 0.0f;
                            
                            AStar.ResetPath();
                            AStar.AStarSearch((Vector2)transform.position, (Vector2)target.transform.position);
                        }
                    }
                    else
                    {
                        if(pathElement == AStar.GetPath().Count)
                        {
                            //switch to attack
                            //reset path
                            (currentState as Chase).ReachedEndDest = true;
                            pathElement = 0;
                           
                            AStar.ResetPath();
                        }
                        else
                        {
                           
                            dir = (AStar.GetPath()[pathElement].pos - (Vector2)transform.position).normalized;
                            if (Vector2.Distance(transform.position, AStar.GetPath()[pathElement].pos) < thresholdToSwitchDir)
                            {
                                pathElement++;
                            }
                        }
                      
                        
                    }
                    
                }
            }
            else
            {
                dir = (grid.GetCenter() - (Vector2)transform.position).normalized;
            }

            
           
        }
        
        
    }
    
}
