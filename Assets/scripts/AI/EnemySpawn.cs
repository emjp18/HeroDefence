using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public enum ENEMY_TYPE { MINI_BOSS, CHASE_PLAYER, CHASE_BUILDING, CHASE_BOTH}
public class EnemySpawn : MonoBehaviour
{
    [SerializeField] int maxEnemiesChasePlayer = 10;
    [SerializeField] int maxenemyChaseBuildingcount = 20;
    [SerializeField] int maxenemyChaseBothcount = 10;
    [SerializeField] AiGrid2 grid;
    [SerializeField] List<Transform> spawnpoints;
    [SerializeField] int enemyChasePlayercount = 0;
    [SerializeField] int enemyChaseBuildingcount = 0;
    [SerializeField] int enemyChaseBothcount = 0;
    [SerializeField] GameObject enemyChasePlayer;
    [SerializeField] GameObject enemyChaseBuilding;
    [SerializeField] GameObject enemyChaseBoth;
    List<GameObject> chasePlayerenemies = new List<GameObject>();
    List<GameObject> chaseBuildingenemies = new List<GameObject>();
    List<GameObject> chaseBothenemies = new List<GameObject>();
    int waveNumber = 0;
    void UpdateEnemyLists()
    {
       
        for (int i = 0; i < enemyChasePlayercount; i++)
        {
           
            chasePlayerenemies[i].SetActive(true);
        }
        for (int i = 0; i < enemyChaseBuildingcount; i++)
        {
           
            chaseBuildingenemies[i].SetActive(true);
        }
        for (int i = 0; i < enemyChaseBothcount; i++)
        {

            chaseBothenemies[i].SetActive(true);
        }
    }
    private void Start()//allocates a fixed number of enemies once instead of doign instantiate again and again
    {
        enemyChasePlayer.SetActive(false);
        enemyChaseBuilding.SetActive(false);
        enemyChaseBoth.SetActive(false);
        Physics2D.SetLayerCollisionMask(6, 7);
        Physics2D.SetLayerCollisionMask(6, 6);
        for (int i = 0; i < maxEnemiesChasePlayer; i++)
        {
            chasePlayerenemies.Add(Instantiate(enemyChasePlayer));
            chasePlayerenemies[i].SetActive(false);
        }
        for (int i = 0; i < maxenemyChaseBuildingcount; i++)
        {
            chaseBuildingenemies.Add(Instantiate(enemyChaseBuilding));
            chaseBuildingenemies[i].SetActive(false);
        }
        for (int i = 0; i < maxenemyChaseBothcount; i++)
        {
            chaseBothenemies.Add(Instantiate(enemyChaseBoth));
            chaseBothenemies[i].SetActive(false);
        }
    }
    public void EndNightPhase()
    {

        for (int i = 0; i < enemyChasePlayercount; i++)
        {
           
            chasePlayerenemies[i].SetActive(false);
        }
        for (int i = 0; i < enemyChaseBuildingcount; i++)
        {
       
            chaseBuildingenemies[i].SetActive(false);
        }
        for (int i = 0; i < enemyChaseBothcount; i++)
        {
          
            chaseBothenemies[i].SetActive(false);
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
        for (int i = 0; i < enemyChasePlayercount; i++)
        {
            if(c>=spawnpoints.Count)
                c = 0;
            chasePlayerenemies[i].transform.position = spawnpoints[c++].position;
            chasePlayerenemies[i].GetComponent<EnemyChasePlayerComponent>().StartNightPhase();
          
        }
        for (int i = 0; i < enemyChaseBuildingcount; i++)
        {
            if (c >= spawnpoints.Count)
                c = 0;
            chaseBuildingenemies[i].transform.position = spawnpoints[c++].position;
            chaseBuildingenemies[i].GetComponent<EnemyChaseBuildingComponent>().StartNightPhase();
         
        }
        for (int i = 0; i < enemyChaseBothcount; i++)
        {
            if (c >= spawnpoints.Count)
                c = 0;
            chaseBothenemies[i].transform.position = spawnpoints[c++].position;
            chaseBothenemies[i].GetComponent<EnemyChasePlayerAndBuildingComponent>().StartNightPhase();
         
        }
    }
}
