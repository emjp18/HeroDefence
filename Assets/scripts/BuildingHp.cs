using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHp : MonoBehaviour

{
    public HealthBar healthbar;
    public int maxHealth = 1000;
    public int currentHealth;
    public float dmgTakenCD;
    [SerializeField] private List<GameObject> fireList=new List<GameObject>();
    void Start()
    {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(currentHealth);
    }
    void Update()
    {
        if (currentHealth<700)
        {
            fireList[0].gameObject.SetActive(true);
        }
        if (currentHealth < 500)
        {
            fireList[1].gameObject.SetActive(true);
        }
        if (currentHealth < 300)
        {
            fireList[2].gameObject.SetActive(true);
        }
        healthbar.SetHealth(currentHealth);
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
