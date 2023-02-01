using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionMenu : MonoBehaviour
{
    public GameObject menu;
    public bool isMenuOpen = false;
    public MonsterMind currentMonster;
    private FirstPersonController fps;

    private GameObject dialogueBox;
    private TextMeshProUGUI dialogueText;
    public bool isInteracting;
    void Start()
    {
        menu = GameObject.Find("InteractionMenu");
        menu.SetActive(false);
        fps = GameObject.Find("Player").GetComponent<FirstPersonController>();
        dialogueBox = GameObject.Find("DialogueBox");
        dialogueText = GameObject.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        dialogueBox.SetActive(false);
        dialogueText.text = null;
    }
    private void Update()
    {
        if (isMenuOpen)
        {
            fps.menuOpen = true;
        }
        if (!isMenuOpen)
        {
            fps.menuOpen = false;
        }
    }
    public void OpenInteractionMenu()
    {
        isMenuOpen = true;
        menu.SetActive(true);
    }
    public void CloseInteractionMenu()
    {
        isMenuOpen = false;
        menu.SetActive(false);
    }
    public void TalkTo()
    {
        if (currentMonster.canTalk)
        {
            StartCoroutine(TalkToCoroutine());
        }
        else
        {
            Debug.Log("cant talk coroutine here");
        }
    }
    private IEnumerator TalkToCoroutine()
    {
        isInteracting = true;
        dialogueBox.SetActive(true);
        currentMonster.CheckDialogueText();
        dialogueText.text = currentMonster.lineToSend;
        yield return new WaitForSeconds(5f);
        dialogueBox.SetActive(false);
        dialogueText.text = null;
        if (!currentMonster.alreadyTalked && currentMonster.likesTalking)
        {
            currentMonster.IncreaseAffection();
        }
        currentMonster.alreadyTalked = true;
        isInteracting = false;
    }
    public void Use()
    {

    }
    public void Inspect()
    {

    }
    public void Feed()
    {

    }

}
