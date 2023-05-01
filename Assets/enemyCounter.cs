using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class enemyCounter : MonoBehaviour
{
    [SerializeField] WaveManager enemies;
    //public TextMeshPro text;
    //GameObject[] enemies;
    private void Start()
    {
       
    }
    public void Update()
    {
        //enemies = GameObject.FindGameObjectsWithTag("EnemyWithPhysics");
        gameObject.GetComponent<TextMeshPro>().text = enemies.amountOfEnemies.ToString() + " hejsan";
        //text.text = "Enemies ;" + enemies.Length.ToString();
        Debug.Log("TestarCounter");
    }
}
