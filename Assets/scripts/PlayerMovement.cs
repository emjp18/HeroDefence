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
    Vector2 movement;
    public int health = 100; // �ndra till maxhealth?
    public int currentHealth;
    private int goldAmount;
    private int healthPotionAmount;

    public event EventHandler OnGoldAmountChanged;
    public event EventHandler OnHealthPotionAmountChanged;

    public HealthBar healthBar;

    public static PlayerMovement Instance { get; private set; }

    private void Start()
    {
        currentHealth = health;
        healthBar.SetMaxHealth(health);
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
            if(currentHealth> health)
            {
                currentHealth = health;
            }
        }
    }
    public void BoughtItem(Item.ItemType itemType)
    {
        Debug.Log("Bought item: " + itemType);
        switch (itemType)
        {
            case Item.ItemType.HealthPotion: AddHealthPotion(); break;

        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }


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

        healthBar.SetHealth(currentHealth);

        if (Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage(5);
        }
        if(Input.GetKeyUp(KeyCode.H))
        {
            TryConsumeHealthPotion();
        }

    }
    void FixedUpdate()
    {
        /// Movement
        rb.velocity = movement * moveSpeed * Time.fixedDeltaTime;
        //rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
