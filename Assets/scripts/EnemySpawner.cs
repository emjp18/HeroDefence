using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENEMY_TYPE {  SWARM,ARMY,BOMB,RANGE,BOSS, ARMY1  }
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] AiGrid grid;
    [SerializeField] List<Transform> hidePoints;
    [SerializeField] List<Transform> hideDistancePoints;
    [SerializeField] List<Transform> spawnPoints;
    [SerializeField] List<Transform> targetPoints;
    [SerializeField] int maxSwarmerAmount = 40;
    int swarmAmountPerHidePoint;
    [SerializeField] int maxArmyamount = 40;
    int armyAmountPerSpawnPoint;
    [SerializeField] int maxBombAmount = 16;
    int bombamountperSpawn;
    [SerializeField] int maxRangeamount = 16;
    int rangeamountperSpawn;
    [SerializeField] int maxArmy1amount = 10;
    int army1AmountPerSpawn;
    [SerializeField] List<EnemyBase> enemyPrefabTemplates;
    List<EnemyBase> enemiesArmy = new List<EnemyBase>();
    List<EnemyBase> enemiesSwarm = new List<EnemyBase>();
    List<EnemyBase> enemiesBomb = new List<EnemyBase>();
    List<EnemyBase> enemiesRange = new List<EnemyBase>();
    List<EnemyBase> enemiesboss = new List<EnemyBase>();
    List<EnemyBase> enemiesarmy1s = new List<EnemyBase>();
    public bool startNight = true;
    public bool endNight = false;
   
    private void Start()
    {
        
        army1AmountPerSpawn = maxArmy1amount/ spawnPoints.Count;

        foreach ( EnemyBase enemy in enemyPrefabTemplates)
        {
            switch (enemy.Enemytype)
            {
                case ENEMY_TYPE.SWARM:
                    {
                        int id = 0;
                        for (int i = 0; i < maxSwarmerAmount; i++)
                        {
                            if (i == swarmAmountPerHidePoint * (id+1))
                                id++;
                            enemiesSwarm.Add(Instantiate(enemy));
                            enemiesSwarm[i].Init( grid, swarmAmountPerHidePoint, id, hidePoints[id],
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
                            if (i == armyAmountPerSpawnPoint * (id + 1))
                                id++;
                            enemiesArmy.Add(Instantiate(enemy));
                            enemiesArmy[i].Init(grid, armyAmountPerSpawnPoint, id, hidePoints[id],
                                hideDistancePoints[id], player);
                            enemiesArmy[i].gameObject.SetActive(false);

                        }


                        break;
                    }
                case ENEMY_TYPE.BOMB:
                    {
                        int id = 0;
                        for (int i = 0; i < maxBombAmount; i++)
                        {
                            if (i == bombamountperSpawn * (id + 1))
                                id++;
                            enemiesBomb.Add(Instantiate(enemy));
                            enemiesBomb[i].Init(grid, bombamountperSpawn, id, hidePoints[id],
                                hideDistancePoints[id], player);
                            enemiesBomb[i].gameObject.SetActive(false);

                        }


                        break;
                    }
                case ENEMY_TYPE.RANGE:
                    {
                        int id = 0;
                        for (int i = 0; i < maxRangeamount; i++)
                        {
                            if (i == rangeamountperSpawn * (id + 1))
                                id++;
                            enemiesRange.Add(Instantiate(enemy));
                            enemiesRange[i].Init(grid, rangeamountperSpawn, id, hidePoints[id],
                                hideDistancePoints[id], player);
                            enemiesRange[i].gameObject.SetActive(false);

                        }


                        break;
                    }
                case ENEMY_TYPE.BOSS:
                    {
                        for(int i=0; i<4;i++ )
                        {
                            enemiesboss.Add(Instantiate(enemy));
                            enemiesboss[i].Init(grid, 0, 0, hidePoints[0],
                                hideDistancePoints[0], player);
                            enemiesboss[i].gameObject.SetActive(false);
                        }
                  


                        break;
                    }
                case ENEMY_TYPE.ARMY1:
                    {
                        for (int i = 0; i < maxArmy1amount; i++)
                        {
                            enemiesarmy1s.Add(Instantiate(enemy));
                            if(i==0)
                            {
                                enemiesarmy1s[i].Init(grid,army1AmountPerSpawn, 0,player,true);
                            }
                            else
                            {
                                enemiesarmy1s[i].Init(grid, army1AmountPerSpawn, 0, player);
                            }
                            enemiesarmy1s[i].gameObject.SetActive(false);
                        }



                        break;
                    }
            }

            enemy.gameObject.SetActive(false);
        }
        



    }
    public void EndNightPhase()
    {

        for (int i = 0; i < swarmAmountPerHidePoint; i++)
        {
            enemiesSwarm[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < bombamountperSpawn; i++)
        {
            enemiesBomb[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < armyAmountPerSpawnPoint; i++)
        {
            enemiesArmy[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < rangeamountperSpawn; i++)
        {
            enemiesRange[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < 4; i++)
        {
            enemiesboss[i].gameObject.SetActive(false);
        }

    }
    public void StartNightPhase()
    {
        grid.RegenerateGrid();
        //for(int j=0; j<hidePoints.Count; j++)
        //{
        //    if(spawnRange==false&&spawnBomb==false&&spawnBoss==false)
        //    {
        //        for (int i = 0; i < swarmAmountPerHidePoint; i++)
        //        {

        //            enemiesSwarm[i + (swarmAmountPerHidePoint * j)].gameObject.SetActive(true);
        //            enemiesSwarm[i + (swarmAmountPerHidePoint * j)].gameObject.transform.position =
        //                (Vector2)hidePoints[j].position;

        //            enemiesSwarm[i + (swarmAmountPerHidePoint * j)].StartNightPhase(grid);



        //        }
               
        //    }
        //    else if(spawnRange == true&&spawnBomb==false && spawnBoss == false)
        //    {
        //        for (int i = 0; i < rangeamountperSpawn; i++)
        //        {

        //            enemiesRange[i + (rangeamountperSpawn * j)].gameObject.SetActive(true);
        //            enemiesRange[i + (rangeamountperSpawn * j)].gameObject.transform.position =
        //                (Vector2)hidePoints[j].position + Random.insideUnitCircle.normalized * i;
                   
        //            enemiesRange[i + (rangeamountperSpawn * j)].SetTarget(targetPoints[j]);
        //            enemiesRange[i + (rangeamountperSpawn * j)].StartNightPhase(grid);
        //            enemiesRange[i + (rangeamountperSpawn * j)].eStats().Speed =
        //              enemiesRange[i + (rangeamountperSpawn * j)].eStats().Speed + Random.Range(-30, 30);

        //        }
        //    }
        //    else if (spawnRange ==false && spawnBomb == true && spawnBoss == false)
        //    {
        //        for (int i = 0; i < bombamountperSpawn; i++)
        //        {

                    
        //            enemiesBomb[i + (bombamountperSpawn * j)].gameObject.SetActive(true);
        //            enemiesBomb[i + (bombamountperSpawn * j)].gameObject.transform.position =
        //                (Vector2)hidePoints[j].position + Random.insideUnitCircle.normalized * i;
                        

                    
        //            enemiesBomb[i + (bombamountperSpawn * j)].SetTarget(targetPoints[j]);
        //            enemiesBomb[i + (bombamountperSpawn * j)].StartNightPhase(grid);
        //            enemiesBomb[i + (bombamountperSpawn * j)].eStats().Speed =
        //                 enemiesBomb[i + (bombamountperSpawn * j)].eStats().Speed + Random.Range(-30, 30);

        //        }
        //    }
           

            
            
        //}
        for (int j = 0; j < spawnPoints.Count; j++)
        {
            Vector2 dir = (targetPoints[j].position - spawnPoints[j].position).normalized;
            dir *= -1;

           
            for (int i = 0; i < army1AmountPerSpawn; i++)
            {

                enemiesarmy1s[i + (army1AmountPerSpawn * j)].gameObject.SetActive(true);
                enemiesarmy1s[i + (army1AmountPerSpawn * j)].gameObject.transform.position =
                    (Vector2)spawnPoints[j].position + dir * i;
                enemiesarmy1s[i + (army1AmountPerSpawn * j)].SetTarget(targetPoints[j]);
                enemiesarmy1s[i + (army1AmountPerSpawn * j)].StartNightPhase(grid);



            }
        }

    }
  

    private void Update()
    {
        for (int i = 1; i < army1AmountPerSpawn; i++)
        {
            (enemiesarmy1s[i] as Army1).SetLeader(enemiesarmy1s[0].transform.position);
        }
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
    }
}
