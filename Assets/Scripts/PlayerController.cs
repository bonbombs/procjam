using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public MapGenerator map;
    public GameObject accessory;
    public GameObject boat;
    public float speed = 2.0f;
    public bool Lock { get; set; }

    public SpriteRenderer spriteRenderer;
    public BoxCollider box;
    private bool mFacing;
    private Vector3 mOriginalPosition;
    private Vector3 mProjectedPosition;
    private bool mIsOnSea;
    private bool mIsOnBoard;

	// Use this for initialization
	void Start () {
        int len = map.IslandStart.Length;
        int randIdx = UnityEngine.Random.Range(0, len);
        transform.position = map.IslandStart[randIdx];
        mIsOnSea = false;
        mIsOnBoard = false;
        BoxCollider[] boxes = GetComponents<BoxCollider>();
        foreach (BoxCollider b in boxes)
        {
            if (!b.isTrigger) box = b;
        }
        Lock = false;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Lock) return;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (horizontal != 0)
            mFacing = (horizontal > 0);
        if ((horizontal != 0 || vertical != 0) && !boat.activeInHierarchy)
            spriteRenderer.GetComponent<Animator>().SetBool("isWalking", true);
        else
            spriteRenderer.GetComponent<Animator>().SetBool("isWalking", false);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        spriteRenderer.flipX = mFacing;
        accessory.GetComponent<SpriteRenderer>().flipX = mFacing;
        mOriginalPosition = transform.position;
        mProjectedPosition = Quaternion.Euler(0, 45f, 0) * new Vector3(horizontal, 0, vertical) * speed;
        transform.position += mProjectedPosition;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Boarding"))
        {
            box.enabled = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Boarding"))
        {
            mIsOnBoard = true;
            boat.SetActive(false);
            accessory.SetActive(true);
            box.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Boarding");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Sea" && mIsOnBoard)
        {
            mIsOnSea = true;
            mIsOnBoard = false;
            boat.SetActive(true);
           // box.enabled = true;
            gameObject.layer = LayerMask.NameToLayer("Water");
        }
        else if (collision.collider.tag == "Land" && mIsOnBoard)
        {
            mIsOnSea = false;
            mIsOnBoard = false;
            accessory.SetActive(false);
            box.enabled = true;
            gameObject.layer = LayerMask.NameToLayer("Land");
        }
    }
}
