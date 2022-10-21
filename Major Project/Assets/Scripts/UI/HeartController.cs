using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartController : MonoBehaviour {
    [SerializeField] Sprite[] heartSprites;

    private int value = 2;

    private Image image;

    private void Awake() {
        image = this.gameObject.GetComponent<Image>();
    }

    public void SetValue(int val) {
        value = val;
        image.sprite = heartSprites[value];
    }
}
