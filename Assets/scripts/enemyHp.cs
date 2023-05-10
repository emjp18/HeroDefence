using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHp : MonoBehaviour
{
    public int maxHealth = 100;
    public Boss bossScript;
    public float timer;
    int currentHealth;
    public HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(currentHealth);
    }

    private void Update()
    {
        healthBar.SetHealth(currentHealth);
        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    TakeDamage(20);
        //    Debug.Log("TestarHp");

        //}
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            //bossScript.PlayDead();           
            Die();
        }
    }
    void Die()
    {
        gameObject.SetActive(false);
        healthBar.gameObject.SetActive(false);
        //PlayerMovement.Instance.goldAmount += 20;
    }
}
