using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.Tilemaps;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Swarmer : EnemyBase
{
    [SerializeField] float speed = 150;
    [SerializeField] Transform movementRangePoint;
    [SerializeField] Transform hidePoint;
    [SerializeField] Transform player;
    FlockBehaviourChase flockingBehavior;
    bool outOfRange = false;
    public override void Init( AiGrid grid, int flockamount)
    {
        flockweights = new FlockWeights();
        flockweights.random = RandomW;
        flockweights.align = AlignW;
        flockweights.separateAgents = SeparateAgentW;
        flockweights.separateStatic = SeparateObstacleW;
        flockweights.cohesive = CohesiveW;
        flockweights.moveToTarget = TargetW;
        stats = new EnemyStats(Enemytype);
        var box = GetComponent<BoxCollider2D>();
        flockingBehavior = new FlockBehaviourChase(flockweights, grid, box,
            flockamount, gameObject.tag);
        root = new Root(new List<Node> { new ChaseWithinArea(), new Idle()
        });
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats.AttackRange = player.GetComponent<BoxCollider2D>().size.x;
        stats.MovementRange = (movementRangePoint.position - hidePoint.position).magnitude;
        stats.Speed = speed;

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
        root.SetData("withinChaseRange", false);
        root.SetData("withinAttackRange", false);
        root.SetData("deactivate", false);
        root.SetData("attackHeavy", false);
        root.SetData("attacking", false);
        root.SetData("velocity", rb.velocity);
        root.SetData("type", Enemytype);
        root.SetData("flockWeights", flockweights);
        root.SetData("hidePoint", (Vector2)hidePoint.position);
        root.SetData("moveToCenter", false);

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
        root.SetData("withinChaseRange",
            (Vector2.Distance((Vector2)player.position, (Vector2)hidePoint.position) < stats.MovementRange));
        root.SetData("dead",
       stats.Health<=0);
        root.SetData("withinAttackRange",
         (Vector2.Distance((Vector2)player.position, (Vector2)transform.position) < stats.AttackRange));


        outOfRange =
            Vector2.Distance((Vector2)transform.position, (Vector2)hidePoint.position) > stats.MovementRange;
        
        if ((bool)root.GetData("withinAttackRange"))
        {
            int r = Random.Range(0, 2);
            if (r == 0)
            {
                root.SetData("attackHeavy", true);
            }
            else
            {
                root.SetData("attackHeavy", false);
            }
        }
        if((bool)root.GetData("withinAttackRange"))
        {
            root.SetData("movementDirection", Vector2.zero);
        }
        if(outOfRange)
        {
            root.SetData("moveToCenter", true);
        }
        if((bool)root.GetData("moveToCenter"))
        {
            if(Vector2.Distance((Vector2)transform.position, (Vector2)hidePoint.position)<1)
            {
                root.SetData("moveToCenter", false);
            }
        }
    }
}
