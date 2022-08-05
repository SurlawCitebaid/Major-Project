using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "ScriptableObjects/Consumable")]
public class Consumable : ScriptableObject {
    public int value;
    public Color32 colour;
    public new string name;
}
