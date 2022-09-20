using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class shopItemsScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Item[] items;
    GameObject shopPanel_1, shopPanel_2, shopPanel_3, shopPanel_4;
    GameObject[] panels;
    int[] index;
    void Start()
    {

        shopPanel_1 = transform.Find("shopPanel_1").gameObject;
        shopPanel_2 = transform.Find("shopPanel_2").gameObject;
        shopPanel_3 = transform.Find("shopPanel_3").gameObject;
        shopPanel_4 = transform.Find("shopPanel_4").gameObject;

        panels = new GameObject[] { shopPanel_1, shopPanel_2, shopPanel_3, shopPanel_4 };
        index = new int[items.Length];
    }
    public void updatePanel()
    {
        int size;
        for (int i = 0; i < items.Length; i++)
        {
            index[i] = i;                //get indexes of items
        }
        
        Shuffle(index);                  //shuffle indexes of items

        if(index.Length > 4)
        {
            size = panels.Length;        
        } else
        {
            size = index.Length;
        }

        for (int i = 0; i < size; i++)
        {
            if (panels[i].activeSelf)
            {
                TextMeshProUGUI ass = panels[i].GetComponentInChildren<TextMeshProUGUI>();
                Image dd = panels[i].transform.Find("Image").GetComponent<Image>();
                ass.text = items[index[i]].name + "\n " + items[index[i]].description;
                dd.sprite = items[index[i]].sprite;
            }
        }
    }
    public void purchase(int i)
    {
        shopScript ass = this.transform.parent.GetComponent<shopScript>();
        if (ass.myInt >= items[index[i]].price)            
        {
            ass.myInt = ass.myInt - items[index[i]].price;
            panels[i].SetActive(false);

            // give item to player
        }
    }
    void Shuffle(int[] indexes)
    {
        for (int i = indexes.Length - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i);
            int temp = indexes[i];
            indexes[i] = indexes[rnd];
            indexes[rnd] = temp;
        }
    }
}
