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
    FlockWeights flockweights;
    public override void Init(FlockWeights flockweights, AiGrid grid, int flockamount)
    {
        stats = new EnemyStats(Enemytype);
        var box = GetComponent<BoxCollider2D>();
        flockingBehavior = new FlockBehaviourChase(flockweights, grid, box,
            flockamount);
        root = new Root(new List<Node> { new ChaseWithinArea(), new Selector(new List<Node>{new AttackFast(),
        new AttackHeavy()}), new Idle(),
        new Death()});
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats.AttackRange = player.GetComponent<BoxCollider2D>().size.x;
        this.flockweights = flockweights;


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
        root.SetData("outOfRange", false);
        root.SetData("hidePoint", (Vector2)hidePoint.position);
        
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


        root.SetData("outOfRange",
            Vector2.Distance((Vector2)transform.position, (Vector2)hidePoint.position) >= stats.MovementRange*0.75f);
        
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

      
    }
}
