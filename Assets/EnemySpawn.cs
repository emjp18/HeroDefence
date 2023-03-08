using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemySpawn : MonoBehaviour
{
    bool night = false;
    [SerializeField] int maxEnemies = 5;
    [SerializeField] float spawnFrequency = 10;
    int enemySpawned = 0;
    float time = 0;
    
    [SerializeField] string enemyName = "Enemy";
    GameObject enemy;
    [SerializeField] List<Transform> targets;
    [SerializeField] List<Transform> spawnPoints;
    int targetNr = 0;
    void Start()
    {
        Physics2D.IgnoreLayerCollision(6, 6);
        Physics2D.IgnoreLayerCollision(6, 7);//This helps the pathfinding, enemies will ignore collision but if they accidentally touches
        //they will not stop but pass through the objects.
        enemy = GameObject.Find(enemyName);
        var AIComp = enemy.GetComponent<AI>();
        var grid = AIComp.GetGrid();
        foreach (Transform target in targets)
        {
           
            Vector2 targetPos = target.position;
            grid.SetTargetToValidCell(ref targetPos);
            target.position = targetPos;
        }
       
    }

   
    void Update()
    {
        if(night&&enemySpawned<maxEnemies)
        {
            if(time>spawnFrequency)
            {
                time = 0.0f;
                foreach (Transform t in spawnPoints)
                {
                    Instantiate(enemy, t.position, t.rotation);
                    if (targetNr >= targets.Count)
                        targetNr = 0;
                    enemy.GetComponent<AI>().SetTarget(targets[targetNr++]);
                    enemySpawned++;
                }
            }
            else
            {
                time += Time.deltaTime;
            }
            
        }
    }
    public void SetNight(bool night)
    {
        this.night = night;
    }
    public void SetEnemyToSpawn(string nameOfGameObject)
    {
        enemyName = nameOfGameObject;
    }
}
