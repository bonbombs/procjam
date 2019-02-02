using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : BaseManager<InventoryManager> {

    [SerializeField]
    Text BadgeCount;
    [SerializeField]
    public Transform PackIcon;

    public List<ItemData> Items { get; private set; }

	// Use this for initialization
	void Start () {
        Items = new List<ItemData>();
	}

    void Update()
    {
        if (Input.GetButtonDown("Reset"))
        {
            Items = new List<ItemData>();
            BadgeCount.text = Items.Count.ToString();
        }
    }

    public void AddItem(ItemData newItem, int addCount = 1)
    {
        for (int i = 0; i < addCount; i++)
        {
            Items.Add(newItem);
        }

    }

    public void RemoveItem(ItemData item, int removeCount = 1)
    {
        for (int i = 0; i < removeCount; i++)
        {
            Items.Remove(item);
        }
        BadgeCount.text = Items.Count.ToString();
    }
}
