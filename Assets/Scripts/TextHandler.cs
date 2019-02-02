using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Renderer parentRenderer = transform.parent.GetComponent<Renderer>();
        Renderer myRenderer = GetComponent<Renderer>();
        myRenderer.sortingLayerID = parentRenderer.sortingLayerID;
        myRenderer.sortingOrder = parentRenderer.sortingOrder;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
