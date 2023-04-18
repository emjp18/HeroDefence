using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 200f;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animatorWarrior;
    [SerializeField] private TrailRenderer tr;
    Vector2 movement;
    int currentHealth;
    public int pMaxHealth = 100;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="EnemyAttack")
        {
            
           
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if(isDashing)
        {
            return;
        }
        /// player input
        Input.GetAxisRaw("Horizontal");
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        animatorWarrior.SetFloat("Horizontal", movement.x);
        animatorWarrior.SetFloat("Vertical", movement.y);
        animatorWarrior.SetFloat("Speed", movement.sqrMagnitude);
        if (Input.GetKeyDown(KeyCode.D))
        {
            FindObjectOfType<AudioManager>().Play("Steps");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            FindObjectOfType<AudioManager>().Play("Steps");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            FindObjectOfType<AudioManager>().Play("Steps");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            FindObjectOfType<AudioManager>().Play("Steps");
        }
        if (Input.GetKeyUp(KeyCode.D)) 
        {
            FindObjectOfType<AudioManager>().StopPlaying("Steps");
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            FindObjectOfType<AudioManager>().StopPlaying("Steps");
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            FindObjectOfType<AudioManager>().StopPlaying("Steps");
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            FindObjectOfType<AudioManager>().StopPlaying("Steps");
        }
        //if (Input.GetKeyUp(KeyCode.A))
        //{
        //    FindObjectOfType<AudioManager>().StopPlaying("Steps");
        //}

        if (Input.GetMouseButtonDown(0))
        {
            animatorWarrior.SetBool("Attack",true);
        }
        //else
        //{
        //    //animatorWarrior.SetBool("StopAttack",true);
        //    animatorWarrior.SetBool("Attack", false);
        //}
        if (Input.GetMouseButtonUp(0))
        {
            animatorWarrior.SetBool("FrontAttack", false);
            animatorWarrior.SetBool("StopAttack",true) ;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }
    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
            
        
        
        /// Movement
        rb.velocity = movement * moveSpeed * Time.fixedDeltaTime;
        //rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = new Vector2(0f, transform.localScale.y * dashingPower);
        }
        else if (Input.GetKey(KeyCode.S)){
            rb.velocity = new Vector2(0f, transform.localScale.y * -dashingPower);
        }
        else if (Input.GetKey(KeyCode.A)){
            rb.velocity = new Vector2(transform.localScale.x * -dashingPower, 0f);
        }
        else {
            rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        }
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        isDashing= false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash=true;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }

    }
    void Die()
    {
        gameObject.SetActive(false);
    }
}
