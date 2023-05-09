using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject player;
    public float detectionRange = 15f;
    public Animator animator;

    public float enemyAttackTime = 2.5f;
    float enemyAttackCD = 0f;

    public float enemyAttackRange = 10f;
    public LayerMask playerLayer;
    public int enemyAttackDamage = 50;

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= detectionRange)
        {
            if (Time.time >= enemyAttackCD)
            {
                enemyAttack();
                Debug.Log("Player is within range of this object!");
                enemyAttackCD = Time.time + enemyAttackTime;
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

}
