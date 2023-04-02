using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class Boss : EnemyBase
{
    Vector2 position = Vector2.zero;
    Vector2 oldPosition = Vector2.one;
    float jitteringTestTime = 0;
    float jitteringTestTimeDelay = 5;
    float time = 0;
    float reachabletime = 0;
    float reachableDelay = 6;
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
        root.SetData("boxSize", box.size.x);
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
        root.SetData("dead", false);
        root.SetData("moving", false);
        root.SetData("attacking", false);
        root.SetData("idle", false);
        root.SetData("type", Enemytype);
        root.SetData("reachable", true);
        position = oldPosition = (Vector2)transform.position;

    }

    private void Update()
    {




        anim.SetBool("attacking", (bool)root.GetData("attacking"));
        anim.SetBool("moving", (bool)root.GetData("moving"));
        anim.SetBool("dead", (bool)root.GetData("dead"));
        AvoidNearbyEnemies();

        float pd = Vector2.Distance(transform.position, player.position);
        if (pd < stats.ChasePlayerRange && (bool)root.GetData("reachable"))
        {


            root.SetData("targetPosition", (Vector2)player.position);
        }
        else
        {


            root.SetData("targetPosition", (Vector2)buildingTarget.position);
        }
        if (!(bool)root.GetData("reachable"))
        {
            reachabletime += Time.deltaTime;
            if (reachabletime >= reachableDelay)
            {
                reachabletime = 0;
                root.SetData("reachable", true);
            }
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
        if (!(bool)root.GetData("withinAttackRange") && !(bool)root.GetData("idle"))
        {
            jitteringTestTime += Time.deltaTime;
            if (jitteringTestTime >= jitteringTestTimeDelay)
            {
                jitteringTestTime = 0;
                if (Vector2.Distance((Vector2)transform.position, oldPosition) <= Utility.GRID_CELL_SIZE_LARGE)
                {
                    root.SetData("idle", true);
                }
                oldPosition = (Vector2)transform.position;
            }

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
        stats.AttackPlayerRange = GetComponent<BoxCollider2D>().size.x * 0.75f;
        stats.AttackBuildingRange = Utility.GRID_CELL_SIZE_LARGE * 1.5f;
        box = GetComponent<BoxCollider2D>();
        spriteRend = GetComponent<SpriteRenderer>();

    }
}
