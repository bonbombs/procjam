using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeredithAnimations : MonoBehaviour {

    public Animator anim;
    [SerializeField]
    public Quest[] quests;

    // Use this for initialization
    void Start () {
        InvokeRepeating("RandomizeIdle", 5f, 5f);
    }

    void RandomizeIdle()
    {
        if (DialogueController.Instance.isInteracting)
        {
            anim.SetBool("hasKnittingKit", false);
            anim.SetBool("hasInstrument", false);
        }
        else
        {
            if (Random.Range(0, 10) % 2 == 0)
                anim.SetBool("hasKnittingKit", QuestManager.Instance.IsQuestDone(quests[0]));
            else
                anim.SetBool("hasInstrument", QuestManager.Instance.IsQuestDone(quests[1]));
        }
    }
}
