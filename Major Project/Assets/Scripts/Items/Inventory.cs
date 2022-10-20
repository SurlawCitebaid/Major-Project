using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour {
    public static Inventory instance;
    public GameObject itemDisplay;
    public List<ItemValues> inventory;
    private int invSize;

    public Inventory()
    {
        inventory = new List<ItemValues>();
    }
    private void Awake()
    {
        instance = this;
        invSize = inventory.Count;
    }
    private void Update()
    {
        if(invSize != inventory.Count)
        {
            ShowItem(inventory[inventory.Count-1]);
            invSize = inventory.Count;
        }
    }
    private void ShowItem(ItemValues item)
    {
        GameObject panel = Instantiate(itemDisplay, Camera.main.transform);
        SpriteRenderer picture = panel.transform.Find("Picture").GetComponent<SpriteRenderer>();
        picture.sprite = item.GetSprite();
        TextMeshPro textpanel = panel.transform.Find("Text").GetComponent<TextMeshPro>();
        textpanel.text = item.GetName() + "\n" + item.GetDescription();
        Destroy(panel, 3f);
    }
    public void AddItem(Item item) {
        foreach (ItemValues ass in inventory)
        {
            if(ass.GetItemID() == item.item_id)
            {
                ass.SetAmount(ass.GetAmount() + 1);
                return;
            }
        }
        inventory.Add(new ItemValues(item));
    }
}

