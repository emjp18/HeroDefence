using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class SelectScript : MonoBehaviour
{

    [SerializeField]Collider2D hitBox;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject gameObject;
    [SerializeField] GameObject gameObject2;
    Vector3 mousePos;
    private void Start()
    {
  
    }
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (hitBox.OverlapPoint(mousePos))
        {
            Debug.Log("TestarMousePos");
            text.gameObject.SetActive(true);
            gameObject.SetActive(true);
            gameObject2.SetActive(true);
        }
        else
        {
            text.gameObject.SetActive(false);
            gameObject.SetActive(false);
            gameObject2.SetActive(false);
        }
        //Debug.Log("mouseposition : "+mousePos);
    }
}
