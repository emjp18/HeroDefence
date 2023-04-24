using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackPointDown;
    public Transform attackPointUp;
    public Transform attackPointRight;
    public Transform attackPointLeft;
    public GameObject player;

    public float attackRange = 0.5f;
    public float knockbackRange = 10f;
    public LayerMask enemyLayers;
    public int attackDamage = 40;
    public float attackRate = 1f;
    public float knockbackTime = 10f;
    float nextAttackTime = 0f;
    float knockbackCD = 0f;
    void Update()
    {
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
                nextAttackTime = Time.time + attackRate;
            }
        }
        if(Time.time >= knockbackCD)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Knockback();
                knockbackCD = Time.time + knockbackTime;
            }

        }
    
        void Attack()
        {
            
            if (Input.GetKey(KeyCode.A)){
                animator.SetTrigger("attackLeft");
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPointLeft.position, attackRange, enemyLayers);
                foreach (Collider2D enemy in hitEnemies)
                {
                    enemy.GetComponent<enemyHp>().TakeDamage(attackDamage);
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                animator.SetTrigger("attackRight");
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPointRight.position, attackRange, enemyLayers);
                foreach (Collider2D enemy in hitEnemies)
                {
                    enemy.GetComponent<enemyHp>().TakeDamage(attackDamage);
                }
            }
            else if (Input.GetKey(KeyCode.W))
            {
                animator.SetTrigger("attackUp");
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPointUp.position, attackRange, enemyLayers);
                foreach (Collider2D enemy in hitEnemies)
                {
                    enemy.GetComponent<enemyHp>().TakeDamage(attackDamage);
                }
            }
            else
            {
                animator.SetTrigger("attack");
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPointDown.position, attackRange, enemyLayers);
                foreach (Collider2D enemy in hitEnemies)
                {
                    enemy.GetComponent<enemyHp>().TakeDamage(attackDamage);
                }
            }
            

        }
        void Knockback()
        {
            animator.SetTrigger("attack");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPointDown.position, knockbackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                
                enemy.GetComponent<KnockbackFeedBack>().PlayFeedBack(player);
            }

        }
        
    }
   
}
