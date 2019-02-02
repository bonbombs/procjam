using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendController : MonoBehaviour {

    [SerializeField]
    public SpriteRenderer mySprite;
    [SerializeField]
    public Animator anim;
    private bool inVicinity;
    private Transform targetFollow;

    [SerializeField]
    public FriendData friendData;

	// Use this for initialization
	void Start () {
        
	}

    // Update is called once per frame
    void Update () {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            targetFollow = other.transform;
            StartCoroutine(DialogueController.Instance.DisplayCharacterName(friendData.Name));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            SpriteRenderer targetSprite = targetFollow.GetComponent<PlayerController>().spriteRenderer;
            if (mySprite.transform.position.x > targetSprite.transform.position.x)
            {
                mySprite.flipX = true;
            }
            else
            {
                mySprite.flipX = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(DialogueController.Instance.DisplayCharacterName(""));
        }
    }
}
