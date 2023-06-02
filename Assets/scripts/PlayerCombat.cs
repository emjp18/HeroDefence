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
    public GameObject particles;
    public GameObject particles2;
    private float buffActiveTime = 5f;


    private void Start()
    {
        knockbackCD = 0;
        knockbackTime = 10f;
        shieldCD = 0f;

      
    }
    void Update()
    {

        nextAttackTime -= Time.deltaTime;//counts dowwn the cooldown
        knockbackCD-= Time.deltaTime;
        shieldCD-= Time.deltaTime;
        if (nextAttackTime<=0)//checks for cooldown
        {
            if (Input.GetKeyDown(KeyCode.Space))//checks for key pressed
            {
                Attack();
                FindObjectOfType<AudioManager>().Play("SwordAtt");
                nextAttackTime =  1;//adds cooldowwn
            }
        }

        if (knockbackCD<=0)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                knockbackCD =10;
                FindObjectOfType<AudioManager>().Play("Sbash");
                Knockback();
            }

        }
        if (shieldCD<=0)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(GetComponent<PlayerMovement>().ShieldAbility());
                StartCoroutine(AttackBuff());
                shieldCD = 10;
                particles.SetActive(true);
                particles2.SetActive(true);
            }

        }
        if(shieldCD<=5) 
        {
            particles.SetActive(false);
            particles2.SetActive(false);
        }


        void Attack()
        {

            if (Input.GetKey(KeyCode.A))//checks  for direction
            {
                animator.SetTrigger("attackLeft");
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPointLeft.position, attackRange, enemyLayers);//checks for enemies  hit on enemylayers withing attackrange
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
                enemy.GetComponent<enemyHp>().TakeDamage(20);
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
