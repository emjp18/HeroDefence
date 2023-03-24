using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.Tilemaps;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Swarmer : EnemyBase
{
    [SerializeField] Transform hidePoint;
    [SerializeField] Transform player;
    FlockBehaviourChase flockingBehavior;
    public override void Init(FlockWeights flockweights, AiGrid grid)
    {
        stats = new EnemyStats(Enemytype);
        flockingBehavior = new FlockBehaviourChase(flockweights, grid);
        root = new Root(new List<Node> { new ChaseWithinArea(), new AttackFast(), new Idle(),
        new Death()});
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats.Range = 5;
       
    }

    public override void StartNightPhase(AiGrid grid)
    {
        movementDirection = Vector2.zero;
       
        root.SetData("targetPosition", (Vector2)player.position);
        root.SetData("movementDirection", movementDirection);
        root.SetData("flockPattern", flockingBehavior);
        root.SetData("box", GetComponent<BoxCollider2D>());
        root.SetData("position", (Vector2)transform.position);
        root.SetData("dead", false);
        root.SetData("withinRange", false);
        root.SetData("withinAttackRange", false);
        root.SetData("deactivate", false);
       
        root.SetData("velocity", rb.velocity);
    }

    void Update()
    {
        root.SetData("velocity", rb.velocity);
        root.SetData("targetPosition", (Vector2)player.position);
        root.SetData("position", (Vector2)transform.position);
        root.Evaluate();
        movementDirection = (Vector2)root.GetData("movementDirection");

        if((bool)root.GetData("deactivate"))
        {
            gameObject.SetActive(false);
        }
        root.SetData("withinRange",
            (Vector2.Distance((Vector2)player.position, (Vector2)hidePoint.position) < stats.MovementRange));
        root.SetData("dead",
       stats.Health<=0);
        root.SetData("withinAttackRange",
         (Vector2.Distance((Vector2)player.position, (Vector2)transform.position) < stats.Range));
    }
}
