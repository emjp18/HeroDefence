using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Arrow : MonoBehaviour
{
    public float speed = 20f;
    public static int damage = 50;
    public Rigidbody2D rb;
    ArcherWeapon dmg;
    public int increaseDMG=50;


    void Start()
    {
        rb.velocity = transform.right * speed;
        
    }

    public void IncreaseAD()
    {
        damage += increaseDMG;
    }
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        enemyHp enemy = hitInfo.GetComponent<enemyHp>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Debug.Log("du har blivit träffad");
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject, 2f);
        }
        Debug.Log(hitInfo.GetComponent<Boss>());
        //Destroy(gameObject);
    }
}