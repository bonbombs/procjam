using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePanel : MonoBehaviour {

    public RectTransform rect;

	// Use this for initialization
	void Start () {
        StartCoroutine(SlideDown());
	}

    IEnumerator SlideDown()
    {
        Vector3 targetRect = rect.position;
        Vector3 startRect = rect.position + Vector3.up * rect.sizeDelta.y;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            rect.position = Vector3.Lerp(startRect, targetRect, t);
            yield return null;
        }
        rect.position = targetRect;
    }
}
