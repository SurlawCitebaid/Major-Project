using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPedestal : MonoBehaviour {
    [SerializeField] private Item[] item_list;
    private Item item_scriptableObject;
    private GameObject itemOnPedestal;
    private SpriteRenderer sprite_renderer;
    private bool itemGot = false;
    public Item item;
    private void Awake() {
        itemOnPedestal = transform.Find("Item").gameObject;
        sprite_renderer = itemOnPedestal.GetComponent<SpriteRenderer>();
    }

    private void Start() {
        item_scriptableObject = item_list[Random.Range(0, item_list.Length)];
        sprite_renderer.sprite = item_scriptableObject.GetSprite();
        item = item_scriptableObject;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(!itemGot && collision.gameObject.CompareTag("Player"))
        {
            Inventory.instance.AddItem(item);
            Destroy(itemOnPedestal);
            itemGot = true;
        }
    }
}
