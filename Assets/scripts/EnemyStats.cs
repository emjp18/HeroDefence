using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats
{
    float movementSpeed;
    float attackRangePlayer;
    float attackRangeBuilding;
    float chasePlayerRange;
    
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
                    break;
                }
            case ENEMY_TYPE.RANGE:
                {
                    movementSpeed = 100;
                    chasePlayerRange = 20;
                    break;
                }
            case ENEMY_TYPE.EXPLOSIVE:
                {
                    movementSpeed = 175;
                    chasePlayerRange = -1;
                    break;
                }
            case ENEMY_TYPE.BOSS:
                {
                    movementSpeed = 50;
                    chasePlayerRange = Utility.GRID_CELL_SIZE_LARGE*1.5f;

                    break;
                }

        }
    }
    
    
}
