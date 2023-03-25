using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENEMY_TYPE {  SWARMER,ARMY  }
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] AiGrid grid;
    [SerializeField] List<Transform> hidePoints;
    [SerializeField] List<Transform> spawnPoints;//need to be in the same order as target
                                                 //points. meaning northspawn == nothtarget
    [SerializeField] List<Transform> targetPoints;
    [SerializeField] int maxSwarmerAmount = 40;
    [SerializeField] List<EnemyBase> enemyPrefabTemplates;
    List<EnemyBase> enemiesArmy = new List<EnemyBase>();
    List<EnemyBase> enemiesSwarm = new List<EnemyBase>();
    [SerializeField] int swarmWaveAmountPerHidePoint = 10;
    [SerializeField] int maxArmyamount = 40;
    [SerializeField] int armyWaveAmountPerSpawnPoint = 10;
    public bool startNight = true;
    public bool endNight = false;
    private void Start()
    {
        


        foreach ( EnemyBase enemy in enemyPrefabTemplates)
        {
            switch (enemy.Enemytype)
            {
                case ENEMY_TYPE.SWARMER:
                    {
                        for (int i = 0; i < maxSwarmerAmount; i++)
                        {
                            enemiesSwarm.Add(Instantiate(enemy));
                            enemiesSwarm[i].Init( grid, swarmWaveAmountPerHidePoint);
                            enemiesSwarm[i].gameObject.SetActive(false);

                        }


                        break;
                    }
                case ENEMY_TYPE.ARMY:
                    {
                        for (int i = 0; i < maxArmyamount; i++)
                        {
                            enemiesArmy.Add(Instantiate(enemy));
                            enemiesArmy[i].Init(grid, armyWaveAmountPerSpawnPoint);
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
            for (int i = 0; i < armyWaveAmountPerSpawnPoint; i++)
            {

                enemiesArmy[i + (armyWaveAmountPerSpawnPoint * j)].gameObject.SetActive(true);
                enemiesArmy[i + (armyWaveAmountPerSpawnPoint * j)].gameObject.transform.position =
                    (Vector2)spawnPoints[j].position;
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
