using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "DialogueData", order = 1)]
public class DialogueData : ScriptableObject
{
    public string[] dialogues;
}