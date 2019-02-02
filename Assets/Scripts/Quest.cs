using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest", order = 1)]
public class Quest : ScriptableObject
{
    public string QuestName;
    public string dialogueGetQuest;
    public string dialogueDuringQuest;
    public string dialogueFinishQuest;
    public ItemData item;
    public int itemCount;
    public bool canRepeat;
    public bool isComplete;
}