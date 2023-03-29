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
    protected float followLeaderWeight;
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
    protected Vector2 leaderPos;
    protected bool setnewpath = true;
    protected bool attacking = false;
    protected AiGrid grid;
    protected QUAD_NODE rootNode; //Todo separate into classes
    protected List<Vector2> temp = new List<Vector2>();
    protected List<Vector2> temp2 = new List<Vector2>();
    protected List<float> temp3 = new List<float>();
    public Vector2 ObstaclePos
    {
        get => obstacleCell;
      
    }
    public bool Attacking
    {
        get => attacking;
        set => attacking = value;
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
    public float LeaderW
    {
        get => followLeaderWeight;
        set => followLeaderWeight = value;
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
    public void UpdateStaticCollision()
    {
        List<Vector2> temp0 = new List<Vector2>();
        List<Vector2> temp1 = new List<Vector2>();
        List<Vector2> temp2 = new List<Vector2>();
        List<Vector2> temp3 = new List<Vector2>();
      
        
        
        foreach (A_STAR_NODE node in grid.GetCustomGrid())
        {
            if (node.obstacle && node.pos.x <= node.bounds.Width * grid.GetCustomGrid().GetLength(0) * 0.5f//left, down
                || node.obstacle && node.pos.y <= node.bounds.Height * grid.GetCustomGrid().GetLength(1) * 0.5f)
            {
                temp0.Add(node.pos);
            }
            if (node.obstacle && node.pos.x > node.bounds.Width * grid.GetCustomGrid().GetLength(0) * 0.5f //right, down
               || node.obstacle && node.pos.y <= node.bounds.Height * grid.GetCustomGrid().GetLength(1) * 0.5f)
            {
                temp1.Add(node.pos);
            }
            if (node.obstacle && node.pos.x <= node.bounds.Width * grid.GetCustomGrid().GetLength(0) * 0.5f //left up
                || node.obstacle && node.pos.y > node.bounds.Height * grid.GetCustomGrid().GetLength(1) * 0.5f)
            {
                temp2.Add(node.pos);
            }
            if (node.obstacle && node.pos.x > node.bounds.Width * grid.GetCustomGrid().GetLength(0) * 0.5f //right, up
                || node.obstacle && node.pos.y > node.bounds.Height * grid.GetCustomGrid().GetLength(1) * 0.5f)
            {
                temp3.Add(node.pos);
            }
        }
        Debug.Log(rootNode.children[0].bounds);
        Utility.staticObstacles.Add(rootNode.children[0].bounds,temp2);
        Utility.staticObstacles.Add(rootNode.children[3].bounds, temp0);
        Utility.staticObstacles.Add(rootNode.children[2].bounds,temp1);
        Utility.staticObstacles.Add(rootNode.children[1].bounds,temp3);
    }
    public void UpdateGrid(AiGrid grid)
    {
        this.grid = grid;
        rootNode = grid.Getroot();
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
    public abstract Vector2 CalculateDirection(Vector2 pos, Vector2 targetPos, Vector2 currentVelocity, Vector2 currentDirection, Vector2 leaderPos);
    
    public void AddCollision(AiGrid grid)
    {
        nearbyObstacles.Clear();
        foreach (A_STAR_NODE node in grid.GetCustomGrid())
        {
            if (node.obstacle)
            {
                nearbyObstacles.Add(node.pos);
                
                nearbyObstacleBoxes.Add(node.bounds);
            }
        }
    }
    protected Vector2 Alignment(Vector2 currentVelocity, Vector2 currentDirection)
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
        //alignmentMove = Vector2.SmoothDamp(currentDirection, alignmentMove, ref currentVelocity, smoothTime);
        return alignmentMove;
    }

    protected Vector2 SeparationObstacles(Vector2 pos, Vector2 targetPos, Vector2 currentVelocity, Vector2 currentDirection)
    {
        if (nearbyObstacles.Count == 0)
            return Vector2.zero;


        Vector2 avoidanceMove = Vector2.zero;
        int nAvoid = 0;
        
        foreach (Vector2 position in nearbyObstacles)
        {

            if ((position - pos).magnitude <= detectCollisionRadius)
            {
                nAvoid++;
                avoidanceMove += (pos - position);

                if (!needsNewPath/*&&isLeader*/)
                {
                    float cos = Vector2.Dot((position - pos).normalized, (targetPos - pos).normalized);
                    if (cos < Mathf.PI * 0.5f && cos >= 0)
                    {
                        obstacleCell = position;
                        needsNewPath = true;
                        //return Vector2.zero;

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
    protected Vector2 AvoidStaticObstacles(Vector2 pos)
    {
        temp.Clear();
        int depth = 0;
        Utility.FindObstaclesFromNode(pos, rootNode,ref depth, ref temp);

        if (temp.Count == 0)
            return Vector2.zero;


        Vector2 avoidanceMove = Vector2.zero;

        
        float sum = 0;
        int c = 0;
        foreach (Vector2 obstacle in temp)
        {
            if ((pos - obstacle).magnitude < avoidanceRadius)
            {
                c++;
                sum += (pos - obstacle).magnitude;
                temp2.Add((pos - obstacle));
                temp3.Add((pos - obstacle).magnitude);
            }
        }
     
     
        for (int i=0; i<c; i++)
        {
            float properMagnitude = temp3[i] / sum * 100;
            avoidanceMove += (temp2[i].normalized * properMagnitude);
            

        }

        return avoidanceMove.normalized;
    }
    //protected Vector2 SeparationObstacles2(Vector2 pos, Vector2 targetPos, Vector2 currentVelocity, Vector2 currentDirection)
    //{
    //    Vector2Int index = Vector2Int.zero;
    //    Utility.GetAIGridIndex(pos, this.rootNode, ref index);
    //    int columns = grid.GetCustomGrid().GetLength(1);
    //    int rows = grid.GetCustomGrid().GetLength(0);
    //    for (int i = 0; i < 8; i++)
    //    {
    //        bool obstacle = false;


    //        if (index.y < columns - 1)
    //        {
    //            Utility.FindNearbyStaticObstacles(grid.GetCustomGrid()
    //            [index.x, index.y + 1].pos, rootNode, grid, out obstacle);
    //        }

    //        if (y < columns - 1 && x < rows - 1)
    //        {
    //            Utility.FindNearbyStaticObstacles(grid.GetCustomGrid()
    //          [index.x+1, index.y + 1].pos, rootNode, grid, out obstacle);
    //        }

    //        if (x < rows - 1)
    //        {
    //            Utility.FindNearbyStaticObstacles(grid.GetCustomGrid()
    //          [index.x + 1, index.y + 1].pos, rootNode, grid, out obstacle);
    //        }
    //            customGrid[x + 1, y].obstacle = true;

    //        if (x < rows - 1 && y > 0)
    //        {

    //        }
    //            customGrid[x + 1, y - 1].obstacle = true;
    //        if (y > 0)
    //        {

    //        }
    //            customGrid[x, y - 1].obstacle = true;
    //        if (x > 0 && y > 0)
    //        {

    //        }
    //            customGrid[x - 1, y - 1].obstacle = true;
    //        if (x > 0)
    //        {

    //        }
    //            customGrid[x - 1, y].obstacle = true;
    //        if (x > 0 && y < columns - 1)
    //        {

    //        }
    //            customGrid[x - 1, y + 1].obstacle = true;
    //    }

    //}
    protected Vector2 Separation(Vector2 pos, Vector2 currentVelocity, Vector2 currentDirection)
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
    protected Vector2 Cohesion(Vector2 pos, Vector2 currentVelocity, Vector2 currentDirection)
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
    protected Vector2 MoveToTarget(Vector2 pos, Vector2 targetPos, BoxCollider2D box, Vector2 currentVelocity, Vector2 currentDirection)
    {

        Vector2 dir = (targetPos - pos).normalized;
        //if (waitForTarget)
        //{
        //    float dot = Vector2.Dot(dir, oldDir);
        //    float angle = Mathf.Acos(dot);
        //    angle *= Mathf.Rad2Deg;

        //    if (oldTargetpos != targetPos && Mathf.Abs(angle) > angleThreshold && angle != float.NaN)
        //    {

        //        oldDir = dir;
        //        waitForTarget = false;
        //        oldTargetpos = targetPos;
        //    }
        //    else
        //    {
        //        return Vector2.zero;
        //    }

        //}



        //foreach (RectangleFloat b in nearbyObstacleBoxes)
        //{
        //    if (Utility.PointAABBIntersectionTest(b, (dir * b.Width) + pos))
        //    {

        //        return Vector2.zero;

        //    }
        //}

        //dir = Vector2.SmoothDamp(currentDirection, dir, ref currentVelocity, smoothTime);

        return dir;
    }

    protected Vector2 MoveRandom(Vector2 pos, Vector2 targetPos, BoxCollider2D box, Vector2 currentVelocity, Vector2 currentDirection)
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
    protected Vector2 FollowLeader(Vector2 pos, Vector2 targetPos, BoxCollider2D box, Vector2 currentVelocity, Vector2 currentDirection)
    {

        Vector2 movement = (leaderPos - pos).normalized;


        movement = Vector2.SmoothDamp(currentDirection, movement, ref currentVelocity, smoothTime);

        return movement;
    }
    public FlockBehaviour(FlockWeights weights, AiGrid grid, BoxCollider2D box, int flockAgentAmount, string tag
        ,int flockID, bool leader, QUAD_NODE root)
    {
        this.isLeader = leader;
        flockTag = tag;
        avoidanceRadius = box.size.x > box.size.y ? box.size.x : box.size.y;
        avoidanceRadius *= 0.55f;
        this.box = box;
        this.flockID = flockID;
        nearbyColliders = new Collider2D[flockAgentAmount];
        //flockradius = this.box.size.x > this.box.size.y ?
        //    this.box.size.x: this.box.size.y;
        flockradius = this.box.size.x > this.box.size.y ?
            3 * this.box.size.x : 3 * this.box.size.y;
        separationWeight = weights.separateAgents;
        separationObstWeight = weights.separateStatic;
        targetWeight = weights.moveToTarget;
        alignmentWeight = weights.align;
        cohesionWeight = weights.cohesive;
        moveRandomWeight = weights.random;
        followLeaderWeight = weights.leader;
        detectCollisionRadius = grid.GetCellSize();
        AddCollision(grid);

    }
}

public class FlockBehaviourChase : FlockBehaviour
{
    public FlockBehaviourChase(FlockWeights weights, AiGrid grid, BoxCollider2D box,int flockAgentAmount, string tag,
        int flockid, bool leader, QUAD_NODE root) 
        : base(weights, grid,box,flockAgentAmount, tag, flockid, leader, root)
    {
        
    }

    public override Vector2 CalculateDirection(Vector2 pos, Vector2 targetPos,  Vector2 currentVelocity, Vector2 currentDirection, Vector2 leaderPos)
    {
        GetNearbyObjects(pos, box);
        newDirection = Vector2.zero;
        this.leaderPos = leaderPos;

        var coh = (Cohesion(pos, currentVelocity, currentDirection).normalized) * cohesionWeight;
        newDirection += coh;
        var align = (Alignment(currentVelocity, currentDirection).normalized) * alignmentWeight;
        newDirection += align;
        var sepA = (Separation(pos, currentVelocity, currentDirection).normalized) * separationWeight;
        newDirection += sepA;
        var sepO = (SeparationObstacles(pos, targetPos, currentVelocity, currentDirection).normalized) * separationObstWeight;
        newDirection += sepO;
        var moveT = (MoveToTarget(pos, targetPos, box, currentVelocity, currentDirection).normalized) * targetWeight;
        newDirection += moveT;
        newDirection = Vector2.SmoothDamp(currentDirection, newDirection, ref currentVelocity, smoothTime);
        return newDirection.normalized;


    }
   

}
public class FlockBehaviourAvoidance : FlockBehaviour
{
    public FlockBehaviourAvoidance(FlockWeights weights, AiGrid grid, BoxCollider2D box, int flockAgentAmount, string tag,
        int flockid, bool leader, QUAD_NODE root)
        : base(weights, grid, box, flockAgentAmount, tag, flockid, leader, root)
    {

    }
    public override Vector2 CalculateDirection(Vector2 pos, Vector2 targetPos, Vector2 currentVelocity,
        Vector2 currentDirection, Vector2 leaderPos)
    {

        return AvoidStaticObstacles(pos);


    }
}