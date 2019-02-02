using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour {
    [SerializeField]
    Material GrabbableMaterial;
    [SerializeField]
    Material DefaultMaterial;
    [SerializeField]
    SpriteRenderer mySprite;

    public ItemData data;

    private float t;

    public void Grab()
    {
        Animator anim = GetComponentInChildren<Animator>();
        anim.speed = 0;
        // lerp to bag and/or play sound
        transform.SetParent(InventoryManager.Instance.PackIcon);
        StartCoroutine(LerpTo());
        InventoryManager.Instance.AddItem(data);
    }

    IEnumerator LerpTo()
    {
        float lerpTime = 0f;
        Vector3 startPos = transform.position;
        Vector3 targetPos = InventoryManager.Instance.PackIcon.position;
        while (lerpTime < 1.0f) {
            lerpTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, targetPos, lerpTime / 10f);
            yield return null;
        }
        //Destroy(gameObject, 1.25f);
    }

    void Start()
    {
        t = 0;
    }

    public void SetData(ItemData newData)
    {
        data = newData;
        mySprite.sprite = data.ItemSprite;
    }

    void Update()
    {
        if (mySprite.material == GrabbableMaterial)
        {
            t += Time.deltaTime;
            Color c = mySprite.material.color;
            c.a = Mathf.Sin(t % Mathf.PI) + 50;
            mySprite.material.color = c;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            mySprite.material = GrabbableMaterial;
            StartCoroutine(DialogueController.Instance.DisplayItemName(data));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            mySprite.material = DefaultMaterial;
            StartCoroutine(DialogueController.Instance.DisplayCharacterName(""));
        }
    }
}
