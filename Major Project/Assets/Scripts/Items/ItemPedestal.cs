using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPedestal : MonoBehaviour {
    [SerializeField] private Item[] item_list;
    private Item item_scriptableObject;

    private SpriteRenderer sprite_renderer;

    [System.Obsolete]
    private void Awake() {
        sprite_renderer = transform.FindChild("Item").GetComponent<SpriteRenderer>();
    }

    private void Start() {
        item_scriptableObject = item_list[Random.Range(0, item_list.Length)];
        sprite_renderer.sprite = item_scriptableObject.sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Inventory inventory = collision.gameObject.GetComponent<Inventory>();
        if (inventory != null)
            inventory.AddItem(item_scriptableObject);
    }
}
