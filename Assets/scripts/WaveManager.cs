using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;
using static WaveManager;

public class WaveManager : MonoBehaviour
{
    
    public enum SpawnState {Day,Night,ConstantSpawn,Waiting }
    public EnemySpawner spawnerScript;
    public Day_Night_Cycle dayNitCycle;
    [System.Serializable]
    
    public class Wave
    {
        
        public string name;
        public int amountOfEnemies;
    }

    //public List<Wave> waves = new List<Wave>();
    public Wave[] waves;
    public int nextWave=0;
    private int amountOfEnemies;
    public SpawnState spawnState = SpawnState.Night;
    public float timeBetweenWaves = 5f;
    public float searchCountdown=1f;
    public bool waveComplete = false;
    
    public GameObject Player;

    public void Start()
    {
        enemyCounter();


    }
    public void Update()
    {
 

        if (!dayNitCycle.daytime && spawnState == SpawnState.Night)
        {
            //Debug.Log(spawnerScript.enemiesBoss.Count + "Count");
            StartCoroutine(SpawningTimer(waves[nextWave]));
            spawnState = SpawnState.Day;
            //dayNitCycle.daytime = true;

        }
        if (dayNitCycle.daytime && spawnState == SpawnState.Day)
        {
            spawnerScript.ClearAll();
           
            spawnState = SpawnState.Waiting;
        }
        if (spawnState == SpawnState.Waiting)
        {
            completeWave();
            if (!EnemyIsAlive())
            {
                waveComplete = true;
                Debug.Log("HEJHOPP");

                spawnState = SpawnState.Night;
            }
            else
            {
                //Debug.Log("Satan");
                return;
            }
           
        }

        //Debug.Log(nextWave + " Next wave");

    }
    IEnumerator SpawningTimer(Wave wave)
    {
        SpawnMobs();

        yield break;
    }
    public bool EnemyIsAlive()
    {
        searchCountdown-=Time.deltaTime;
        if(searchCountdown<= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }
    public void SpawnMobs()
    {
        spawnerScript.StartNightPhase();
        spawnerScript.startNight = false;
    }
    public void enemyCounter()
    {
        //foreach (var wave in waves)
        //{
            amountOfEnemies = waves[nextWave].amountOfEnemies;
            spawnerScript.NumberOfEnemies(amountOfEnemies);

        //}
    }
    public void completeWave()
    {
        if (nextWave + 1 > waves.Length - 1)
        {
            Debug.Log("Compeleted all waves");
            nextWave = 0;


        }
        else
        {
        Debug.Log("TESTING");
            nextWave++;
            enemyCounter();

        }
       
    }

}
