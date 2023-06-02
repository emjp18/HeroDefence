using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectScript : MonoBehaviour
{
    /// <summary>
    /// Activates text/icon/panel for abilitys in character menu when hovering over them with mouse.
    /// </summary>
    [SerializeField]Collider2D hitBox;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject gameObject1;
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
            gameObject1.SetActive(true);
            gameObject2.SetActive(true);
        }
        else
        {
            text.gameObject.SetActive(false);
            gameObject1.SetActive(false);
            gameObject2.SetActive(false);
        }
        //Debug.Log("mouseposition : "+mousePos);
    }
}
