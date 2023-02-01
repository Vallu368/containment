using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMind : MonoBehaviour
{

    public int currentAffection = 0;
    public int medAffection = 5;
    public int highAffection = 10;

    [Header("Functional Options")]
    public bool canTalk;

    [Header("Talking Parameters")]
    public bool likesTalking;
    public bool alreadyTalked;

    [Header("Dialogue Lines For Talking")]
    public List<string> lowAffectionLines = new List<string>();
    public List<string> medAffectionLines = new List<string>();
    public List<string> highAffectionLines = new List<string>();

    [HideInInspector] public string lineToSend;

    public void IncreaseAffection()
    {
        currentAffection++;
    }
    public void CheckDialogueText() //changes dialogue that will go out to the dialogue box
    {
        if (!alreadyTalked)
        {
            if (currentAffection < medAffection)
            {
                lineToSend = lowAffectionLines[Random.Range(0, lowAffectionLines.Count)];
            }
            if (currentAffection > medAffection && currentAffection < highAffection)
            {
                lineToSend = medAffectionLines[Random.Range(0, medAffectionLines.Count)];
            }
            if (currentAffection > highAffection)
            {
                lineToSend = highAffectionLines[Random.Range(0, highAffectionLines.Count)];

            }
        }
    }
}
