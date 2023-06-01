using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour, IShopCustomer
{
    [SerializeField] float moveSpeed = 200f;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animatorWarrior;
    [SerializeField] Animator animatorRanger;
    [SerializeField] private TrailRenderer tr;
    Vector2 movement;
    public int currentHealth;
    public int maxHealth = 100;
    [SerializeField] public int alive = 0;
    public int goldAmount;
    private int healthPotionAmount;
    public HealthBar healthBar;
    public static PlayerMovement Instance { get; private set; }
    public event EventHandler OnGoldAmountChanged;
    public event EventHandler OnHealthPotionAmountChanged;
    [SerializeField] public bool canDash = true;
    private bool isDashing;
    public hitIndicator hitIndi;
    public float dmgTakenCD;
    private float dashingPower = 24 * 2f;
    private float dashingTime = 0.2f;
    public float dashingCooldown = 1f;
    private Vector3 mousePos;
    private float shieldActiveTime = 5f;
    private bool shieldActive = false;
    public int plusHealth = 20;
    public float plusSpeed = 20;
    private int musicCounter1;
    private int musicCounter2;
    private int musicCounter3;
    private int musicCounter4;
    public GameObject buttonCanvas;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "EnemyAttack")
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
    private void AddsuperSpeed()
    {
        moveSpeed = moveSpeed*2;
    }
    private void Addspeed()
    {
        moveSpeed += 20;
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
            Debug.Log("du e fattig");
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
            case Item.ItemType.Boots: Addspeed(); break;
            case Item.ItemType.MagicMushroom: AddsuperSpeed(); break;

        }
    }

    void Update()
    {
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0 && alive == 0)
        {
            animatorWarrior.SetBool("Dead", true);
            animatorRanger.SetBool("Die", true);
    
            Debug.Log("ALIVE");
            alive++;
            //alive = false;
        }
        if (alive >= 1)
        {
            movement.x = 0;
            movement.y = 0;
            FindObjectOfType<AudioManager>().StopPlaying("Steps");

        }
        if (alive <= 0)
        {


            if (dmgTakenCD > 0)
            {
                dmgTakenCD -= Time.deltaTime;
            }
            if (Input.GetKeyUp(KeyCode.H))
            {
                TryConsumeHealthPotion();
            }
            if (currentHealth <= 0 && alive == 0)
            {
                animatorWarrior.SetBool("Dead", true);
           

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
            animatorRanger.SetFloat("Horizontal", movement.x);
            animatorRanger.SetFloat("Vertical", movement.y);
            animatorRanger.SetFloat("Speed", movement.sqrMagnitude);
            if (Input.GetKeyDown(KeyCode.D))
            {
                FindObjectOfType<AudioManager>().Play("Steps");
                musicCounter1 = 1;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                FindObjectOfType<AudioManager>().Play("Steps");
                musicCounter2 = 1;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                FindObjectOfType<AudioManager>().Play("Steps");
                musicCounter3 = 1;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                FindObjectOfType<AudioManager>().Play("Steps");
                musicCounter4 = 1;
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                musicCounter1= 0;              
            }
            if (Input.GetKeyUp(KeyCode.A))
            {          
                musicCounter2 = 0;
            }
            if (Input.GetKeyUp(KeyCode.S))
            {       
                musicCounter4 = 0;
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                musicCounter3= 0;
            }
            if (musicCounter1+musicCounter2+musicCounter3+musicCounter4 ==0)
            {
                FindObjectOfType<AudioManager>().StopPlaying("Steps");
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            {
                Debug.Log("Test");
                StartCoroutine(Dash());
            }
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
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(transform.localScale.y * dashingPower, transform.localScale.y * dashingPower);
            FindObjectOfType<AudioManager>().Play("Swosh");
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(transform.localScale.y * -dashingPower, transform.localScale.y * dashingPower);
            FindObjectOfType<AudioManager>().Play("Swosh");
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(transform.localScale.y * dashingPower, transform.localScale.y * -dashingPower);
            FindObjectOfType<AudioManager>().Play("Swosh");
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(transform.localScale.y * -dashingPower, transform.localScale.y * -dashingPower);
            FindObjectOfType<AudioManager>().Play("Swosh");
        }
        else if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = new Vector2(0f, transform.localScale.y * dashingPower);
            FindObjectOfType<AudioManager>().Play("Swosh");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.velocity = new Vector2(0f, transform.localScale.y * -dashingPower);
            FindObjectOfType<AudioManager>().Play("Swosh");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(transform.localScale.x * -dashingPower, 0f);
            FindObjectOfType<AudioManager>().Play("Swosh");
        }
        else
        {
            rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
            FindObjectOfType<AudioManager>().Play("Swosh");
        }
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    public void TakeDamage(int damage)
    {

        if (dmgTakenCD <= 0)
        {
            FindObjectOfType<AudioManager>().Play("PlayerHit");
            dmgTakenCD = 0.5f;
            //currentHealth -= damage;
            hitIndi.playerHit = true;
            hitIndi.ppv.weight = 1;
            if (shieldActive == true)
            {
                currentHealth -= damage / 2;
            }
            if (shieldActive == false)
            {
                currentHealth -= damage;
            }
        }
        if (currentHealth <= 0)
        {
            Die();
        }

    }
    void Die()
    {
        //gameObject.SetActive(false);
    }
    public IEnumerator ShieldAbility()
    {
        shieldActive = true;
        yield return new WaitForSeconds(shieldActiveTime);
        shieldActive = false;
    }
    public void IncreaseHealth()
    {
        currentHealth += plusHealth;
        maxHealth += plusHealth;
        buttonCanvas.SetActive(false);
    }
    public void IncreaseSpeed()
    {
        
        moveSpeed += plusSpeed;
        buttonCanvas.SetActive(false);
    }
}
