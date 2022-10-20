using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject {
    public int item_id;
    public new string name;
    public int price;
    public string description;

    public Sprite GetSprite()
    {
        switch(item_id)
        {
            default:
            case 0: return ItemSprites.instance.WorryDoll;
            case 1: return ItemSprites.instance.Pill;
            case 2: return ItemSprites.instance.HealthPot;
            case 3: return ItemSprites.instance.MagnifyingGlass;
            case 4: return ItemSprites.instance.Feather;
            case 5: return ItemSprites.instance.Spring;
            case 6: return ItemSprites.instance.Battery;
            case 7: return ItemSprites.instance.WillOWisp;

        }
    }
}
