using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public static Inventory instance;
    [SerializeField] public List<ItemValues> inventory;

    public Inventory()
    {
        inventory = new List<ItemValues>();
    }


    private void Awake()
    {
        instance = this;
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

