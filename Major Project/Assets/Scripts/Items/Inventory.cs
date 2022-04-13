using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    private List<Item> inventory = new List<Item>();

    public void AddItem(Item item) {
        inventory.Add(item);
    }
}
