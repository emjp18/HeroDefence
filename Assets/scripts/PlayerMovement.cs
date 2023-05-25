using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IShopCustomer
{
    [SerializeField] float moveSpeed = 200f;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animatorWarrior;
    [SerializeField] private TrailRenderer tr;
    Vector2 movement;
    public int currentHealth;
    public int maxHealth = 100;
    public int alive= 0;
    public int goldAmount;
    private int healthPotionAmount;
    private int mushroomAmount;

    public event EventHandler OnGoldAmountChanged;
    public event EventHandler OnHealthPotionAmountChanged;


    public HealthBar healthBar;

    public static PlayerMovement Instance { get; private set; }

    private bool haveBoots = false;
    private bool canDash = true;
    private bool isDashing;
    public hitIndicator hitIndi;
    private float dashingPower = 24*2f;
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
        healthBar.SetMaxHealth(currentHealth);
    }
    private void Awake()
    {
        Instance = this;
        goldAmount = 100;
        healthPotionAmount = 1;

    }
    public int GetGoldAmount()
    {
        return goldAmount;
    }
    public int GetHealthPotionAmount()
    {
        return healthPotionAmount;
    }
    private void AddHealthPotion()
    {
        healthPotionAmount++;
        OnHealthPotionAmountChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool TrySpendGoldAmount(int spendGoldAmount)
    {
        if (GetGoldAmount() >= spendGoldAmount)
        {
            goldAmount -= spendGoldAmount;
            OnGoldAmountChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }
        else
        {
            Debug.Log("du �r fattig");
            return false;
        }
    }



    public void TryConsumeHealthPotion()
    {
        if (healthPotionAmount > 0)
        {
            healthPotionAmount--;
            OnHealthPotionAmountChanged?.Invoke(this, EventArgs.Empty);
            currentHealth += 20;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }


    public void BoughtItem(Item.ItemType itemType)
    {
        Debug.Log("Bought item: " + itemType);
        switch (itemType)
        {
            case Item.ItemType.HealthPotion: AddHealthPotion(); break;
            case Item.ItemType.Boots: haveBoots = true; break;
            case Item.ItemType.MagicMushroom: mushroomAmount = 1; break;

        }
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.H) && currentHealth<maxHealth )
        {
            TryConsumeHealthPotion();
        }
        if(haveBoots)
        {
            moveSpeed = 300f;
        }
        if(mushroomAmount == 1)
        {
            moveSpeed = 1000f;
            Debug.Log("du har en svamp :D");
            mushroomAmount = 0;
        }
        if (currentHealth<=0 && alive==0)
        {
            animatorWarrior.SetBool("Dead",true);
            
            Debug.Log("ALIVE");
            alive++;
            
        //alive = false;
        }


        if (isDashing)
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
        if (Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage(1);
            Debug.Log("TestarHp");
            
        }
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
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        healthBar.SetHealth(currentHealth);
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
        hitIndi.playerHit = true;
        hitIndi.ppv.weight = 1;
        if (currentHealth <= 0)
        {
            Die();
        }

    }
    void Die()
    {
        //gameObject.SetActive(false);
    }
}
