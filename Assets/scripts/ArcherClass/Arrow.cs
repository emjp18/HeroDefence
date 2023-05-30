using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 20f;
    public int damage;
    public Rigidbody2D rb;

    void Start()
    {
        rb.velocity = transform.right * speed;
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