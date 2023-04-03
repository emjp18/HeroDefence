using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENEMY_TYPE {  BASIC,EXPLOSIVE, RANGE,BOSS  }
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform buildingTarget;
    [SerializeField] AiGrid grid;
    [SerializeField] AiGrid bossGrid;
    [SerializeField] List<Transform> spawnPoints;
    [SerializeField] GameObject hitObject;
    [SerializeField] int enemiesBasicPerSpawn = 5;
    [SerializeField] int explosivePerSpawn = 3;
    [SerializeField] int rangePerSpawn = 5;
    [SerializeField] int bossPerSpawn = 1;
    [SerializeField] List<EnemyBase> enemyPrefabTemplates;
    List<EnemyBase> enemiesBasic = new List<EnemyBase>();
    List<EnemyBase> enemiesBoss= new List<EnemyBase>();
    List<EnemyBase> enemiesRange = new List<EnemyBase>();
    List<EnemyBase> enemiesExplosive = new List<EnemyBase>();
    [SerializeField]  int nightphase = 0;
    public bool startNight = true;
    public bool endNight = false;
   
    private void Start()
    {

       
        foreach( EnemyBase enemy in enemyPrefabTemplates)
        {
            switch (enemy.Enemytype)
            {
                case ENEMY_TYPE.BASIC:
                    {
                        for(int i=0; i< enemiesBasicPerSpawn* spawnPoints.Count; i++)
                        {
                            enemiesBasic.Add(Instantiate(enemy));
                            enemiesBasic[i].Init(grid, player, buildingTarget,Instantiate(hitObject));
                            enemiesBasic[i].gameObject.SetActive(false);
                        }
                        break;
                    }
                case ENEMY_TYPE.RANGE:
                    {
                        for (int i = 0; i < rangePerSpawn * spawnPoints.Count; i++)
                        {
                           enemiesRange.Add(Instantiate(enemy));
                            enemiesRange[i].Init(grid, player, buildingTarget, Instantiate(hitObject));
                            enemiesRange[i].gameObject.SetActive(false);
                        }
                        break;
                    }
                case ENEMY_TYPE.EXPLOSIVE:
                    {
                        for (int i = 0; i < explosivePerSpawn * spawnPoints.Count; i++)
                        {
                            enemiesExplosive.Add(Instantiate(enemy));
                            enemiesExplosive[i].Init(grid, player, buildingTarget, Instantiate(hitObject));
                            enemiesExplosive[i].gameObject.SetActive(false);
                        }
                        break;
                    }
                case ENEMY_TYPE.BOSS:
                    {
                        for (int i = 0; i < bossPerSpawn * spawnPoints.Count; i++)
                        {
                            enemiesBoss.Add(Instantiate(enemy));
                            enemiesBoss[enemiesBoss.Count-1].Init(bossGrid, player, buildingTarget,
                                Instantiate(hitObject));
                            enemiesBoss[enemiesBoss.Count - 1].gameObject.SetActive(false);
                        }
                        break;
                    }
            }
            hitObject.SetActive(false);
            enemy.gameObject.SetActive(false);
        }
        



    }
    public void EndNightPhase()
    {

       foreach (EnemyBase enemy in enemiesBasic)
        {
            enemy.gameObject.SetActive(false);
        }
        foreach (EnemyBase enemy in enemiesBoss)
        {
            enemy.gameObject.SetActive(false);
        }
        foreach (EnemyBase enemy in  enemiesExplosive)
        {
            enemy.gameObject.SetActive(false);
        }
        foreach (EnemyBase enemy in enemiesRange)
        {
            enemy.gameObject.SetActive(false);
        }

    }
    public void StartNightPhase()
    {
        grid.RegenerateGrid();
        bossGrid.RegenerateGrid();
        Utility.UpdateStaticCollision(grid);
        Utility.UpdateStaticCollisionLarge(bossGrid);
        int sp = spawnPoints.Count;

        switch(nightphase)
        {
            case 0:
                {
                    for (int i = 0; i < sp; i++)
                    {
                        for (int j = 0; j < enemiesBasicPerSpawn; j++)
                        {
                            enemiesBasic[j + i * enemiesBasicPerSpawn].transform.position = spawnPoints[i].position;
                            enemiesBasic[j + i * enemiesBasicPerSpawn].gameObject.SetActive(true);
                            enemiesBasic[j + i * enemiesBasicPerSpawn].StartNightPhase(grid);
                        }

                    }
                    break;
                }
            case 1:
                {
                    for (int i = 0; i < sp; i++)
                    {
                        for (int j = 0; j < bossPerSpawn; j++)
                        {
                            enemiesBoss[j + i * bossPerSpawn].transform.position = spawnPoints[i].position;
                            enemiesBoss[j + i * bossPerSpawn].gameObject.SetActive(true);
                            enemiesBoss[j + i * bossPerSpawn].StartNightPhase(bossGrid);
                        }

                    }
                    break;
                }
            case 2:
                {
                    for (int i = 0; i < sp; i++)
                    {
                        for (int j = 0; j < rangePerSpawn; j++)
                        {
                            enemiesRange[j + i * rangePerSpawn].transform.position = spawnPoints[i].position;
                            enemiesRange[j + i * rangePerSpawn].gameObject.SetActive(true);
                            enemiesRange[j + i * rangePerSpawn].StartNightPhase(grid);
                        }

                    }
                    break;
                }
            case 3:
                {
                    for (int i = 0; i < sp; i++)
                    {
                        for (int j = 0; j < explosivePerSpawn; j++)
                        {
                            enemiesExplosive[j + i * explosivePerSpawn].transform.position = spawnPoints[i].position;
                            enemiesExplosive[j + i * explosivePerSpawn].gameObject.SetActive(true);
                            enemiesExplosive[j + i * explosivePerSpawn].StartNightPhase(grid);
                        }

                    }
                    break;
                }
        }


       

    }


    private void Update()
    {
        
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
