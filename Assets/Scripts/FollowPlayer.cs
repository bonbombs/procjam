using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public MapGenerator generator;
    public Transform player;
    public Vector3 offset;
    public bool shouldFit;
    public float zoomInSize;
    public float zoomOutSize;
    public float zoomOutDistance;
    public float damping;
    public float zoomDamp;

    private bool isZoom;
    private float targetOrthoSize;
    private Vector3 targetVector;

	// Use this for initialization
	void Start () {
        isZoom = false;
        shouldFit = true;
        targetOrthoSize = Camera.main.orthographicSize;
	}

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isZoom)
            {
                targetOrthoSize = zoomOutSize;
                targetVector = transform.forward * -zoomOutDistance;
                isZoom = true;
            }
            else
            {
                targetOrthoSize = zoomInSize;
                targetVector = transform.forward / -zoomOutDistance;
                isZoom = false;
            }
        }
    }

    // Update is called once per frame
    void LateUpdate () {

        if (shouldFit) FollowAndFit();
        else Follow();

	}

    public void UpdatePosition(Vector3 newPos)
    {
        transform.position = newPos;
    }

    void Follow()
    {
        Vector3 zoomOffset = Vector3.zero;
        if (isZoom)
            zoomOffset = transform.forward * -5;
        Vector3 wantedOffset = player.position + offset + zoomOffset;
        Vector3 currentOffset = transform.position + targetVector;
        float currentSize = Camera.main.orthographicSize;
        currentOffset = Vector3.Lerp(currentOffset, wantedOffset, damping * Time.deltaTime);
        transform.position = currentOffset;
        Camera.main.orthographicSize = Mathf.Lerp(currentSize, targetOrthoSize, damping * Time.deltaTime);
    }

    void FollowAndFit()
    {
        Bounds targetBounds = generator.mapBounds;
        Vector3 targetPos = new Vector3(targetBounds.center.x, targetBounds.center.y, -1f) + (Vector3.up * offset.y);
        Vector3 currentPos = Camera.main.transform.position;
        float currentSize = Camera.main.orthographicSize;
        float targetSize = 0;

        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = targetBounds.size.x / targetBounds.size.y;

        if (screenRatio >= targetRatio)
        {
            targetSize = targetBounds.size.y / 2;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            targetSize = targetBounds.size.y / 2 * differenceInSize;
        }

        Camera.main.orthographicSize = Mathf.Lerp(currentSize, targetSize, 1 * Time.deltaTime);
        Camera.main.transform.position = Vector3.Lerp(currentPos, targetPos, 1 * Time.deltaTime);
    }
}
