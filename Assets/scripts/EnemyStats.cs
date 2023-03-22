using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats
{
    float health;
    float quickDamage;
    float heavyDamage;

    public float Health
    {
        get { return health; }
        set { health = value; }
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
                    break;
                }
            case ENEMY_TYPE.CHASE_BUILDING:
                {
                    health = 10;
                    quickDamage = 5;
                    heavyDamage = 10;
                    break;
                }
            case ENEMY_TYPE.CHASE_PLAYER:
                {
                    health = 10;
                    quickDamage = 5;
                    heavyDamage = 10;
                    break;

                }
            case ENEMY_TYPE.CHASE_BOTH:
                {
                    health = 10;
                    quickDamage = 5;
                    heavyDamage = 10;
                    break;
                }
        }
    }
    
    
}
