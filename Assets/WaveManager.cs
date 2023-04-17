using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        public int waveNr;
    }

    //public List<Wave> waves = new List<Wave>();
    public Wave[] waves;
    private int nextWave=0;
    private int amountOfEnemies;

    private SpawnState spawnState = SpawnState.Night;
     void Start()
     {
        dayNitCycle.daytime = false;
        enemyCounter();

     }
    void Update()
    {

        Debug.Log(spawnerScript.enemiesBoss.Count + "Count");
        if (!dayNitCycle.daytime && spawnState == SpawnState.Night)
        {
            StartCoroutine(SpawningWave(waves[nextWave]));
            spawnState = SpawnState.Day;
        }
        if(dayNitCycle.daytime)
        {
            spawnerScript.ClearAll();
            completeWave();
            spawnState = SpawnState.Waiting;
        }
        if (spawnState == SpawnState.Waiting)
        {
            

            spawnState = SpawnState.Night;
        }


    }
    IEnumerator SpawningWave(Wave wave)
    {
        SpawnMobs();

        yield break;
    }
    void SpawnMobs()
    {
        spawnerScript.StartNightPhase();
        spawnerScript.startNight = false;
    }
    void enemyCounter()
    {

        amountOfEnemies = waves[nextWave].amountOfEnemies;
        spawnerScript.NumberOfEnemies(amountOfEnemies);

    }
    void completeWave()
    {
        Debug.Log("TESTING");
        if (nextWave+1> waves.Length-1)
        {
            Debug.LogWarning("Compeleted all waves");
            nextWave = 0;
            enemyCounter();


        }
        else
        {
            nextWave = 1;
            enemyCounter();

        }
    }

}
