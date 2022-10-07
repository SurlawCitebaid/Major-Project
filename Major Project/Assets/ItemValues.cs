using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemValues
{
    private Item currentItem;
    private int amount = 1;

    public ItemValues(Item item)
    {
        currentItem = item;
    }
    public int GetItemID()
    {
        return currentItem.item_id;
    }
    public Item GetItem()
    {
        return currentItem;
    }
    public Sprite GetSprite()
    {
        return currentItem.GetSprite();
    }
    public int GetAmount()
    {
        return amount;
    }
    public void SetAmount(int i)
    {
        amount = i;
    }


}
