using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInteraction : Interactable
{
    private InteractionMenu interactionMenu;
    private void Start()
    {
        interactionMenu = GameObject.Find("Canvas").GetComponent<InteractionMenu>();
    }
    public override void OnFocus()
    {
        if (!interactionMenu.isInteracting)
        {
            interactionMenu.currentMonster = this.gameObject.GetComponent<MonsterMind>();
        }
    }

    public override void OnInteract()
    {
        if (!interactionMenu.isInteracting)
        {
            interactionMenu.OpenInteractionMenu();
        }
        else Debug.Log("already interacting");

    }
    public override void OnLoseFocus()
    {
        if (interactionMenu.isMenuOpen)
        {
            interactionMenu.CloseInteractionMenu();
        }
    }
}
