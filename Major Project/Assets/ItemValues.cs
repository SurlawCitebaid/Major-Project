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
    public int getItemID()
    {
        return currentItem.item_id;
    }
    public Item getItem()
    {
        return currentItem;
    }
    public Sprite getSprite()
    {
        return currentItem.GetSprite();
    }
    public int getAmount()
    {
        return amount;
    }
    public void SetAmount(int i)
    {
        amount = i;
    }


}
