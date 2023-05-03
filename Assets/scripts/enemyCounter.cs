using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class enemyCounter : MonoBehaviour
{
    //[SerializeField] WaveManager enemies;
    public TextMeshProUGUI text;
    int couter;
    GameObject[] enemies;
    private void Start()
    {
       text=GetComponentInChildren<TextMeshProUGUI>();
    }
    public void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //text.text = enemies.waves[enemies.nextWave].amountOfEnemies.ToString() + " hejsan";

        text.text = "Enemies Remaining: "+enemies.Count().ToString();
        //GameObject.FindGameObjectWithTag("Enemy");
        //text.text = "Enemies ;" + enemies.Length.ToString();
        Debug.Log("TestarCounter");
    }
}
