using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSprites : MonoBehaviour
{
    public static ItemSprites instance;

    private void Awake()
    {
        instance = this;
    }
    public Sprite Pill;
    public Sprite WorryDoll;
}
