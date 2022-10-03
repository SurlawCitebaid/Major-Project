using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public static Inventory instance;
    [SerializeField] private List<Item> inventory = new List<Item>();

    private void Awake()
    {
        instance = this;
    }
    public void AddItem(Item item) {
        inventory.Add(item);
    }
}
