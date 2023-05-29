using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHp : MonoBehaviour

{
    public int maxHealth = 1000;
    public int currentHealth;
    public float dmgTakenCD;
    void Start()
    {
        currentHealth = maxHealth;
    }
    void Update()
    {
        if (dmgTakenCD > 0)
        {
            dmgTakenCD -= Time.deltaTime;
        }
    }

    public void TakeDamage(int damage)
    {

        if (dmgTakenCD <= 0)
        {
            dmgTakenCD = 0.5f;
            currentHealth -= damage;
           
        }
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
