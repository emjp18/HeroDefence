using BehaviorTree;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.Rendering.LookDev;
using UnityEngine;



public abstract class FlockBehaviour
{
    protected List<Transform> nearbyAgents = new List<Transform>();
    protected List<Vector2> nearbyObstacles = new List<Vector2>();
    protected List<RectangleFloat> nearbyObstacleBoxes = new List<RectangleFloat>();
    protected float detectCollisionRadius = 6;
    protected float avoidanceRadius = 3;
    protected Collider2D[] nearbyColliders = new Collider2D[20];
    protected float cohesionWeight = 1.6f;
    protected float alignmentWeight = 1.25f;
    protected float separationWeight = 3.0f;
    protected float targetWeight = 1;
    protected float separationObstWeight = 2.0f;
    protected int flockPhysicslayer = 6;
    protected float smoothTime = 0.3f;
    protected Vector2 oldTargetpos = Vector2.zero;
    protected bool waitForTarget = false;
    protected Vector2 oldDir = Vector2.zero;
    protected float angleThreshold = 35;
    protected Vector2 newDirection;
    protected RectangleFloat agentBounds = new RectangleFloat();
    
   
    
    
    protected void GetNearbyObjects(Vector2 pos, BoxCollider2D box)
    {
       


        nearbyAgents.Clear();




        int nr = Physics2D.OverlapCircleNonAlloc(pos, detectCollisionRadius, nearbyColliders);
        for (int i = 0; i < nr; i++)
        {
            if (nearbyColliders[i] != box && nearbyColliders[i].gameObject.layer == flockPhysicslayer)
            {
                nearbyAgents.Add(nearbyColliders[i].transform);
            }
           
        }

    }
    public abstract Vector2 CalculateDirection(Vector2 pos, Vector2 targetPos, BoxCollider2D box,Vector2 currentVelocity, Vector2 currentDirection);
    
    
    public FlockBehaviour(FlockWeights weights, AiGrid grid)
    {
        separationWeight = weights.separateAgents;
        separationObstWeight = weights.separateStatic;
        targetWeight = weights.moveToTarget;
        alignmentWeight = weights.align;
        cohesionWeight = weights.cohesive;

        detectCollisionRadius = grid.GetCellSize() * 1.5f;
        foreach (A_STAR_NODE node in grid.GetCustomGrid())
        {
            if (node.obstacle)
            {
                nearbyObstacles.Add(node.pos);
                RectangleFloat bounds = new RectangleFloat();
                bounds.X = node.pos.x - grid.GetCellSize() * 0.5f;
                bounds.Y = node.pos.y + grid.GetCellSize() * 0.5f;
                bounds.Width = bounds.Height = grid.GetCellSize();
                nearbyObstacleBoxes.Add(bounds);
            }
        }
    }
}

public class FlockBehaviourChase : FlockBehaviour
{
    public FlockBehaviourChase(FlockWeights weights, AiGrid grid) : base(weights, grid)
    {
    }

    public override Vector2 CalculateDirection(Vector2 pos, Vector2 targetPos, BoxCollider2D box, Vector2 currentVelocity, Vector2 currentDirection)
    {
        
        avoidanceRadius =  box.size.x > box.size.y ? box.size.x : box.size.y;
        avoidanceRadius *= 1.25f;
        GetNearbyObjects(pos, box);
        newDirection = Vector2.zero;

        newDirection += MoveToTarget(pos, targetPos, box,currentVelocity,currentDirection) * targetWeight;

 

        var cohesion = Cohesion(pos, currentVelocity, currentDirection) * cohesionWeight;

        if (cohesion.magnitude > cohesionWeight * cohesionWeight)
        {
            cohesion.Normalize();
            cohesion *= cohesionWeight;
        }
        newDirection += cohesion;

        var alignment = Alignment(currentVelocity, currentDirection) * alignmentWeight;

        if (alignment.magnitude > alignmentWeight * alignmentWeight)
        {
            alignment.Normalize();
            alignment *= alignmentWeight;
        }
        newDirection += alignment;

        var separation = Separation(pos, currentVelocity, currentDirection) * separationWeight;

        if (separation.magnitude > separationWeight * separationWeight)
        {
            separation.Normalize();
            separation *= separationWeight;
        }
        newDirection += separation;
        var separationOb = SeparationObstacles(pos, targetPos, currentVelocity, currentDirection) * separationObstWeight;

        if (separationOb.magnitude > separationObstWeight * separationObstWeight)
        {
            separationOb.Normalize();
            separationOb *= separationObstWeight;
        }
        newDirection += separationOb;
        

        return newDirection.normalized;



    }
    Vector2 Alignment(Vector2 currentVelocity, Vector2 currentDirection)
    {
        if (nearbyAgents.Count == 0)
            return Vector2.zero;

  
        Vector2 alignmentMove = Vector2.zero;
        //Filter();
        foreach (Transform item in nearbyAgents)
        {
            alignmentMove += item.gameObject.GetComponent<EnemyBase>().Movement;
        }
        alignmentMove /= nearbyAgents.Count;
        alignmentMove = Vector2.SmoothDamp(currentDirection, alignmentMove, ref currentVelocity, smoothTime);
        return alignmentMove;
    }
   
