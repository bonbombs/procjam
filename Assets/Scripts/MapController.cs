using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class MapController : MonoBehaviour {

    [SerializeField]
    MapGenerator Generator;
    [SerializeField]
    Text StatusText;
    [SerializeField]
    GameObject PlayerUI;
    [SerializeField]
    GameObject PlayerCharacter;
    [SerializeField]
    Grabbable GrabbableReference;
    [SerializeField]
    public FriendController[] Friends;
    [SerializeField]
    public ItemData[] Items;

    public Vector3 cameraOffset;
    public PlayerController PC;
    

    private bool didSpawnPlayer;

	// Use this for initialization
	void Start () {
        didSpawnPlayer = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Reset"))
        {
            foreach (FriendController f in FindObjectsOfType<FriendController>())
                Destroy(f.gameObject);
            Destroy(PC.gameObject);
            FollowPlayer fp = Camera.main.GetComponent<FollowPlayer>();
            fp.shouldFit = true;
            fp.player = null;
            didSpawnPlayer = false;
            StatusText.text = "Building...";
            Generator.ResetGeneration();
        }

        if (Generator.isDone && !didSpawnPlayer)
        {
            didSpawnPlayer = true;
            
            
            Generator.GetComponent<LineRenderer>().enabled = false;

            StartCoroutine(InitPlayer());
            InitItems();
            PlayerUI.SetActive(true);
        }
	}

    IEnumerator InitPlayer()
    {
        StatusText.text = "Done!";
        yield return new WaitForSeconds(2.0f);
        PC = Instantiate(PlayerCharacter).GetComponent<PlayerController>();
        PC.GetComponent<PlayerController>().map = Generator;
        List<int> usedPositions = new List<int>();
        foreach (FriendController FriendCharacter in Friends)
        {
            int len = Generator.IslandStart.Length;
            FriendController FC = Instantiate(FriendCharacter).GetComponent<FriendController>();
            int randIdx = UnityEngine.Random.Range(0, len);
            
            while (usedPositions.Contains(randIdx))
            {
                randIdx = UnityEngine.Random.Range(0, len);
            }
            usedPositions.Add(randIdx);
            FC.transform.position = Generator.IslandStart[randIdx];
        }
        FollowPlayer fp = Camera.main.GetComponent<FollowPlayer>();
        fp.shouldFit = false;
        fp.player = PC.transform;
        fp.UpdatePosition(new Vector3(-8.05f, 14.53f, -7.85f));
    }

    void InitItems()
    {
        foreach (ItemData Item in Items)
        {
            int len = Generator.IslandStart.Length;
            Grabbable grabbable = Instantiate(GrabbableReference).GetComponent<Grabbable>();
            grabbable.SetData(Item);
            grabbable.transform.position = Generator.GetRandomLandTile();
        }
    }
}
