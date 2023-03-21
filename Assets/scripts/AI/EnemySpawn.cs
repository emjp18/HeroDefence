using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public enum ENEMY_TYPE { MINI_BOSS, CHASE_PLAYER, CHASE_BUILDING, CHASE_BOTH}
public class EnemySpawn : MonoBehaviour
{
    [SerializeField] List<GameObject> enemies;
    private void Start()
    {
        
    }
    private void Update()
    {
        
    }
}
