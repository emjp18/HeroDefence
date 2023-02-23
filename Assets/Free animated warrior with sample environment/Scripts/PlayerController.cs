using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    [SerializeField] AnimationSwitch animSwitch;

    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float jumpDuration = 0.5f;
    public float jumpAnimationDuration = 0.7f;

    public bool jumpAnimation = false;
    public bool isJumping = false;
    public float jumpTimer = 0f;
    public float jumpAnimTimer = 0f;


    public bool isAttacking = false;
    public float attackTimer = 0f;
    public float attackDuration = 0.7f;

    public float DeathTimer = 0f;
    public float DeathTime = 15f;
    public bool isDead = false;

    bool isFacingRight = true;
    float horizontalInput;
    float verticalInput;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }
    private void FixedUpdate()
    {
       
        
        if (Input.GetMouseButtonDown(0) && !isDead && !jumpAnimation)
        {
            isAttacking = true;
            animSwitch.AnimateAttack();
            attackTimer = 0;
        }
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > attackDuration) isAttacking = false;
        }

        if (horizontalInput > float.Epsilon|| horizontalInput < -float.Epsilon || verticalInput > float.Epsilon || verticalInput < -float.Epsilon)
        {
            Vector2 movement = new Vector2(horizontalInput, verticalInput);
            rb.velocity = movement * moveSpeed;
     
            if (!jumpAnimation && !isAttacking && !isDead) animSwitch.AnimateRun();

            if(isFacingRight&& horizontalInput < 0||!isFacingRight && horizontalInput > 0)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1;
                transform.localScale= localScale;
            }

        }
        else
        {
            if (!jumpAnimation && !isAttacking && !isDead) animSwitch.AnimateIDLE();
        }

        
    }

}

/*CODE DOCUMENTATION
 * This code is from a free asset, Emil has made some modifcations so that the controller always runs instead of wals, and can't jump.
 * More controls might be added for attacking or this will be in another script.
 Changed from update to fixedupdate since rb is used.
Added code to flip the character. Added vertical input*/
