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
    [SerializeField] int nightphase = 0;
    public bool startNight = false;
    public bool endNight = false;
    public List<EnemyBase> enemiesBoss = new List<EnemyBase>();
    [SerializeField] List<EnemyBase> enemyPrefabTemplates;
    public int bossPerSpawn = 1;
    [SerializeField] Transform player;
    [SerializeField] Transform buildingTarget;
    [SerializeField] AiGrid grid;
    [SerializeField] AiGrid bossGrid;
    [SerializeField] List<Transform> spawnPoints;
    [SerializeField] GameObject hitObject;
 
    [System.Serializable]
    
    public class Wave
    {
        
        public string name;
        public int amountOfEnemies;

    }

    //public List<Wave> waves = new List<Wave>();
    public Wave[] waves;
    public int nextWave=0;
    public int amountOfEnemies;
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
        Debug.Log(spawnState);
        if (startNight)
        {
            StartNightPhase();
            startNight = false;
        }
        if (endNight)
        {
            EndNightPhase();
            endNight = false;
        }
        if (!dayNitCycle.daytime && spawnState == SpawnState.Night)
        {
            //Debug.Log(spawnerScript.enemiesBoss.Count + "Count");
            spawnState = SpawnState.Waiting;
            StartCoroutine(Spawner(waves[nextWave]));
            StartNightPhase();
            //dayNitCycle.daytime = true;


        }
        //if (dayNitCycle.daytime && spawnState == SpawnState.Day)
        //{
        //    spawnerScript.ClearAll();
           
        //    spawnState = SpawnState.Waiting;
        //}
        if (spawnState == SpawnState.Waiting)
        {
            
            if (!EnemyIsAlive())
            {

                //spawnState = SpawnState.Day;
                dayNitCycle.daytime= true;
                dayNitCycle.changeTime = false;
                completeWave();
                if (nextWave == 4)
                {
                    waveComplete = true;
                }
            }
            else
            {
                //Debug.Log("Satan");
                return;
            }
           
        }

        Debug.Log(nextWave + " Next wave");

    }
    IEnumerator Spawner(Wave wave)
    {
        amountOfEnemies = wave.amountOfEnemies;
        bossPerSpawn = amountOfEnemies;
        //SpawnMobs();
        foreach (EnemyBase enemy in enemyPrefabTemplates)
        {
            for (int i = 0; i < bossPerSpawn * spawnPoints.Count; i++)
            {
                enemiesBoss.Add(Instantiate(enemy));
                enemiesBoss[enemiesBoss.Count - 1].Init(bossGrid, player, buildingTarget,
                    Instantiate(hitObject));
                enemiesBoss[enemiesBoss.Count - 1].gameObject.SetActive(false);                
            }
            //hitObject.SetActive(false);
            //enemy.gameObject.SetActive(false);
        }
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
        foreach (var wave in waves)
        {
            //amountOfEnemies 
            //spawnerScript.NumberOfEnemies(amountOfEnemies);

        }
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
            spawnState = SpawnState.Night;
            //enemyCounter();

        }
       
    }
    public void EndNightPhase()
    {
        foreach (EnemyBase enemy in enemiesBoss)
        {
            enemy.gameObject.SetActive(false);
        }
    }
    public void StartNightPhase()
    {
        int rand = 0;
        rand = Random.Range(0, 15);
        Vector3 randomTest = new Vector3(rand, rand, rand);
        bossPerSpawn = waves[nextWave].amountOfEnemies;
        grid.RegenerateGrid();
        bossGrid.RegenerateGrid();
        Utility.UpdateStaticCollision(grid);
        Utility.UpdateStaticCollisionLarge(bossGrid);
        int sp = spawnPoints.Count;
        for (int i = 0; i < sp*bossPerSpawn; i++)
        {
            //for (int j = 0; j < bossPerSpawn; j++)
            //{
                enemiesBoss[ i ].transform.position = spawnPoints[i].position;
                enemiesBoss[ i ].gameObject.SetActive(true);
                enemiesBoss[ i ].StartNightPhase(bossGrid);
                Debug.Log(i);
            //}

        }
    }

}
