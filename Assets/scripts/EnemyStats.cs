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
    float chaseTargetRange;
    float movementRestrictionRange;
    public float Health
    {
        get { return health; }
        set { health = value; }
    }
    public float MovementRange
    {
        get { return movementRestrictionRange; }
        set { movementRestrictionRange = value; }
    }
    public float ChaseTargetRange
    {
        get { return chaseTargetRange; }
        set { chaseTargetRange = value; }
    }
    public float ChasePlayerRange
    {
        get { return chasePlayerRange; }
        set { chasePlayerRange = value; }
    }
    public float AttackRange
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

            case ENEMY_TYPE.SWARMER:
                {
                 
                    quickDamage = 5;
                    movementSpeed = 150;
                    heavyDamage = 10;
                    break;
                }
            case ENEMY_TYPE.ARMY:
                {
                    chasePlayerRange = 4;
                    quickDamage = 5;
                    movementSpeed = 100;
                    heavyDamage = 10;
                    break;
                }
        }
    }
    
    
}
