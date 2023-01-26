using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestInteractable : Interactable
{
    public bool isInteracting;
    public FirstPersonController fps;
    public InteractionMenu interaction;
    public string dialogueText1;

    public string dialogueToSend;

    private void Start()
    {
        interaction = GameObject.Find("Canvas").GetComponent<InteractionMenu>();
        fps = GameObject.Find("Player").GetComponent<FirstPersonController>();
    }

    public void PickDialogue()
    {
        dialogueToSend = dialogueText1;
    }
    public override void OnFocus()
    {
    }
    public override void OnInteract()
    {
        if (interaction.monsterMind == null)
        {
            interaction.Initialize(this.gameObject);
            Debug.Log(this.gameObject);
            Debug.Log("interacting");
        }
        else Debug.Log("already interracting with other");
    }
    public override void OnLoseFocus()
    {
        if (interaction.menu.activeInHierarchy)
        {
            interaction.Leave();
        }
        else Debug.Log("already running coroutine");
    }

}
