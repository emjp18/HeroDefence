using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Item 
{
    public enum ItemType
    {
        Boots,
        HealthPotion,
        MagicMushroom
    }

    public static int GetCost(ItemType itemType)
    {
        switch (itemType)
        {
            default:
                case ItemType.Boots: return 50;
                case ItemType.HealthPotion: return 30;
                case ItemType.MagicMushroom: return 999;
        }
    }

    public static Sprite GetSprite(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.Boots: return GameAssets.i.bootsSprite;
            case ItemType.HealthPotion: return GameAssets.i.healthPotionSprite;
            case ItemType.MagicMushroom: return GameAssets.i.magicMushroomSprite;
        }
    }
}
