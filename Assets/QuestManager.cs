using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : BaseManager<QuestManager> {

    public Dictionary<Quest, bool> QuestLog;
    public Dictionary<string, Quest> OngoingQuests;

    private void Start()
    {
        QuestLog = new Dictionary<Quest, bool>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Reset"))
        {
            QuestLog = new Dictionary<Quest, bool>();
        }
    }

    public bool QuestInProgress(Quest quest)
    {
        return QuestLog.ContainsKey(quest);
    }

    public bool IsQuestDone(Quest quest)
    {
        return QuestLog.ContainsKey(quest) && QuestLog[quest];
    }

	public bool AddQuest(string name, Quest newQuest)
    {
        if (QuestLog.ContainsKey(newQuest)) return false;
        else
        {
            QuestLog.Add(newQuest, false);
            OngoingQuests.Add(name, newQuest);
            return true;
        }
    }

    public bool CompleteQuest(string name, Quest quest)
    {
        if (QuestLog.ContainsKey(quest))
        {
            // If Player meets requirements...
            QuestLog[quest] = true;
            OngoingQuests.Remove(name);
            return true;
        }

        return false;
    }
}