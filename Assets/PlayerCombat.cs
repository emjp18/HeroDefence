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
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
        void Attack()
        {
            Collider2D[] hitEnemies;
            if (Input.GetKey(KeyCode.A)){
                animator.SetTrigger("attackLeft");
                hitEnemies = Physics2D.OverlapCircleAll(attackPointLeft.position, attackRange, enemyLayers);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                animator.SetTrigger("attackRight");
                hitEnemies = Physics2D.OverlapCircleAll(attackPointRight.position, attackRange, enemyLayers);

            }
            else if (Input.GetKey(KeyCode.W))
            {
                animator.SetTrigger("attackUp");
                hitEnemies = Physics2D.OverlapCircleAll(attackPointUp.position, attackRange, enemyLayers);
            }
            else
            {
                animator.SetTrigger("attack");
                hitEnemies = Physics2D.OverlapCircleAll(attackPointDown.position, attackRange, enemyLayers);
            }
            
            foreach(Collider2D enemy in hitEnemies)
            {
                Debug.Log("enemy hit");
            }
        }
        
    }
   
}
