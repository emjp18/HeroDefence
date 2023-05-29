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
    public int xpPerKill = 10;
    public GameObject player;


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
        player.GetComponent<LevelSystem>().RecieveXp(xpPerKill);
        healthBar.gameObject.SetActive(false);
   
        //PlayerMovement.Instance.goldAmount += 20;
    }
}
