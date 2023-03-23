using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using static State;

public class Flocking : MonoBehaviour
{
    List<EnemyBase> flock;
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
        //dir = (target.position - transform.position).normalized;
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
        //if (leader)
        //    dir = (target.position - transform.position).normalized;
        //else
        //    dir = Vector2.zero;
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
/*
 *  public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehavior behavior;

    [Range(10, 500)]
    public int startingCount = 250;
    const float AgentDensity = 0.08f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 10f)]
    public float neighborRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    // Start is called before the first frame update
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < startingCount; i++)
        {
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                Random.insideUnitCircle * startingCount * AgentDensity,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                transform
                );
            newAgent.name = "Agent " + i;
            newAgent.Initialize(this);
            agents.Add(newAgent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);

            //FOR DEMO ONLY
            //agent.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, context.Count / 6f);

            Vector2 move = behavior.CalculateMove(agent, context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);
        }
    }

    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);
        foreach (Collider2D c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }

 
 */