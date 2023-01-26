using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionMenu : MonoBehaviour
{
    public FirstPersonController fps;
    public MonsterMind monsterMind;
    public GameObject menu;
    public GameObject dialogueBox;


    private void Awake()
    {
        dialogueBox = GameObject.Find("DialogueBox");
        menu = GameObject.Find("InteractionMenu");
        fps = GameObject.Find("Player").GetComponent<FirstPersonController>();
        menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(GameObject obj)
    {
        fps.menuOpen = true;
        Debug.Log("initializing " + obj);
        menu.SetActive(true);
        if (obj.GetComponent<MonsterMind>() != null)
        {
            monsterMind = obj.GetComponent<MonsterMind>();
        }
    }
    public void TalkTo()
    {
        StartCoroutine(TalkCoroutine());
    }
    public void Leave()
    {
        fps.menuOpen = false;
        if (menu.activeInHierarchy)
        {
            menu.SetActive(false);
        }
        monsterMind = null;

    }
    private IEnumerator TalkCoroutine()
    {
        fps.menuOpen = false;
        menu.SetActive(false);
        dialogueBox.SetActive(true);
        TextMeshProUGUI text = dialogueBox.GetComponent<TextMeshProUGUI>();
        monsterMind.GetComponent<TestInteractable>().PickDialogue();
        text.text = monsterMind.gameObject.GetComponent<TestInteractable>().dialogueToSend;
        yield return new WaitForSeconds(5f);
        if (monsterMind.likesTalking)
        {
            monsterMind.affection++;
        }
        else
        {
            monsterMind.affection = monsterMind.affection - 1;
        }
        Leave();
        dialogueBox.SetActive(false);
        text.text = null;
        Debug.Log("done talking");
        yield return null;
    }
}
