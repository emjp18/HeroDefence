using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats
{
    float movementSpeed;
    float attackRangePlayer;
    float attackRangeBuilding;
    float chasePlayerRange;
    float health;
    public float Health
    {
        get => health;
        set => health = value;
    }
    public float ChasePlayerRange
    {
        get => chasePlayerRange;
        set => chasePlayerRange = value;
    }
    public float AttackBuildingRange
    {
        get => attackRangeBuilding;
        set => attackRangeBuilding = value;
    }
    public float AttackPlayerRange
    {
        get => attackRangePlayer;
        set => attackRangePlayer = value;
    }
    public float MovementSpeed
    {
        get => movementSpeed;
        set => movementSpeed = value;
    }
    public EnemyStats(ENEMY_TYPE type)
    {
        switch(type)
        {
            case ENEMY_TYPE.BASIC:
                {
                    movementSpeed = 150;
                    chasePlayerRange = 10;
                    health = 50;
                    break;
                }
            case ENEMY_TYPE.RANGE:
                {
                    movementSpeed = 100;
                    chasePlayerRange = 20;
                    health = 50;
                    break;
                }
            case ENEMY_TYPE.EXPLOSIVE:
                {
                    movementSpeed = 175;
                    chasePlayerRange = -1;
                    health = 50;
                    break;
                }
            case ENEMY_TYPE.BOSS:
                {
                    movementSpeed = 150;
                    chasePlayerRange = Utility.GRID_CELL_SIZE_LARGE*1.5f;
                    health = 100;
                    break;
                }
            
        }
    }
    
    
}
