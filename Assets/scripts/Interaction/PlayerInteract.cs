using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;


public class PlayerInteract : MonoBehaviour, IShopCustomer
{

    /// <summary>
    /// flytta senare
    /// </summary>
    public int goldAmount = 100;
    public int healthPotionAmount = 0;

    public int GetGoldAmount()
    {
        return goldAmount;
    }
    public event EventHandler OnGoldAmountChanged;
    public event EventHandler OnHealthPotionAmountChanged;
    public int GetHealthPotionAmount()
    {
        return healthPotionAmount;
    }
    private void AddHealthPotion()
    {
        healthPotionAmount++;
        OnHealthPotionAmountChanged?.Invoke(this, EventArgs.Empty);
    }
    //
    //
    //

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

            GetInteractableObject();
        }
    }

    public NPCInteractable GetInteractableObject()
    {
        List<NPCInteractable> npcInteractableList = new List<NPCInteractable>();
        float interactRange = 1f;
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, interactRange);
        foreach (Collider2D collider in colliderArray)
        {

            if (collider.TryGetComponent(out NPCInteractable npcInteractable))
            {
                npcInteractableList.Add(npcInteractable);
                if (Input.GetKeyDown(KeyCode.E))
                {

                    npcInteractable.Interact();
                }

            }
        }

        NPCInteractable closestNPCInteractable = null;
        foreach (NPCInteractable npcInteractable in npcInteractableList)
        {
            if (closestNPCInteractable == null)
            {
                closestNPCInteractable = npcInteractable;
            }
            else
            {
                if (Vector2.Distance(transform.position, npcInteractable.transform.position) < Vector2.Distance(transform.position, closestNPCInteractable.transform.position))
                {
                    closestNPCInteractable = npcInteractable;
                }
            }
        }
        return closestNPCInteractable;
    }


    /// <summary>
    /// 
    /// Player script??
    /// </summary>
    /// <param name="itemType"></param>
    public void BoughtItem(Item.ItemType itemType)
    {
        Debug.Log("Bought item: " + itemType);
        switch (itemType)
        {
            case Item.ItemType.HealthPotion: AddHealthPotion(); break;

        }
    }

    public bool TrySpendGoldAmount(int spendGoldAmount)
    {
        if (GetGoldAmount() >= spendGoldAmount)
        {
            goldAmount -= spendGoldAmount;
            OnGoldAmountChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }
        else
        {
            Debug.Log("du är fattig neiger");
            return false;
        }
    }

    ///
    ///
    ///
}
