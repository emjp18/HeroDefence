using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using static State;

public class Flocking : MonoBehaviour
{
    [SerializeField]  bool leader = false;
    [SerializeField] Transform target;
    Vector2 dir;
    Vector2 currentVelocity;
    Rigidbody2D rb;
    Collider2D box;
    List<Transform> neighbours;
    float avoidRadius;
    float neighbourRange;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<Collider2D>();
        dir = (target.position - transform.position).normalized;
        avoidRadius = box.bounds.size.x*1.5f;
        neighbourRange = avoidRadius * 3;
    }
    private void FixedUpdate()
    {
        rb.velocity = dir * 100 * Time.fixedDeltaTime;
        currentVelocity = rb.velocity;
     

    }
    void Update()
    {
        if (leader)
            dir = (target.position - transform.position).normalized;
        else
            dir = Vector2.zero;
        UpdateNeighbours();
        dir += Cohesion();
        dir += Alignment();
        dir += Separation();


    }
    private void UpdateNeighbours()
    {
        neighbours.Clear();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(transform.position, neighbourRange);
        foreach (Collider2D c in contextColliders)
        {
            if (c.tag == "Flock")
                neighbours.Add(c.gameObject.transform);
        }

    }
    public Vector2 Alignment()
    {
        
        if (neighbours.Count == 0)
            return transform.forward;

       
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
    public Vector2 Separation()
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
