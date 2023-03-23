using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseFlock : EnemyBase
{
    [SerializeField] Transform target;
     FlockBehaviourChase flockPattern;
    public override void Init()
    {
        flockPattern = new FlockBehaviourChase();
        stats = new EnemyStats(ENEMY_TYPE.CHASE_PLAYER);
        Node attack = new Sequence(new List<Node> { new AttackFast(), new AttackHeavy(), new Evade(), new Attack() });
        
         root = new Root(new List<Node> { new ChaseTargetFLOCK(), attack, new MoveToChaseRange() });
      
    }
    public override void InitA_STAR(AiGrid2 grid)
    {
        flockPattern.SetGrid(grid);
    }
    public override void InitFlock(float sep, float sepOb, float tar, float align, float cohesive)
    {
        flockPattern.SeparationWeight= sep;
        flockPattern.SeparateOBWeight= sepOb;
        flockPattern.CohesionWeight = cohesive;
        flockPattern.AlignmentWeight= align;
        flockPattern.TargetWeight= tar;
    }
    public override void StartNightPhase()
    {
        movementDirection = (target.position - transform.position).normalized;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        root.SetData("attackRange", stats.Range);
        root.SetData("targetPosition", (Vector2)target.position);
        root.SetData("pathindex", 0);
        root.SetData("movementDirection", movementDirection);
        root.SetData("flockPattern", flockPattern);
        root.SetData("box", GetComponent<BoxCollider2D>());
        root.SetData("time", 0.0f);
        root.SetData("time", 0.0f);
        root.SetData("time", 0.0f);
        root.SetData("cellsize", cellsize);
        root.SetData("stats", stats);
        root.SetData("velocity", rb.velocity);
        root.SetData("position", (Vector2)transform.position);
        root.SetData("dynamicTarget", false);
        root.SetData("targetUpdated", true);
        root.SetData("shouldChase", true);
        root.SetData("chaseRange", stats.ChaseTargetRange);
    }
    private void Update()
    {
        root.SetData("position", (Vector2)transform.position);
        root.SetData("targetPosition", (Vector2)target.position);
        root.SetData("velocity", rb.velocity);

        if ((bool)root.GetData("shouldChase"))
        {
            movementDirection = (Vector2)root.GetData("movementDirection");
        }
        else
        {
            movementDirection = Vector2.zero;
            root.SetData("movementDirection", movementDirection);
        }
        root.Evaluate();
    }
}
