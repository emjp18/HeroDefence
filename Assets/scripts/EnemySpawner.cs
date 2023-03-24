using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SPAWN_TYPE { NORTH = 0, EAST = 2, WEST = 3, SOUTH = 1 } //make sure the spawn points are in this order
public enum ENEMY_TYPE {  SWARMER }
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] AiGrid grid;
    [SerializeField] List<Transform> hidePoints;
/*    [SerializeField] List<Transform> spawnpoints;*/ //north +x, south +x, east - y west - y
    [SerializeField] int maxSwarmerAmount = 40;
    [SerializeField] List<EnemyBase> enemyPrefabTemplates;
    //List<EnemyBase> enemiesNorth = new List<EnemyBase>();
    //List<EnemyBase> enemiesSouth = new List<EnemyBase>();
    //List<EnemyBase> enemiesWest= new List<EnemyBase>();
    //List<EnemyBase> enemiesEast = new List<EnemyBase>();
    List<EnemyBase> enemiesSwarm = new List<EnemyBase>();
    [SerializeField] int swarmWaveAmountPerHidePoint = 10;
    //int waveNumber = -1;
    //[SerializeField]  bool north = false;
    //[SerializeField] bool south = false;
    //[SerializeField] bool west = false;
    //[SerializeField] bool east = false;
    [SerializeField] float targetPower;
    [SerializeField] float separatePower;
    [SerializeField] float separateObjectPower;
    [SerializeField] float AlignPower;
    [SerializeField] float CohesionPower;
    [SerializeField] float randomDirPower;
    FlockWeights flockWeights = new FlockWeights();
    public bool startNight = true;
    public bool endNight = false;
    private void Start()
    {
        flockWeights.align = AlignPower;
        flockWeights.separateAgents = separatePower;
        flockWeights.cohesive = CohesionPower;
        flockWeights.separateStatic = separateObjectPower;
        flockWeights.moveToTarget = targetPower;
        flockWeights.random = randomDirPower;


        foreach ( EnemyBase enemy in enemyPrefabTemplates)
        {
            switch (enemy.Enemytype)
            {
                case ENEMY_TYPE.SWARMER:
                    {
                        for (int i = 0; i < maxSwarmerAmount; i++)
                        {
                            enemiesSwarm.Add(Instantiate(enemy));
                            enemiesSwarm[i].Init(flockWeights, grid, swarmWaveAmountPerHidePoint);
                            enemiesSwarm[i].gameObject.SetActive(false);

                        }


                        break;
                    }
            }

            enemy.gameObject.SetActive(false);
        }
        



    }
    public void EndNightPhase()
    {

        for (int i = 0; i < swarmWaveAmountPerHidePoint; i++)
        {
            enemiesSwarm[i].gameObject.SetActive(false);
        }

    }
    public void StartNightPhase()
    {
        for(int j=0; j<hidePoints.Count; j++)
        {
            for (int i = 0; i < swarmWaveAmountPerHidePoint; i++)
            {
                
                enemiesSwarm[i + (swarmWaveAmountPerHidePoint * j)].gameObject.SetActive(true);
                enemiesSwarm[i + (swarmWaveAmountPerHidePoint * j)].gameObject.transform.position =
                    (Vector2)hidePoints[j].position;
                enemiesSwarm[i + (swarmWaveAmountPerHidePoint * j)].StartNightPhase(grid);

               

            }
        }
       

    }
    //Temporary

    private void Update()
    {
        if(startNight)
        {
            StartNightPhase();
            startNight = false;
        }
        if (endNight)
        {
            EndNightPhase();
            endNight = false;
        }
    }
}
