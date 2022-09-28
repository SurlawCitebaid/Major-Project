using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class shopScript : MonoBehaviour
{
    GameObject currencyObject, shopButtonObject, loadoutButtonObject, shopItemsPanel, resetObject;
    shopItemsScript items;
    Button loadoutButton, shopButton;
    TextMeshProUGUI shopText, loadOutButtonText, currency;

    public int myInt = 100;
    private void Start()
    {

        shopItemsPanel = transform.Find("ItemsPanel").gameObject;                                           //shop items panels
        items = shopItemsPanel.GetComponent<shopItemsScript>();

        shopButtonObject = transform.Find("ShopButton").gameObject;
        shopButton = shopButtonObject.GetComponent<Button>();                                               // shop section
        shopText = shopButtonObject.GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>();

        currencyObject = transform.Find("Currency").gameObject;                                             //currency section
        currency = currencyObject.GetComponent<TextMeshProUGUI>();

        loadoutButtonObject = transform.Find("Loadout").gameObject;
        loadoutButton = loadoutButtonObject.GetComponent<Button>();                                         // loadout section
        loadOutButtonText = loadoutButtonObject.GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>();

        toggleShopPanel(true);                      // sets default panel
        items.UpdatePanel();                        // sets current stage items
    }

   
    private void Update()
    {
        
        currency.text = myInt.ToString();

    }

    public void toggleShopPanel(bool input)
    {
        if(input)
        {
            shopItemsPanel.SetActive(true);                     
                                //sets shop items and buy buttons to visible

            shopText.fontStyle = FontStyles.Bold;
            shopButton.GetComponent<Image>().enabled = true;
            
            loadOutButtonText.fontStyle = FontStyles.Normal;
            loadoutButton.GetComponent<Image>().enabled = false;

        } else
        {
            shopItemsPanel.SetActive(false);
                               //sets shop items and buy buttons to invisible
            resetObject.SetActive(false);

            shopText.fontStyle = FontStyles.Normal;
            shopButton.GetComponent<Image>().enabled = false;

            loadOutButtonText.fontStyle = FontStyles.Bold;
            loadoutButton.GetComponent<Image>().enabled = true;
        } 
    }
}
