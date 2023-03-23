using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public enum SPAWN_TYPE { NORTH = 0, EAST = 2, WEST = 3, SOUTH = 1 } //make sure the spawn points are in this order
public enum ENEMY_TYPE { MINI_BOSS, CHASE_PLAYER, CHASE_BUILDING, CHASE_BOTH}
public class EnemySpawn : MonoBehaviour
{
    [SerializeField] AiGrid2 grid;
    [SerializeField] List<Transform> spawnpoints; //north +x, south +x, east - y west - y
    [SerializeField] int maxEnemyChasePlayercount = 10;
    [SerializeField] int maxEnemyChaseBuildingcount = 20;
    [SerializeField] int maxEnemyChaseBothcount = 10;
    [SerializeField] EnemyBase enemyChasePlayer;
    [SerializeField] EnemyBase enemyChaseBuilding;
    [SerializeField] EnemyBase enemyChaseBoth;
    int enemyChasePlayercount = 4;
    int enemyChaseBuildingcount = 4;
    int enemyChaseBothcount = 4;
    List<EnemyBase> chasePlayerenemies = new List<EnemyBase>();
    List<EnemyBase> chaseBuildingenemies = new List<EnemyBase>();
    List<EnemyBase> chaseBothenemies = new List<EnemyBase>();
    int waveNumber = 0;
    float enemySize;

