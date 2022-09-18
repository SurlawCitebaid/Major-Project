using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
public class Item : ScriptableObject {
    public int item_id;
    public new string name;
    public int price;
    public string description;
    public Sprite sprite;
}
