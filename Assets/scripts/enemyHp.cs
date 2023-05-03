using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHp : MonoBehaviour
{
    public int maxHealth = 100;
    public Boss bossScript;
    public float timer;
    int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
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
    }
}
