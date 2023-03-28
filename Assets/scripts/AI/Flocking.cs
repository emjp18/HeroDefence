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
    protected float detectCollisionRadius;
    protected float flockradius;
    protected float avoidanceRadius;
    protected Collider2D[] nearbyColliders;
    protected float cohesionWeight;
    protected float alignmentWeight;
    protected float separationWeight;
    protected float targetWeight;
    protected float separationObstWeight;
    protected float moveRandomWeight;
    protected int flockPhysicslayer = 6;
    protected string flockTag;
    protected float smoothTime = 0.3f;
    protected Vector2 oldTargetpos = Vector2.zero;
    protected bool waitForTarget = false;
    protected Vector2 oldDir = Vector2.zero;
    protected float angleThreshold = 35;
    protected Vector2 newDirection;
    protected Vector2 obstacleCell;
    protected RectangleFloat agentBounds = new RectangleFloat();
    protected BoxCollider2D box;
    protected int flockID;
    protected bool isLeader = false;
    protected bool needsNewPath = false;
    public Vector2 ObstaclePos
    {
        get => obstacleCell;
      
    }
    public bool Leader
    {
        get => isLeader;
        set => isLeader = value;
    }
    public bool NewPath
    {
        get => needsNewPath;
        set => needsNewPath = value;
    }
    public float RandomW
    {
        get => moveRandomWeight;
        set => moveRandomWeight = value;
    }
    public float TargetW
    {
        get => targetWeight;
        set => targetWeight = value;
    }
    public float AlignW
    {
        get => alignmentWeight;
        set => alignmentWeight = value;
    }
    public float CohesiveW
    {
        get => cohesionWeight;
        set => cohesionWeight = value;
    }
    public float SeparateAgentW
    {
        get => separationWeight;
        set => separationWeight = value;
    }
    public float SeparateObstacleW
    {
        get => separationObstWeight;
        set => separationObstWeight = value;
    }

    protected void GetNearbyObjects(Vector2 pos, BoxCollider2D box)
    {

        
        nearbyAgents.Clear();

        int nr = Physics2D.OverlapCircleNonAlloc(pos, flockradius, nearbyColliders);
        for (int i = 0; i < nr; i++)
        {

            if (nearbyColliders[i].gameObject.GetComponent<EnemyBase>() == null)
                continue;

            if (nearbyColliders[i] != box && nearbyColliders[i].gameObject.tag == flockTag
           && nearbyColliders[i].gameObject.GetComponent<EnemyBase>().FlockID == flockID)
            {
                nearbyAgents.Add(nearbyColliders[i].transform);
                
            }




        }
        

    }
    public abstract Vector2 CalculateDirection(Vector2 pos, Vector2 targetPos, Vector2 currentVelocity, Vector2 currentDirection);
    
    
    public FlockBehaviour(FlockWeights weights, AiGrid grid, BoxCollider2D box, int flockAgentAmount, string tag
        ,int flockID, bool leader)
    {
        this.isLeader = leader;
        flockTag = tag;
        avoidanceRadius = box.size.x > box.size.y ? box.size.x : box.size.y;
        this.box = box;
        this.flockID = flockID;
        nearbyColliders = new Collider2D[flockAgentAmount];
        //flockradius = this.box.size.x > this.box.size.y ?
        //    this.box.size.x: this.box.size.y;
        flockradius = this.box.size.x > this.box.size.y ?
            flockAgentAmount * 0.5f * this.box.size.x : flockAgentAmount*0.5f * this.box.size.y;
        separationWeight = weights.separateAgents;
        separationObstWeight = weights.separateStatic;
        targetWeight = weights.moveToTarget;
        alignmentWeight = weights.align;
        cohesionWeight = weights.cohesive;
        moveRandomWeight = weights.random;
        detectCollisionRadius = grid.GetCellSize();
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
    public FlockBehaviourChase(FlockWeights weights, AiGrid grid, BoxCollider2D box,int flockAgentAmount, string tag,
        int flockid, bool leader) 
        : base(weights, grid,box,flockAgentAmount, tag, flockid, leader)
    {
        
    }

    public override Vector2 CalculateDirection(Vector2 pos, Vector2 targetPos,  Vector2 currentVelocity, Vector2 currentDirection)
    {
        
       
   
        GetNearbyObjects(pos, box);
        newDirection = Vector2.zero;
        
        newDirection += MoveToTarget(pos, targetPos, box, currentVelocity, currentDirection) * targetWeight;
        Debug.Log(newDirection);
        newDirection += MoveRandom(pos, targetPos, box, currentVelocity, currentDirection) * moveRandomWeight;

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

        Debug.Log(newDirection);

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
                if (!needsNewPath)
                {
                    float cos = Vector2.Dot((position - pos).normalized, (targetPos - pos).normalized);
                    if (cos > 0.707106781 && cos < 1)
                    {
                        obstacleCell = position;
                        needsNewPath = true;
                        Debug.Log("?");
                    }

                }
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

    Vector2 MoveRandom(Vector2 pos, Vector2 targetPos, BoxCollider2D box, Vector2 currentVelocity, Vector2 currentDirection)
    {

        Vector2 randDir = Random.insideUnitCircle.normalized;
       



        foreach (RectangleFloat b in nearbyObstacleBoxes)
        {
            if (Utility.PointAABBIntersectionTest(b, (randDir * b.Width) + pos))
            {

                return Vector2.zero;

            }
        }
        randDir = Vector2.SmoothDamp(currentDirection, randDir, ref currentVelocity, smoothTime);
        return randDir;
    }

}