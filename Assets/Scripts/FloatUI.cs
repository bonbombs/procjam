using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatUI : MonoBehaviour {

    public float amplitude = 0.1f;

	// Update is called once per frame
	void Update () {
        Vector3 yPos = Vector3.up * Mathf.Sin(Time.fixedTime * 2) * amplitude;
        transform.position += yPos;
	}
}
