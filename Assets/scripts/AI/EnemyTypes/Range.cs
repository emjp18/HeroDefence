using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;

public class Range : EnemyBase
{
    Vector2 velocity = Vector2.zero;
    Vector2 oldvelocity = Vector2.one;
    float time = 0;
    float idleDelay = 10;
    int idleCount = 0;
    int idleMaxCount = 3;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 vel = rb.velocity;
        Vector2 avoid = (Vector2)transform.position - collision.GetContact(0).point.normalized;
        Vector2.SmoothDamp(avoidanceForce, avoid, ref vel, 0.3f);
        if (collision.gameObject.layer == 7 && (bool)root.GetData("checkCollision"))
        {
            root.SetData("oldTarget", (Vector2)root.GetData("targetPosition"));
            root.SetData("newPath", true);
            root.SetData("checkCollision", false);
            root.SetData("movementDirection", Vector2.zero);

        }

    }
    //

    public override void StartNightPhase(AiGrid grid)
    {
        pathfinding.UpdateGrid(grid);
        movementDirection = Vector2.zero;
        root.SetData("targetPosition", (Vector2)buildingTarget.position);
        root.SetData("movementDirection", movementDirection);
        root.SetData("position", (Vector2)transform.position);
        root.SetData("withinAttackRange", false);
        root.SetData("cellSize", grid.GetCellSize());
        root.SetData("aStar", pathfinding);
        root.SetData("index", new Vector2Int());
        root.SetData("checkCollision", true);
        root.SetData("newPath", false);
        root.SetData("reset", false);
        root.SetData("grid", grid);
        root.SetData("boss", true);
        root.SetData("health", stats.Health);
        root.SetData("oldHealth", stats.Health);
        root.SetData("animator", anim);
        root.SetData("hurt", true);
        root.SetData("attackDelay", 1.20f);
        root.SetData("dead", false);
        root.SetData("moving", false);
        root.SetData("attacking", false);
        root.SetData("idle", false);

    }

    private void Update()
    {




        anim.SetBool("attacking", (bool)root.GetData("attacking"));
        anim.SetBool("moving", (bool)root.GetData("moving"));
        anim.SetBool("dead", (bool)root.GetData("dead"));
        AvoidNearbyEnemies();

        float pd = Vector2.Distance(transform.position, player.position);
        if (pd < stats.ChasePlayerRange)
        {


            root.SetData("targetPosition", (Vector2)player.position);
        }
        else
        {


            root.SetData("targetPosition", (Vector2)buildingTarget.position);
        }


        root.SetData("position", (Vector2)transform.position);



        movementDirection = (Vector2)root.GetData("movementDirection");
        if ((Vector2)root.GetData("targetPosition") == (Vector2)buildingTarget.transform.position)
        {
            root.SetData("withinAttackRange",
         (Vector2.Distance((Vector2)root.GetData("targetPosition"),
         (Vector2)transform.position) < stats.AttackBuildingRange));
        }
        else
        {
            root.SetData("withinAttackRange",
         (Vector2.Distance((Vector2)root.GetData("targetPosition"),
         (Vector2)transform.position) < stats.AttackPlayerRange));
        }
        if (!(bool)root.GetData("withinAttackRange"))
        {
            if (velocity != rb.velocity)
            {
                if (velocity == oldvelocity)
                {
                    idleCount++;
                    if (idleCount >= idleMaxCount)
                    {
                        idleCount = 0;
                        root.SetData("idle", true);
                    }

                }

                oldvelocity = velocity;
                velocity = rb.velocity;
            }

            if ((bool)root.GetData("idle"))
            {
                time += Time.deltaTime;
                if (time >= idleDelay)
                {
                    time = 0;
                    root.SetData("idle", false);
                }

            }
        }

        if (movementDirection.x < 0)
            spriteRend.flipX = true;
        else
            spriteRend.flipX = false;

        root.Evaluate();
    }

    public override void Init(AiGrid grid, Transform player, Transform building,
        int flockamount = 0, int flockID = 0, bool flockLeader = false, Transform hidePoint = null, Transform movementRangePoint = null)
    {
        pathfinding = new AStar2D(grid);
        this.player = player;
        this.buildingTarget = building;
        stats = new EnemyStats(Enemytype);
        root = new Root(new List<Node> { new Chase(), new Attack(), new TakeDamage(), new Idle() });
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats.AttackPlayerRange = GetComponent<BoxCollider2D>().size.x;
        stats.AttackBuildingRange = Utility.GRID_CELL_SIZE_LARGE * 1.5f;
        box = GetComponent<BoxCollider2D>();
        spriteRend = GetComponent<SpriteRenderer>();
    }
}