    Vector2 SeparationObstacles(Vector2 pos, Vector2 targetPos, Vector2 currentVelocity, Vector2 currentDirection)
    {
        if(nearbyObstacles.Count == 0)
            return Vector2.zero;

  
        Vector2 avoidanceMove = Vector2.zero;
        int nAvoid = 0;
   
        foreach (Vector2 position in nearbyObstacles)
        {
         
            if ((position - pos).magnitude < detectCollisionRadius)
            {
                nAvoid++;
                avoidanceMove += (pos - position);
            }
        }
        if (nAvoid > 0)
        {
            avoidanceMove /= nAvoid;
            waitForTarget = true;
            oldDir = (targetPos - pos).normalized;
            
        }
       

        
        return avoidanceMove;
    }
    Vector2 Separation(Vector2 pos, Vector2 currentVelocity, Vector2 currentDirection)
    {
        if (nearbyAgents.Count == 0)
            return Vector2.zero;


        Vector2 avoidanceMove = Vector2.zero;
        int nAvoid = 0;
    
        foreach (Transform t in nearbyAgents)
        {

            if (((Vector2)t.position - pos).magnitude < avoidanceRadius)
            {
                nAvoid++;
                avoidanceMove += (pos - (Vector2)t.position);
            }
        }
        if (nAvoid > 0)
        {
            avoidanceMove /= nAvoid;

           
        }
      



        return avoidanceMove;
    }
    Vector2 Cohesion(Vector2 pos, Vector2 currentVelocity, Vector2 currentDirection)
    {
     
        if (nearbyAgents.Count == 0)
            return Vector2.zero;

   
        Vector2 cohesionMove = Vector2.zero;
       //Filter();
        foreach (Transform item in nearbyAgents)
        {
            cohesionMove += (Vector2)item.position;
        }
        cohesionMove /= nearbyAgents.Count;


        cohesionMove -= pos;
   
        return cohesionMove;
    }
    Vector2 MoveToTarget(Vector2 pos, Vector2 targetPos, BoxCollider2D box, Vector2 currentVelocity, Vector2 currentDirection)
    {
        
        Vector2 dir = (targetPos - pos).normalized;
        if (waitForTarget)
        {
            float dot = Vector2.Dot(dir, oldDir);
            float angle = Mathf.Acos(dot);
            angle *= Mathf.Rad2Deg;

            if (oldTargetpos != targetPos && Mathf.Abs(angle) > angleThreshold && angle != float.NaN)
            {
               
                oldDir = dir;
                waitForTarget = false;
                oldTargetpos = targetPos;
            }
            else
            {
                return Vector2.zero;
            }

            


        }
        


        foreach (RectangleFloat b in nearbyObstacleBoxes)
        {
            if (Utility.PointAABBIntersectionTest(b, (dir* b.Width)+pos))
            {
      
                return Vector2.zero;
                
            }
        }
        dir = Vector2.SmoothDamp(currentDirection, dir, ref currentVelocity, smoothTime);
        return dir;
    }
    
   
}