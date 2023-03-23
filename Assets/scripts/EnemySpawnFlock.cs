using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnFlock : MonoBehaviour
{
    [SerializeField] AiGrid2 grid;
    [SerializeField] List<Transform> spawnpoints; //north +x, south +x, east - y west - y
    [SerializeField] int maxEnemiesPerFlockMax = 25;
    [SerializeField] EnemyBase enemyFlockTemplate;
    List<EnemyBase> enemiesNorth = new List<EnemyBase>();
    List<EnemyBase> enemiesSouth = new List<EnemyBase>();
    List<EnemyBase> enemiesWest= new List<EnemyBase>();
    List<EnemyBase> enemiesEast = new List<EnemyBase>();
    [SerializeField] int enemiesPerFlockPerWave = 10;
    int waveNumber = -1;
    [SerializeField]  bool north = false;
    [SerializeField] bool south = false;
    [SerializeField] bool west = false;
    [SerializeField] bool east = false;
    [SerializeField] float targetPower;
    [SerializeField] float separatePower;
    [SerializeField] float separateObjectPower;
    [SerializeField] float AlignPower;
    [SerializeField] float CohesionPower;

    private void Start()
    {
        enemyFlockTemplate.gameObject.SetActive(false);
        if(north)
        {
            for (int i = 0; i < maxEnemiesPerFlockMax; i++)
            {
                enemiesNorth.Add(Instantiate(enemyFlockTemplate));
                enemiesNorth[i].Init();
                enemiesNorth[i].InitA_STAR(grid);
                enemiesNorth[i].InitFlock(separatePower, separateObjectPower, targetPower, AlignPower, CohesionPower);
                enemiesNorth[i].gameObject.SetActive(false);
            }
        }
        if (south)
        {
            for (int i = 0; i < maxEnemiesPerFlockMax; i++)
            {
                enemiesSouth.Add(Instantiate(enemyFlockTemplate));
                enemiesSouth[i].Init();
                enemiesSouth[i].InitA_STAR(grid);
                enemiesSouth[i].InitFlock(separatePower, separateObjectPower, targetPower, AlignPower, CohesionPower);
                enemiesSouth[i].gameObject.SetActive(false);
            }
        }
        if (west)
        {
            for (int i = 0; i < maxEnemiesPerFlockMax; i++)
            {
                enemiesWest.Add(Instantiate(enemyFlockTemplate));
                enemiesWest[i].Init();
                enemiesWest[i].InitA_STAR(grid);
                enemiesWest[i].InitFlock(separatePower, separateObjectPower, targetPower, AlignPower, CohesionPower);
                enemiesWest[i].gameObject.SetActive(false);
            }
        }
        if (east)
        {
            for (int i = 0; i < maxEnemiesPerFlockMax; i++)
            {
                enemiesEast.Add(Instantiate(enemyFlockTemplate));
                enemiesEast[i].Init();
                enemiesEast[i].InitA_STAR(grid);
                enemiesEast[i].InitFlock(separatePower, separateObjectPower, targetPower, AlignPower, CohesionPower);
                enemiesEast[i].gameObject.SetActive(false);
            }
        }

    }
    public void EndNightPhase()
    {

        if (north)
        {
            for (int i = 0; i < enemiesPerFlockPerWave; i++)
            {
                
                enemiesNorth[i].gameObject.SetActive(false);
            }
        }
        if (south)
        {
            for (int i = 0; i < enemiesPerFlockPerWave; i++)
            {
               
                enemiesSouth[i].gameObject.SetActive(false);
            }
        }
        if (west)
        {
            for (int i = 0; i < enemiesPerFlockPerWave; i++)
            {
               
                enemiesWest[i].gameObject.SetActive(false);
            }
        }
        if (east)
        {
            for (int i = 0; i < enemiesPerFlockPerWave; i++)
            {
             
                enemiesEast[i].gameObject.SetActive(false);
            }
        }
        switch (waveNumber)//Update enemy count information for each type
        {
            case 0:
                {

                    break;
                }
            case 1:
                {

                    break;
                }
            case 2:
                {
                    break;
                }
            case 3:
                {
                    break;
                }
        }

    }
    public void StartNightPhase()
    {
    
        waveNumber++;
        if (north)
        {
            for (int i = 0; i < enemiesPerFlockPerWave; i++)
            {

                enemiesNorth[i].gameObject.SetActive(true);
                enemiesNorth[i].transform.position = (Vector2)spawnpoints[(int)SPAWN_TYPE.NORTH].position+(i*Vector2.right* grid.GetCellSize());
                enemiesNorth[i].StartNightPhase();
            }
        }
        if (south)
        {
            for (int i = 0; i < enemiesPerFlockPerWave; i++)
            {

                enemiesSouth[i].gameObject.SetActive(true);
                enemiesSouth[i].transform.position = (Vector2)spawnpoints[(int)SPAWN_TYPE.SOUTH].position + (i * Vector2.left * grid.GetCellSize());
                enemiesSouth[i].StartNightPhase();
            }
        }
        if (west)
        {
            for (int i = 0; i < enemiesPerFlockPerWave; i++)
            {

                enemiesWest[i].gameObject.SetActive(true);
                enemiesWest[i].transform.position = (Vector2)spawnpoints[(int)SPAWN_TYPE.WEST].position + (i * Vector2.up * grid.GetCellSize());
                enemiesWest[i].StartNightPhase();
            }
        }
        if (east)
        {
            for (int i = 0; i < enemiesPerFlockPerWave; i++)
            {

                enemiesEast[i].gameObject.SetActive(true);
                enemiesEast[i].transform.position = (Vector2)spawnpoints[(int)SPAWN_TYPE.EAST].position + (i * Vector2.down * grid.GetCellSize());
                enemiesEast[i].StartNightPhase();
            }
        }

    }
}
