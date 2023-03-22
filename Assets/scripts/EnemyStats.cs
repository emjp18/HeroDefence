using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats
{
    float health;
    float quickDamage;
    float heavyDamage;
    float attackRange;
    float movementSpeed;
    float chasePlayerRange;
    public float Health
    {
        get { return health; }
        set { health = value; }
    }
    public float PlayerRange
    {
        get { return chasePlayerRange; }
        set { chasePlayerRange = value; }
    }
    public float Range
    {
        get { return attackRange; }
        set { attackRange = value; }
    }
    public float Speed
    {
        get { return movementSpeed; }
        set { movementSpeed = value; }
    }
    public float QuickDamage
    {
        get { return quickDamage; }
        set { quickDamage = value; }
    }
    public float HeavyDamage
    {
        get { return heavyDamage; }
        set { heavyDamage = value; }
    }
    public EnemyStats(ENEMY_TYPE type)
    {
        switch(type)
        {
           
            case ENEMY_TYPE.MINI_BOSS:
                {
                    health = 100;
                    quickDamage = 25;
                    heavyDamage = 50;
                    chasePlayerRange = 10;
                    movementSpeed = 300;
                    attackRange = 3;
                    break;
                }
            case ENEMY_TYPE.CHASE_BUILDING:
                {
                    health = 10;
                    quickDamage = 5;
                    heavyDamage = 10;
                    chasePlayerRange = 10;
                    movementSpeed = 300;
                    attackRange = 3;
                    break;
                }
            case ENEMY_TYPE.CHASE_PLAYER:
                {
                    health = 10;
                    quickDamage = 5;
                    heavyDamage = 10;
                    chasePlayerRange = 20;
                    movementSpeed = 300;
                    attackRange = 3;
                    break;

                }
            case ENEMY_TYPE.CHASE_BOTH:
                {
                    health = 10;
                    quickDamage = 5;
                    heavyDamage = 10;
                    chasePlayerRange = 10;
                    movementSpeed = 300;
                    attackRange = 3;
                    break;
                }
        }
    }
    
    
}
