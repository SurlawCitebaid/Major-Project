using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPedestal : MonoBehaviour {
    [SerializeField] private Item[] item_list;
    [SerializeField] private Sprite OpenChest;
    private Item item_scriptableObject;
    private GameObject itemOnPedestal, interact;
    private SpriteRenderer itemSprite ,chestSprite;
    private bool itemGot = false, inTrigger = false, itemOut = false, open = false;
    public Item item;
    private void Awake() {
        itemOnPedestal = transform.Find("Item").gameObject;
        interact = transform.Find("interact").gameObject;
        itemSprite = itemOnPedestal.GetComponent<SpriteRenderer>();
        chestSprite = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if(!itemGot)
        {
            if (!open && inTrigger && Input.GetKeyDown(KeyCode.E))
            {
                open = true;
                StartCoroutine(genItem());

            }
            if (itemOut && Input.GetKeyDown(KeyCode.E))
            {
                interact.SetActive(false);
                Inventory.instance.AddItem(item);
                Destroy(itemOnPedestal);
                itemGot = true;
            }
        }
        
    }
    IEnumerator genItem()
    {
        chestSprite.sprite = OpenChest;
        item_scriptableObject = item_list[Random.Range(0, item_list.Length)];
        itemSprite.sprite = item_scriptableObject.GetSprite();
        item = item_scriptableObject;
        yield return new WaitForSeconds(1);
        itemOut = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(!itemGot)
            {
                interact.SetActive(true);
            }       
            inTrigger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!itemGot)
            {
                interact.SetActive(false);
            }
            inTrigger = false;
        }
    }
}
