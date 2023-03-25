using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : EnemyBase
{
   
    [SerializeField] Transform player;
    FlockBehaviourChase flockingBehavior;

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
            flockamount,gameObject.tag);
        root = new Root(new List<Node> { new Chase()});
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats.AttackRange = player.GetComponent<BoxCollider2D>().size.x;
        
    }

    public override void StartNightPhase(AiGrid grid)
    {
        movementDirection = Vector2.zero;
        root.SetData("targetPosition", (Vector2)buildingTarget.position);
        root.SetData("movementDirection", movementDirection);
        root.SetData("flockPattern", flockingBehavior);
        root.SetData("position", (Vector2)transform.position);
        root.SetData("dead", false);
        root.SetData("withinAttackRange", false);
        root.SetData("deactivate", false);
        root.SetData("attackHeavy", false);
        root.SetData("attacking", false);
        root.SetData("velocity", rb.velocity);
        root.SetData("type", Enemytype);

    
    }
    private void Update()
    {
        root.SetData("velocity", rb.velocity);

        if(Vector2.Distance(transform.position, player.position) < stats.ChasePlayerRange)
        {
            root.SetData("targetPosition", (Vector2)player.position);
        }
        else
        {
            root.SetData("targetPosition", (Vector2)buildingTarget.position);
        }

       
            
        root.SetData("position", (Vector2)transform.position);
        root.Evaluate();
        movementDirection = (Vector2)root.GetData("movementDirection");

        if ((bool)root.GetData("deactivate"))
        {
            gameObject.SetActive(false);
        }
   
        root.SetData("dead",stats.Health <= 0);
        root.SetData("withinAttackRange",
         (Vector2.Distance((Vector2)player.position, (Vector2)transform.position) < stats.AttackRange));


      
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
