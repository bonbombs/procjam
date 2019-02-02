using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRandomAnimation : MonoBehaviour {

    private float timer;

	// Use this for initialization
	void Start () {
        GetComponent<Animator>().speed = 0;
        timer = UnityEngine.Random.Range(0, 10);
    }

    void Update ()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            GetComponent<Animator>().speed = 1;
        }
    }
}
