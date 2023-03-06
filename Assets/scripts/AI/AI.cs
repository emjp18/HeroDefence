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
    double PathFindingDelay = 5.0;
    double time = 5.0;

    void Start()
    {
        //anim = GetComponent<Animator>();
       
        rb = GetComponent<Rigidbody2D>();
        currentState = new Chase(gameObject, anim, player, target);
      
        AStar = new AStar2D(grid);



    }

 
    void Update()
    {
        currentState = currentState.Process();
        
        if(currentState.GetType() == typeof(Chase))
        {
            
           
            if(AStar.GetPathFound())
            {
                
                var path = AStar.GetPath();
                if(pathElement<path.Count)
                {
                    Vector2 dest = path[pathElement].pos;
                    
                    Vector2 dir = dest - new Vector2(rb.position.x, rb.position.y);
                    float distane = dest.magnitude;
                    dir.Normalize();
                    rb.velocity = dir * movementSpeed;

                    
                    if ((dest - new Vector2(rb.position.x, rb.position.y)).magnitude < 0.10f)
                    {
                        pathElement++;
                        
                      
                    }
                }
                else
                {
                    (currentState as Chase).ReachedEndDest = true;
                }
                
            }
            else
            {
                
                if(time>=PathFindingDelay)
                {
                    time = 0.0;
                    AStar.ResetPath();
                    AStar.AStarSearch(transform.position,target.transform.position);
                }
                else
                {
                    time += Time.deltaTime;
                }
               
            }
        }
        
    }
    public void SetTarget(Transform target)
    {
        this.target = target;
  
    }
}
