using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public class shopItemsScript : MonoBehaviour, ISelectHandler
{
    // Start is called before the first frame update
    [SerializeField] Item[] items;
    int i = 0;
    string checkerString;
    GameObject shopPanel_1, shopPanel_2, shopPanel_3, shopPanel_4, buyButtonObject, resetButtonObject;
    TextMeshProUGUI rerollDisplay;
    GameObject[] panels;
    private int rerollCounter = 3;
    int[] index;
    Button ass;
    void Start()
    {
        resetButtonObject = transform.Find("Reset").gameObject;
        rerollDisplay = resetButtonObject.GetComponentInChildren<TextMeshProUGUI>();

        buyButtonObject = transform.Find("Buy").gameObject;
        buyButtonObject.SetActive(false);

        shopPanel_1 = transform.Find("shopPanel_1").gameObject;
        shopPanel_2 = transform.Find("shopPanel_2").gameObject;
        shopPanel_3 = transform.Find("shopPanel_3").gameObject;
        shopPanel_4 = transform.Find("shopPanel_4").gameObject;

        panels = new GameObject[] { shopPanel_1, shopPanel_2, shopPanel_3, shopPanel_4 };
        index = new int[items.Length];
    }
    private void FixedUpdate()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            buyButtonObject.SetActive(false);
        } 
    }
    public void Reroll()
    {
        if (rerollCounter != 0)
        {
            rerollCounter--;
            updatePanel();
            rerollDisplay.text = ("Reroll " + rerollCounter + "/3").ToString();
        }
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
    public void purchaseFromPanel()
    {
        if(EventSystem.current.currentSelectedGameObject.name == checkerString)
        {
            purchase();
        }    
    }
    public void purchase()
    {
        
        shopScript ass = this.transform.parent.GetComponent<shopScript>();          // change price
        if (ass.myInt >= items[index[i]].price)
        {
            ass.myInt = ass.myInt - items[index[i]].price;
            panels[i].SetActive(false);

            // give item to player
        }
        

        
    }
    public void OnSelect(BaseEventData eventData)
    {

        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "shopPanel_1":
                StartCoroutine(ddess("shopPanel_1"));
                i = 0;
                break;
            case "shopPanel_2":
                StartCoroutine(ddess("shopPanel_2"));
                i = 1;
                break;
            case "shopPanel_3":
                StartCoroutine(ddess("shopPanel_3"));
                i = 2;
                break;
            case "shopPanel_4":
                StartCoroutine(ddess("shopPanel_4"));
                i = 3;
                break;
        }
        
        buyButtonObject.SetActive(true);
    }
    IEnumerator ddess(string panel)
    {
        yield return new WaitForSeconds(.3f);
        checkerString = panel;
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
