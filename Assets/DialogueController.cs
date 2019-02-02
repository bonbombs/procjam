using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : BaseManager<DialogueController> {

    public DialogueData CurrentDialogue;
    public Interactable CurrentInteractable;

    [SerializeField]
    public GameObject DialoguePanel;
    [SerializeField]
    public Text DialogueName;
    [SerializeField]
    public Text DialogueText;

    [SerializeField]
    public GameObject BannerPrompt;
    public Text BannerPromptTitle;
    public Text BannerPromptSubtitle;

    [SerializeField]
    Text StatusText;

    public bool isInteracting;

    private RectTransform rectTransformCanvas;
    private Queue<Dialogue> dialogueQueue;
    private bool shouldShowBannerPrompt;
    private bool isLast;
    private Vector3 displayPos;

    // Use this for initialization
    void Start () {
        isInteracting = false;
        shouldShowBannerPrompt = false;
        rectTransformCanvas = FindObjectOfType<Canvas>().GetComponent<RectTransform>();
        dialogueQueue = new Queue<Dialogue>();
        displayPos = StatusText.transform.parent.GetComponent<RectTransform>().localPosition;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Interact") && isInteracting)
        {
            if (dialogueQueue.Count == 0)
                StartCoroutine(Hide());
            else
                ShowNext();
        }
	}

    private void Display(string name, string text)
    {
        isInteracting = true;
        DialogueName.text = name;
        DialogueText.text = text;
        if (!DialoguePanel.activeInHierarchy)
            DialoguePanel.SetActive(true);
    }

    public void Display(string name, DialogueData data, Interactable interactable)
    {
        if (CurrentInteractable == null) CurrentInteractable = interactable;
        else return;
        int RandomIdx = Random.Range(0, data.dialogues.Length);
        isLast = true;
        Display(name, data.dialogues[RandomIdx]);
    }

    public void Display(string name, Quest quest, Interactable interactable)
    {
        if (CurrentInteractable == null) CurrentInteractable = interactable;
        else return;
        if (QuestManager.Instance.QuestInProgress(quest))
        {
            if (InventoryManager.Instance.Items.Contains(quest.item))
            {
                isLast = false;
                QuestManager.Instance.CompleteQuest(name, quest);
                Dialogue dialogue = new Dialogue(name, quest.dialogueFinishQuest, "FinishQuest", quest);
                dialogueQueue.Enqueue(dialogue);
                InventoryManager.Instance.RemoveItem(quest.item, quest.itemCount);
            }
            else
                isLast = true;
            Display(name, quest.dialogueDuringQuest);
        }
        else
        {
            shouldShowBannerPrompt = true;
            BannerPromptTitle.text = "New Quest!";
            BannerPromptSubtitle.text = quest.QuestName;
            QuestManager.Instance.AddQuest(name, quest);
            isLast = true;
            Display(name, quest.dialogueGetQuest);
        }
    }

    IEnumerator DisplayBannerPrompt()
    {
        RectTransform rect = BannerPrompt.GetComponent<RectTransform>();
        rect.localPosition = Vector2.right * -rectTransformCanvas.rect.width;
        BannerPrompt.SetActive(true);
        yield return ExtensionMethods.UILocalEaseLerp(rect, Vector3.zero, 1);
        rect.localPosition = Vector3.zero;
        shouldShowBannerPrompt = false;
    }

    IEnumerator HideBannerPrompt()
    {
        RectTransform rect = BannerPrompt.GetComponent<RectTransform>();
        yield return ExtensionMethods.UILocalEaseLerp(rect, Vector3.right * rectTransformCanvas.rect.width, 1);
        rect.localPosition = Vector2.right * rectTransformCanvas.rect.width;
        BannerPrompt.SetActive(false);
    }

    public void ShowNext()
    {
        Dialogue nextDialogue = dialogueQueue.Dequeue();
        if (nextDialogue.type == "FinishQuest")
        {
            shouldShowBannerPrompt = true;
            BannerPromptTitle.text = "Quest Complete!";
            BannerPromptSubtitle.text = nextDialogue.quest.QuestName;
        }
        Display(nextDialogue.name, nextDialogue.text);
        isLast = dialogueQueue.Count == 0;
        isInteracting = true;
    }

    public IEnumerator Hide()
    {
        if (shouldShowBannerPrompt)
        {
            yield return DisplayBannerPrompt();
            yield return new WaitForSeconds(3.0f);
            yield return HideBannerPrompt();
        }
        DialoguePanel.SetActive(false);
        isLast = false;
        Camera.main.GetComponent<InteractZoom>().StopInteract();
        yield return new WaitForSeconds(1.0f);
        FindObjectOfType<PlayerController>().Lock = false;
        CurrentInteractable = null;
        isInteracting = false;
    }

    public IEnumerator ShowStatusBar()
    {
        RectTransform rect = StatusText.transform.parent.GetComponent<RectTransform>();
        Vector2 target = displayPos;
        if (rect.position != (Vector3) target)
        {
            yield return ExtensionMethods.UILocalEaseLerp(rect, target, 1);
        }
    }

    public IEnumerator HideStatusBar()
    {
        RectTransform rect = StatusText.transform.parent.GetComponent<RectTransform>();
        Vector2 target = Vector2.up * -rectTransformCanvas.rect.height;
        if (rect.position != (Vector3) target)
        {
            yield return ExtensionMethods.UILocalEaseLerp(rect, target, 1);
        }
    }

    public IEnumerator DisplayItemName(ItemData item)
    {
        StatusText.text = item.ItemName + " - " + item.ItemDesc;
        yield return ShowStatusBar();
    }

    public IEnumerator DisplayCharacterName(string name)
    {
        StatusText.text = name;
        if (name == "")
            yield return HideStatusBar();
        else
            yield return ShowStatusBar();
    }

    public struct Dialogue
    {
        public string name;
        public string text;
        public string type;
        public Quest quest;

        public Dialogue(string newName, string newText, string newType, Quest newQuest)
        {
            name = newName;
            text = newText;
            type = newType;
            quest = newQuest;
        }
    }
}