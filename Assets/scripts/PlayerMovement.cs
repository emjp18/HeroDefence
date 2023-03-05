using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animatorWarrior;
    Vector2 movement;

    void Update()
    {
        /// player input
        Input.GetAxisRaw("Horizontal");
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        animatorWarrior.SetFloat("Horizontal",movement.x);
        animatorWarrior.SetFloat("Vertical", movement.y);
        animatorWarrior.SetFloat("Speed", movement.sqrMagnitude);
    }
    void FixedUpdate()
    {
        /// Movement
   
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
