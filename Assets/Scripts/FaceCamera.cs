using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {

    [SerializeField]
    Transform target;

	// Use this for initialization
	void Update () {
        target.forward = -Camera.main.transform.forward;
        //target.rotation = Quaternion.Euler(new Vector3(target.rotation.x, target.rotation.y, target.rotation.z));
    }
}
