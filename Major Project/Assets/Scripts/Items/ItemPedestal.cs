using System.Collections;
using UnityEngine;
using TMPro;

public class ItemPedestal : MonoBehaviour {
    [SerializeField] private GameObject itemOnPedestal, interact, reroll, Text;
    [SerializeField] private Item[] item_list;
    [SerializeField] private Sprite OpenChest;
    private SpriteRenderer itemSprite, chestSprite;
    private TMP_Text PriceText;
    private Item item_scriptableObject;
    private PlayerController currencyObject;
    private float xPos;
    private bool itemGot = false, inTrigger = false, itemOut = false;
    public int Price;
    public Item item;
    private void Start() {
        currencyObject = GameObject.Find("Player 3").GetComponent<PlayerController>();
        PriceText = Text.GetComponent<TMP_Text>();
        itemSprite = itemOnPedestal.GetComponent<SpriteRenderer>();
        chestSprite = GetComponent<SpriteRenderer>();
        xPos = transform.position.x -1;
        Price = 10;
    }
    private void Update()
    {
        if (inTrigger && !itemGot)
        {
            if(!itemOut)
            {
                interact.SetActive(true);                                                       
            } else
            {
                interact.transform.position = new Vector2(xPos , transform.position.y+2);
                interact.SetActive(true);
                reroll.SetActive(true);
            }
            if (!itemOut)
            {
                if (inTrigger && Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(genItem());
                }
            }
            else
            {
                
                if (itemOut && inTrigger && Input.GetKeyDown(KeyCode.E))
                {
                    interact.SetActive(false);
                    reroll.SetActive(false);
                    Inventory.instance.AddItem(item);
                    Destroy(itemOnPedestal);
                    itemGot = true;
                }
                else if (itemOut && inTrigger && Input.GetKeyDown(KeyCode.R))
                {
                    
                    if (currencyObject.getCurrency() > Price)
                    {


                        currencyObject.SetCurrency( currencyObject.currency -  Price);
                        
                        item_scriptableObject = item_list[Random.Range(0, item_list.Length)];
                            itemSprite.sprite = item_scriptableObject.GetSprite();
                            item = item_scriptableObject;
                        Price *= 2;
                        PriceText.text = Price.ToString();



                    }
                        
                }
            }
        } else if(!inTrigger || !itemGot)
        {
            interact.SetActive(false);
            reroll.SetActive(false);
        }
    }
    IEnumerator genItem()
    {
        itemOut = true;
        PriceText.text = Price.ToString();
        chestSprite.sprite = OpenChest;
        item_scriptableObject = item_list[Random.Range(0, item_list.Length)];
        itemSprite.sprite = item_scriptableObject.GetSprite();
        item = item_scriptableObject;
        yield return new WaitForSeconds(1);

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Player"))
        {
            inTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {           
            inTrigger = false;
        }
    }
}
