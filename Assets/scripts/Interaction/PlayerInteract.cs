using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.UI;




public class PlayerInteract : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {

            GetInteractableObject();
        }

    }

    public NPCInteractable GetInteractableObject()
    {
        List<NPCInteractable> npcInteractableList = new List<NPCInteractable>();
        float interactRange = 1f;
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, interactRange); //check for interactable colliders within player collider
        foreach (Collider2D collider in colliderArray)
        {

            if (collider.TryGetComponent(out NPCInteractable npcInteractable))
            {
                npcInteractableList.Add(npcInteractable);
                if (Input.GetKeyDown(KeyCode.F))
                {

                    npcInteractable.Interact();  //if player is within other interactable collider, call for interact method. 
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
                    closestNPCInteractable = npcInteractable; //check to see the closest interactable collider, for example if you are talking with someone, and walk to a new "closest" npc, change who you can talk to
                }
            }
        }
        return closestNPCInteractable;
    }



}
