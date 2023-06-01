using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public float shieldTime = 10f;
    float nextAttackTime = 0f;
    public float knockbackCD = 0f;
    public float shieldCD = 0f;

    public int plusAD = 10;
    public GameObject buttonCanvas;
    //public GameObject particles;
    private float buffActiveTime = 5f;


    private void Start()
    {
        //particles.SetActive(true);
    }
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
                FindObjectOfType<AudioManager>().Play("SwordAtt");
                nextAttackTime = Time.time + attackRate;
            }
        }
        if (Time.time >= knockbackCD)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                FindObjectOfType<AudioManager>().Play("Sbash");
                Knockback();
                knockbackCD = Time.time + knockbackTime;
            }

        }
        if (Time.time >= shieldCD)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(GetComponent<PlayerMovement>().ShieldAbility());
                StartCoroutine(AttackBuff());
                shieldCD = Time.time + shieldTime;
            }

        }


        void Attack()
        {

            if (Input.GetKey(KeyCode.A))
            {
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
    public void IncreaseAD()
    {
        attackDamage += plusAD;
        buttonCanvas.SetActive(false);
    }
    public IEnumerator AttackBuff()
    {
        attackDamage = attackDamage * 2;
        //particles.SetActive(true);
        yield return new WaitForSeconds(buffActiveTime);
        attackDamage = attackDamage / 2;
        //particles.SetActive(false);
    }
}
