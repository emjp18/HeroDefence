using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;
using static WaveManager;

public class WaveManager : MonoBehaviour
{
    /// <summary>
    /// Creates the rounds and the waves within each round to make a more natural effect.
    /// </summary>
    public enum SpawnState {Day,Night,spawning,Waiting }
    public enum ROUNDSTATE { START,PAUSE}
    private enum ROUNDS {R1,R2,R3,R4,R5 }
    public EnemySpawner spawnerScript;
    [SerializeField]private EnemyAttack enemyAttStat;
    public Day_Night_Cycle dayNitCycle;
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
    [SerializeField] StartWave startScript;
    [SerializeField] Boss bossStats;
    [SerializeField] enemyHp hpStats;
   
    
 
    [System.Serializable]
    
    public class Wave
    {
        
        public string name;
        public int amountOfEnemies;

    }

    public Wave[] waves;
    public int nextWave=0;
    public SpawnState spawnState = SpawnState.Night;
    public ROUNDSTATE rState = ROUNDSTATE.START;
    public float timeBetweenWaves = 5f;
    public float waveCountDown;
    public float searchCountdown=1f;
    public bool waveComplete = false;
    [SerializeField]public int roundCount;
    [SerializeField]public int currentRoundCount;
    int rand = 0;
    int enemieNr=0;
    Vector3 offset;
   
    public GameObject Player;

    public void Start()
    {
        waveCountDown = timeBetweenWaves;
        roundCount = 1;
        currentRoundCount = 1;

    }
    public void Update()
    {
        // changes stats of enemies depening on round.
        if (currentRoundCount == 1)
        {
            hpStats.hpChange = true;
            hpStats.maxHealth = 100;
       
        }
        if (currentRoundCount == 2)
        {
            hpStats.hpChange = true;
            enemyAttStat.enemyAttackDamage = 5;
            hpStats.maxHealth = 100;
        }
        if (currentRoundCount == 3)
        {
            hpStats.hpChange = true;
            enemyAttStat.enemyAttackDamage = 10;
            hpStats.maxHealth = 150;
        }
        if (currentRoundCount == 4)
        {
            hpStats.hpChange = true;

            hpStats.maxHealth = 200;
        }
        if (currentRoundCount == 5)
        {
            hpStats.hpChange = true;
            enemyAttStat.enemyAttackDamage = 15;
            hpStats.maxHealth = 250;
        }
        if (!dayNitCycle.daytime && spawnState == SpawnState.Night)
        {
                if(rState==ROUNDSTATE.START)
                {
                rState= ROUNDSTATE.PAUSE;
                StartCoroutine(Rounds());
                spawnState = SpawnState.Waiting;
                }
        }

        if (spawnState == SpawnState.Waiting)
        {
            // Checks if enemies are alive, if not, resets everything to be able to start next round.
            if (!EnemyIsAlive())
            {
                dayNitCycle.daytime= true;
                dayNitCycle.changeTime = false;
                roundCount++;
                if(currentRoundCount <= 4)
                {
                startScript.states = StartWave.STARTSTATES.START;
                }
                rState = ROUNDSTATE.START;
                spawnState = SpawnState.Night;
                startScript.counter = 0;
            }
            else
            {
                return;
            }
           
        }
        Debug.Log(nextWave + " Next wave");

    }
    IEnumerator Spawner(Wave wave)
    {
        // creates all the enemies.
        bossPerSpawn = wave.amountOfEnemies;

        foreach (EnemyBase enemy in enemyPrefabTemplates)
        {
            for (int i = 0; i < bossPerSpawn * spawnPoints.Count; i++)
            {
                enemiesBoss.Add(Instantiate(enemy));
                enemiesBoss[enemiesBoss.Count - 1].Init(bossGrid, player, buildingTarget,
                    Instantiate(hitObject));
                enemiesBoss[enemiesBoss.Count - 1].gameObject.SetActive(false);                
            }
            hitObject.SetActive(false);
        }
        StartNightPhase();
        yield break;
    }
    IEnumerator Rounds()
    {
        // Creates the waves within a round.
        StartCoroutine(Spawner(waves[nextWave]));
        completeWave();
        yield return new WaitForSeconds(5);
        StartCoroutine(Spawner(waves[nextWave]));
        completeWave();
        yield return new WaitForSeconds(5);
        StartCoroutine(Spawner(waves[nextWave]));
        completeWave();
        yield return new WaitForSeconds(5);

    }
    public bool EnemyIsAlive()
    {
        // Searches for enemies
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
    public void completeWave()
    {
        if (nextWave + 1 > waves.Length - 1)
        {
            Debug.Log("Compeleted all waves");
            nextWave = 0;
            waveComplete= true;

        }
        else
        {
            nextWave++;
            waveCountDown = 5f;
        }
       
    }
    public void StartNightPhase()
    {

        bossPerSpawn = waves[nextWave].amountOfEnemies;
        
        grid.RegenerateGrid();
        bossGrid.RegenerateGrid();
        Utility.UpdateStaticCollision(grid);
        Utility.UpdateStaticCollisionLarge(bossGrid);
        int sp = spawnPoints.Count;

        // Activates all enemies
        for (int i = 0; i < (bossPerSpawn*sp); i++)
        {
            if (enemiesBoss[enemieNr].gameObject.activeSelf == true) continue;

                rand = Random.Range(-5, 5);
                offset = new Vector3(rand, rand, rand);
                enemiesBoss[ enemieNr ].transform.position = spawnPoints[Random.Range(0, spawnPoints.Count())].position+ offset;
                enemiesBoss[ enemieNr ].gameObject.SetActive(true);
                enemiesBoss[ enemieNr ].StartNightPhase(bossGrid);
                
                enemieNr++;

        }
    }
}
