using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using static State;

public class Flocking : MonoBehaviour
{
    [SerializeField] Transform target;
    Vector2 dir;
    Vector2 currentVelocity;
    Rigidbody2D rb;
    Collider2D box;
    List<Transform> neighbours;
    float avoidRadius;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<Collider2D>();
        dir = (target.position - transform.position).normalized;
        avoidRadius = box.bounds.size.x*1.5f;
    }
    private void FixedUpdate()
    {

        currentVelocity = rb.velocity;

    }
    void Update()
    {
        UpdateNeighbours();
    }
    private void UpdateNeighbours()
    {
        
    }
    public Vector2 Alignment()
    {
        //if no neighbors, maintain current alignment
        if (neighbours.Count == 0)
            return transform.forward;

        //add all points together and average
        Vector2 alignmentMove = Vector2.zero;

        foreach (Transform item in neighbours)
        {
            alignmentMove += (Vector2)item.transform.up;
        }
        alignmentMove /= neighbours.Count;
        return alignmentMove;
    }
    public Vector2 Cohesion()
    {

        if (neighbours.Count == 0)
        {
           
            return Vector2.zero;
        }
            


        Vector2 cohesionMove = Vector2.zero;

        foreach (Transform item in neighbours)
        {
            cohesionMove += (Vector2)item.position;
        }
        cohesionMove /= neighbours.Count;
        //dir * 200 * Time.fixedDeltaTime;

        cohesionMove -= (Vector2)transform.position;
        cohesionMove = Vector2.SmoothDamp(transform.up, cohesionMove, ref currentVelocity, 0.3f);
        return cohesionMove;


    }
    public Vector2 Avoidance()
    {

       
        if (neighbours.Count == 0)
            return Vector2.zero;

    
        Vector2 avoidanceMove = Vector2.zero;
        int nAvoid = 0;
   
        foreach (Transform item in neighbours)
        {
            if (Vector2.SqrMagnitude(item.position - transform.position) < avoidRadius)
            {
                nAvoid++;
                avoidanceMove += (Vector2)(transform.position - item.position);
            }
        }
        if (nAvoid > 0)
            avoidanceMove /= nAvoid;

        return avoidanceMove;


    }
}
