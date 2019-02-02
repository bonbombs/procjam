using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractZoom : MonoBehaviour {

    public Vector3 offset;
    public float zoomDamp;

    private Bounds zoomBounds;
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 targetPos = new Vector3(zoomBounds.center.x, -1, zoomBounds.center.z) + offset;
        Vector3 currentPos = Camera.main.transform.position;
        float currentSize = Camera.main.orthographicSize;
        float targetSize = 0;

        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = zoomBounds.size.x / zoomBounds.size.y;

        if (screenRatio >= targetRatio)
        {
            targetSize = zoomBounds.size.y / 2;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            targetSize = zoomBounds.size.y / 2 * differenceInSize;
        }

        Camera.main.orthographicSize = Mathf.Lerp(currentSize, targetSize, 1 * Time.deltaTime);
        Camera.main.transform.position = Vector3.Lerp(currentPos, targetPos, 1 * Time.deltaTime);
    }

    public void StartInteract(Transform a, Transform b)
    {
        GetComponent<FollowPlayer>().enabled = false;
        enabled = true;
        zoomBounds = new Bounds();
        //zoomBounds.Encapsulate(a.position);
        //zoomBounds.Encapsulate(b.position);
        zoomBounds.size = Vector3.one * (Camera.main.orthographicSize * 0.75f);
        Vector3 midpoint = (a.position + b.position) / 2;
        zoomBounds.center = midpoint;
    }

    public void StopInteract()
    {
        GetComponent<FollowPlayer>().enabled = true;
        enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5F);
        Gizmos.DrawCube(zoomBounds.center, zoomBounds.size);
    }
}
