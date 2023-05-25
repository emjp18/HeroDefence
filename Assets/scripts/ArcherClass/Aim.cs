using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    public Camera cam;
    public Transform tr;

    Vector3 mousePos;


    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition); 
    }

    private void FixedUpdate()
    {
        Vector2 lookDir = mousePos - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
