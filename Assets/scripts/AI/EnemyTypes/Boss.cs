using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyBase
{
    GameObject block;

    public override void Init(AiGrid grid, int flockamount, int flockID, Transform hidePoint, Transform movementRangePoint, Transform player)
    {
       
        root = new Root(new List<Node> { new MoveForward(), new AttackHeavy() });
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public override void StartNightPhase(AiGrid grid)
    {
        stats = new EnemyStats(Enemytype);
  
        movementDirection = Vector2.zero;
   
        root.SetData("movementDirection", movementDirection);

        root.SetData("position", (Vector2)transform.position);
        root.SetData("dead", false);
        root.SetData("withinAttackRange", false);
        root.SetData("deactivate", false);
        root.SetData("attackHeavy", false);
        root.SetData("attacking", false);
        root.SetData("forward", (Vector2)(buildingTarget.position-transform.position).normalized);
        root.SetData("type", Enemytype);
 
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "destructable")
        {
            block = collision.gameObject;
            root.SetData("withinAttackRange", true);
        }
        
    }
    void Update()
    {
        root.SetData("position", (Vector2)transform.position);
        root.Evaluate();
        movementDirection = (Vector2)root.GetData("movementDirection");

        if ((bool)root.GetData("deactivate"))
        {
            gameObject.SetActive(false);
        }
       
         if(block!=null)
        {
            if(!block.activeSelf)
            {
                block = null;
                root.SetData("false", true);
            }
        }
    }
}
