using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHp : MonoBehaviour
{
    public int maxHealth;
    public Boss bossScript;
    [SerializeField]WaveManager waveScript;
    public float timer;
    public int currentHealth;
    public HealthBar healthBar;
    public int xpPerKill = 10;
    public GameObject player;
    public bool hpChange;


    void Start()
    {
        hpChange= true;
        //maxHealth = 100;
       
  
    }

    private void Update()
    {
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
        if (hpChange)
        {
            hpChange = false;
            currentHealth = maxHealth;
        }
   

    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        FindObjectOfType<AudioManager>().Play("Zgrunt");
        if (currentHealth <= 0)
        {
            //bossScript.PlayDead();           
            Die();
        }
    }
    void Die()
    {
        PlayerMovement.Instance.goldAmount += 20;
        healthBar.gameObject.SetActive(false);
        gameObject.SetActive(false);
        player.GetComponent<LevelSystem>().RecieveXp(xpPerKill);


    }
}
