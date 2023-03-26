using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENEMY_TYPE {  SWARM,ARMY,BOMB,RANGE  }
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] AiGrid grid;
    [SerializeField] List<Transform> hidePoints;
    [SerializeField] List<Transform> hideDistancePoints;
    [SerializeField] List<Transform> spawnPoints;
    [SerializeField] List<Transform> targetPoints;
    [SerializeField] int maxSwarmerAmount = 40;
    [SerializeField] int swarmWaveAmountPerHidePoint = 10;
    [SerializeField] int maxArmyamount = 40;
    [SerializeField] int armyWaveAmountPerSpawnPoint = 10;
    [SerializeField] List<EnemyBase> enemyPrefabTemplates;
    List<EnemyBase> enemiesArmy = new List<EnemyBase>();
    List<EnemyBase> enemiesSwarm = new List<EnemyBase>();
  
    public bool startNight = false;
    public bool endNight = false;
    private void Start()
    {
        


        foreach ( EnemyBase enemy in enemyPrefabTemplates)
        {
            switch (enemy.Enemytype)
            {
                case ENEMY_TYPE.SWARM:
                    {
                        int id = 0;
                        for (int i = 0; i < maxSwarmerAmount; i++)
                        {
                            if (i == swarmWaveAmountPerHidePoint*(id+1))
                                id++;
                            enemiesSwarm.Add(Instantiate(enemy));
                            enemiesSwarm[i].Init( grid, swarmWaveAmountPerHidePoint, id, hidePoints[id],
                                hideDistancePoints[id],player);
                            enemiesSwarm[i].gameObject.SetActive(false);

                        }


                        break;
                    }
                case ENEMY_TYPE.ARMY:
                    {
                        int id = 0;
                        for (int i = 0; i < maxArmyamount; i++)
                        {
                            if (i == armyWaveAmountPerSpawnPoint * (id + 1))
                                id++;
                            enemiesArmy.Add(Instantiate(enemy));
                            enemiesArmy[i].Init(grid, swarmWaveAmountPerHidePoint, id, hidePoints[id],
                                hideDistancePoints[id], player);
                            enemiesArmy[i].gameObject.SetActive(false);

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
        grid.RegenerateGrid();
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
        for (int j = 0; j < spawnPoints.Count; j++)
        {
            Vector2 dir = (targetPoints[j].position - spawnPoints[j].position).normalized;
            float x = dir.x;
            dir.x = dir.y;
            dir.y = x;
           
            for (int i = 0; i < armyWaveAmountPerSpawnPoint; i++)
            {
                
                enemiesArmy[i + (armyWaveAmountPerSpawnPoint * j)].gameObject.SetActive(true);
                enemiesArmy[i + (armyWaveAmountPerSpawnPoint * j)].gameObject.transform.position =
                    (Vector2)spawnPoints[j].position + dir*i;
                enemiesArmy[i + (armyWaveAmountPerSpawnPoint * j)].SetTarget(targetPoints[j]);
                enemiesArmy[i + (armyWaveAmountPerSpawnPoint * j)].StartNightPhase(grid);



            }
        }

    }
  

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