    Vector2 offset = new Vector2();
    void UpdateEnemyLists()
    {
       if(enemyChasePlayercount<= maxEnemyChasePlayercount)
        {
            for (int i = 0; i < enemyChasePlayercount; i++)
            {

                chasePlayerenemies[i].gameObject.SetActive(true);
            }
        }
        if (enemyChaseBuildingcount <= maxEnemyChaseBuildingcount)
        {
            for (int i = 0; i < enemyChaseBuildingcount; i++)
            {


                chaseBuildingenemies[i].gameObject.SetActive(true);
            }
        }
            
        if (enemyChaseBothcount <= maxEnemyChaseBothcount)
        {
            for (int i = 0; i < enemyChaseBothcount; i++)
            {


                chaseBothenemies[i].gameObject.SetActive(true);
            }

        }
            
    }
    private void Start()
    {
        enemyChasePlayer.gameObject.SetActive(false);
        enemyChaseBuilding.gameObject.SetActive(false);
        enemyChaseBoth.gameObject.SetActive(false);

        Debug.Log((Vector2.up + Vector2.left) * 0.5f);
    
        for (int i = 0; i < maxEnemyChasePlayercount; i++)
        {
            chasePlayerenemies.Add(Instantiate(enemyChasePlayer));
            chasePlayerenemies[i].InitA_STAR(grid);
            chasePlayerenemies[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < maxEnemyChaseBuildingcount; i++)
        {
            chaseBuildingenemies.Add(Instantiate(enemyChaseBuilding));
            chaseBuildingenemies[i].InitA_STAR(grid);
            chaseBuildingenemies[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < maxEnemyChaseBothcount; i++)
        {
            chaseBothenemies.Add(Instantiate(enemyChaseBoth));
            chaseBothenemies[i].InitA_STAR(grid);
            chaseBothenemies[i].gameObject.SetActive(false);
        }
    }
    public void EndNightPhase()
    {

        for (int i = 0; i < enemyChasePlayercount; i++)
        {
           
            chasePlayerenemies[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < enemyChaseBuildingcount; i++)
        {
       
            chaseBuildingenemies[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < enemyChaseBothcount; i++)
        {
          
            chaseBothenemies[i].gameObject.SetActive(false);
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
        UpdateEnemyLists();
        waveNumber++;
        grid.RegenerateGrid();
        int c = 0;
        int north = 0;
        int south = 0;
        int east = 0;
        int west = 0;
        int count = 0;
        for (int i = 0; i < enemyChasePlayercount; i++)
        {
            if(c>=4)
                c = 0;

            switch((SPAWN_TYPE)c)
            {
                case SPAWN_TYPE.NORTH:
                    {
                        offset = Vector2.right;
                        enemySize = enemyChasePlayer.GetComponent<BoxCollider2D>().size.x;
                        north++;
                        count = north;
                        break;
                    }
                case SPAWN_TYPE.EAST:
                    {
                        enemySize = enemyChasePlayer.GetComponent<BoxCollider2D>().size.y;
                        offset = Vector2.down;
                        east++;
                        count = east;
                        break;
                    }
                case SPAWN_TYPE.WEST:
                    {
                        enemySize = enemyChasePlayer.GetComponent<BoxCollider2D>().size.y;
                        offset = Vector2.down;
                        west++;
                        count = west;
                        break;
                    }
                case SPAWN_TYPE.SOUTH:
                    {
                        enemySize = enemyChasePlayer.GetComponent<BoxCollider2D>().size.x;
                        offset = Vector2.right;
                        south++;
                        count = south;
                        break;
                    }

            }
            chasePlayerenemies[i].transform.position = (Vector2)spawnpoints[c++].position + (offset * enemySize * count);
            chasePlayerenemies[i].GetComponent<EnemyChasePlayerComponent>().StartNightPhaseA_STAR(grid);
            


        }
        for (int i = 0; i < enemyChaseBuildingcount; i++)
        {
            if (c >= 4)
                c = 0;
            switch ((SPAWN_TYPE)c)
            {
                case SPAWN_TYPE.NORTH:
                    {
                        offset = Vector2.right;
                        enemySize = enemyChaseBuilding.GetComponent<BoxCollider2D>().size.x;
                        north++;
                        count = north;
                        break;
                    }
                case SPAWN_TYPE.EAST:
                    {
                        enemySize = enemyChaseBuilding.GetComponent<BoxCollider2D>().size.y;
                        offset = Vector2.down;
                        east++;
                        count = east;
                        break;
                    }
                case SPAWN_TYPE.WEST:
                    {
                        enemySize = enemyChaseBuilding.GetComponent<BoxCollider2D>().size.y;
                        offset = Vector2.down;
                        west++;
                        count = west;
                        break;
                    }
                case SPAWN_TYPE.SOUTH:
                    {
                        enemySize = enemyChaseBuilding.GetComponent<BoxCollider2D>().size.x;
                        offset = Vector2.right;
                        south++;
                        count = south;
                        break;
                    }
            }
            chaseBuildingenemies[i].transform.position = (Vector2)spawnpoints[c++].position + (offset * enemySize * count);
            chaseBuildingenemies[i].GetComponent<EnemyChaseBuildingComponent>().StartNightPhaseA_STAR(grid);
         
        }
        for (int i = 0; i < enemyChaseBothcount; i++)
        {
            if (c >= 4)
                c = 0;
            switch ((SPAWN_TYPE)c)
            {
                case SPAWN_TYPE.NORTH:
                    {
                        offset = Vector2.right;
                        enemySize = enemyChaseBoth.GetComponent<BoxCollider2D>().size.x;
                        north++;
                        count = north;
                        break;
                    }
                case SPAWN_TYPE.EAST:
                    {
                        enemySize = enemyChaseBoth.GetComponent<BoxCollider2D>().size.y;
                        offset = Vector2.down;
                        east++;
                        count = east;
                        break;
                    }
                case SPAWN_TYPE.WEST:
                    {
                        enemySize = enemyChaseBoth.GetComponent<BoxCollider2D>().size.y;
                        offset = Vector2.down;
                        west++;
                        count = west;
                        break;
                    }
                case SPAWN_TYPE.SOUTH:
                    {
                        enemySize = enemyChaseBoth.GetComponent<BoxCollider2D>().size.x;
                        offset = Vector2.right;
                        south++;
                        count = south;
                        break;
                    }

            }
            chaseBothenemies[i].transform.position = (Vector2)spawnpoints[c++].position + (offset * enemySize * count);
            chaseBothenemies[i].GetComponent<EnemyChasePlayerAndBuildingComponent>().StartNightPhaseA_STAR(grid);
         
        }
    }
}
