using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject containerGameObject;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private TextMeshProUGUI interactTextMeshProUGUI;


    private void Update()
    {
        if(playerInteract.GetInteractableObject() != null) //if close to npc, show interact UI
        {
            Show(playerInteract.GetInteractableObject());
        }
        else
        {
            Hide(); //if not...
        }
    }
    private void Show(NPCInteractable npcInteractable)
    {
        containerGameObject.SetActive(true);
        interactTextMeshProUGUI.text = npcInteractable.GetInteractText();
    }
    private void Hide()
    {
        containerGameObject.SetActive(false);
    }
}
