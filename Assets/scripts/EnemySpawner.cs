using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SPAWN_TYPE { NORTH = 0, EAST = 2, WEST = 3, SOUTH = 1 } //make sure the spawn points are in this order
public enum ENEMY_TYPE {  SWARMER }
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] AiGrid grid;
    [SerializeField] List<Transform> hidePoints;
    [SerializeField] List<Transform> spawnpoints; //north +x, south +x, east - y west - y
    [SerializeField] int maxSwarmerAmount = 30;
    [SerializeField] EnemyBase enemyA;
    //List<EnemyBase> enemiesNorth = new List<EnemyBase>();
    //List<EnemyBase> enemiesSouth = new List<EnemyBase>();
    //List<EnemyBase> enemiesWest= new List<EnemyBase>();
    //List<EnemyBase> enemiesEast = new List<EnemyBase>();
    List<EnemyBase> enemiesSwarm = new List<EnemyBase>();
    [SerializeField] int swarmWaveAmount = 10;
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
    FlockWeights flockWeights = new FlockWeights();
    
    private void Start()
    {
        flockWeights.align = AlignPower;
        flockWeights.separateAgents = separatePower;
        flockWeights.cohesive = CohesionPower;
        flockWeights.separateStatic = separateObjectPower;
        flockWeights.moveToTarget = targetPower;
       

        switch(enemyA.Enemytype)
        {
            case ENEMY_TYPE.SWARMER:
                {
                    for (int i = 0; i < maxSwarmerAmount; i++)
                    {
                        enemiesSwarm.Add(Instantiate(enemyA));
                        enemiesSwarm[i].Init(flockWeights, grid);
                        enemiesSwarm[i].gameObject.SetActive(false);

                    }

                   
                    break;
                }
        }

        enemyA.gameObject.SetActive(false);



    }
    public void EndNightPhase()
    {

        for (int i = 0; i < swarmWaveAmount; i++)
        {
            enemiesSwarm[i].gameObject.SetActive(false);
        }

    }
    public void StartNightPhase()
    {
        for(int j=0; j<hidePoints.Count; j++)
        {
            for (int i = 0; i < swarmWaveAmount; i++)
            {
                
                enemiesSwarm[i + (swarmWaveAmount * j)].gameObject.SetActive(true);
                enemiesSwarm[i + (swarmWaveAmount * j)].gameObject.transform.position =
                    (Vector2)hidePoints[j].position +
                    (i *
                    enemiesSwarm[i + (swarmWaveAmount * j)].gameObject.GetComponent<BoxCollider2D>().size.x * Vector2.down);
                enemiesSwarm[i + (swarmWaveAmount * j)].StartNightPhase(grid);

               

            }
        }
       

    }
}
