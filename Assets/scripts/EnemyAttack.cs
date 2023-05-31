using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject player;
    public GameObject player2;
    public GameObject building;
    public float detectionRange = 15f;
    public Animator animator;

    public float enemyAttackTime = 3.5f;
    [SerializeField]float enemyAttackCD = 0f;

    public float enemyAttackRange = 10f;
    public LayerMask playerLayer;
    public int enemyAttackDamage = 5;

    // Update is called once per frame
    void Update()
    {
        if(player.gameObject.activeSelf==true)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            float distanceToBuilding = Vector3.Distance(transform.position, building.transform.position);
            if (distanceToPlayer <= detectionRange)
            {
                if (Time.time >= enemyAttackCD)
                {
                    enemyAttack();
                    Debug.Log("Player is within range of this object!");
                    enemyAttackCD = Time.time + enemyAttackTime;
                }
            }
            if (distanceToBuilding <= detectionRange)
            {
                if (Time.time >= enemyAttackCD)
                {
                    enemyAttackBase();
                    Debug.Log("Player is within range of this object!");
                    enemyAttackCD = Time.time + enemyAttackTime;
                }
            }
        }
        if (player2.gameObject.activeSelf == true)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player2.transform.position);
            float distanceToBuilding = Vector3.Distance(transform.position, building.transform.position);
            if (distanceToPlayer <= detectionRange)
            {
                if (Time.time >= enemyAttackCD)
                {
                    enemyAttack();
                    Debug.Log("Player is within range of this object!");
                    enemyAttackCD = Time.time + enemyAttackTime;
                }
            }
            if (distanceToBuilding <= detectionRange)
            {
                if (Time.time >= enemyAttackCD)
                {
                    enemyAttackBase();
                    Debug.Log("Player is within range of this object!");
                    enemyAttackCD = Time.time + enemyAttackTime;
                }
            }
        }


    }

    void enemyAttack()
    {

        animator.SetTrigger("attack");
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(transform.position, enemyAttackRange, playerLayer);
        foreach (Collider2D player in hitPlayer)
        {
            player.GetComponent<PlayerMovement>().TakeDamage(enemyAttackDamage);
        }
    }
    void enemyAttackBase()
    {
        animator.SetTrigger("attack");
        Collider2D[] hitBuilding = Physics2D.OverlapCircleAll(transform.position, enemyAttackRange, playerLayer);
        foreach (Collider2D building in hitBuilding)
        {
            building.GetComponent<BuildingHp>().TakeDamage(enemyAttackDamage);
        }
    }
}
