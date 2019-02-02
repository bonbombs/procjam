using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FriendData", menuName = "FriendData", order = 1)]
public class FriendData : ScriptableObject
{
    public string Name;
    public string GreetingDialogue;
    public DialogueData DialogueData;
    public Quest[] QuestData;
}
