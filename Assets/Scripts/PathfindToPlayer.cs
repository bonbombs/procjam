using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindToPlayer : MonoBehaviour {

    [SerializeField]
    public SpriteRenderer mySprite;
    [SerializeField]
    public Transform targetFollow;
    [SerializeField]
    public Vector3 offsetFollow;

    public float speed = 2.0f;
    public float breakAwayDistance;

    [SerializeField]
    public Animator anim;
    private bool shouldMove;
    private bool shouldFollow;
    private Vector3 targetDestination;
    private Rigidbody rb;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        shouldFollow = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (shouldFollow && targetFollow != null)
        {
            if (Vector3.Distance(transform.position, targetFollow.position) > breakAwayDistance)
            {
                shouldFollow = false;
                return;
            }

            if (shouldFollow && Vector3.Distance(transform.position, targetFollow.position) > 2)
            {
                targetDestination = targetFollow.position + offsetFollow;
                shouldMove = true;
            }
            else
            {
                shouldMove = false;
            }
        }
        else if (targetFollow != null)
        {
            if (Vector3.Distance(transform.position, targetFollow.position) > 2 && shouldMove)
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
    }

    void LateUpdate()
    {
        if (shouldMove && shouldFollow)
        {
            transform.position = Vector3.Lerp(transform.position, targetDestination, Mathf.SmoothStep(0, 1, Time.deltaTime) * speed);
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            targetFollow = other.transform;
            shouldFollow = true;
            shouldMove = true;
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

    private void OnCollisionEnter(Collision collision)
    {
        shouldMove = false;
        shouldFollow = false;
    }

}
