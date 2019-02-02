using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    public enum InteractableType
    {
        Character,
        Item
    }
    [SerializeField]
    public InteractableType type;
    [SerializeField]
    Animator anim;
    [SerializeField]
    public FriendData friendData;

    private bool inVicinity;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!DialogueController.Instance.isInteracting && inVicinity && Input.GetButtonDown("Interact"))
        {
            switch (type){

            }
            Interact();
        }
        else
        {
            anim.SetBool("isInteracting", false);
        }
    }

    public void Interact()
    {
        anim.SetBool("isInteracting", true);
        // Get next available quest
        Quest nextQuest = null;
        for (int i = 0; i < friendData.QuestData.Length; i++)
        {
            if (QuestManager.Instance.IsQuestDone(friendData.QuestData[i])) continue;
            nextQuest = friendData.QuestData[i];
        }
        // Camera zoooooom
        Camera.main.GetComponent<InteractZoom>().StartInteract(transform, FindObjectOfType<PlayerController>().transform);
        FindObjectOfType<PlayerController>().Lock = true;
        // start dialogue
        if (nextQuest != null)
        {
            DialogueController.Instance.Display(friendData.Name, nextQuest, this);
        }
        else
        {
            DialogueController.Instance.Display(friendData.Name, friendData.DialogueData, this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            inVicinity = true;
            
            StartCoroutine(DialogueController.Instance.DisplayCharacterName(friendData.Name));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inVicinity = false;
        }
    }
}
