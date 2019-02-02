using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour {

    public Transform GrabTarget;

    private bool canGrab;
    private Grabbable grabbable;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Interact") && canGrab)
        {
            grabbable.transform.position = GrabTarget.position;
            grabbable.transform.SetParent(GrabTarget, true);
            grabbable.Grab();
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Grabbable")
        {
            canGrab = true;
            grabbable = other.GetComponent<Grabbable>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Grabbable")
        {
            canGrab = false;
            grabbable = null;
        }
    }
}
