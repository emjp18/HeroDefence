using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disappearOnHit : MonoBehaviour
{
    Vector2 velocity = Vector2.zero;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);
        velocity = Vector2.zero;
    }
    private void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = (velocity)*Time.fixedDeltaTime;
    }
    public Vector2 Vel
    {
        set { velocity = value; }
    }
}
