using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestInteractable : Interactable
{
    public bool isInteracting;
    public GameObject dialogueBox;
    public string dialogueText;

    private void Start()
    {
        dialogueBox = GameObject.Find("DialogueBox");
    }
    public override void OnFocus()
    {

        Debug.Log("LOOKING AT " + gameObject.name);
    }
    public override void OnInteract()
    {
        if (!isInteracting)
        {
            Debug.Log("interacting");
            StartCoroutine(TestInteraction());
        }
        else Debug.Log("already interacting");
    }
    public override void OnLoseFocus()
    {
        if (isInteracting)
        {
            StopCoroutine(TestInteraction());
            isInteracting = false;
            dialogueBox.SetActive(false);

        }
        Debug.Log("STOPPED LOOKING AT " + gameObject.name);
    }
    private IEnumerator TestInteraction()
    {
        isInteracting = !isInteracting;
        dialogueBox.SetActive(true);
        TextMeshProUGUI tmp = dialogueBox.GetComponent<TextMeshProUGUI>();
        tmp.text = dialogueText;
        yield return new WaitForSeconds(3f);
        tmp.text = null;
        dialogueBox.SetActive(false);
        isInteracting = !isInteracting;
        yield return null;
    }
}
