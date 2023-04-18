using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 200f;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animatorWarrior;
    Vector2 movement;
    float health = 100;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="EnemyAttack")
        {
            health -= 1;
           
        }
    }
    void Update()
    {
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
            //animatorWarrior.SetBool("Attack",true);
        }
        //else
        //{
        //    //animatorWarrior.SetBool("StopAttack",true);
        //    animatorWarrior.SetBool("Attack", false);
        //}
        if (Input.GetMouseButtonUp(0))
        {
            //animatorWarrior.SetBool("FrontAttack", false);
            //animatorWarrior.SetBool("StopAttack",true) ;
        }
    }
    void FixedUpdate()
    {
        /// Movement
        rb.velocity = movement * moveSpeed * Time.fixedDeltaTime;
        //rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
