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
    TextMeshProUGUI shopText, loadOutButtonText, currency, rerollDisplay;
    private int rerollCounter = 3;
    public int myInt = 100;
    private void Start()
    {
        resetObject = transform.Find("Reset").gameObject;
        rerollDisplay = resetObject.GetComponentInChildren<TextMeshProUGUI>();

        shopItemsPanel = transform.Find("ItemsPanel").gameObject;                                           //shop items panels
        items = shopItemsPanel.GetComponent<shopItemsScript>();

        currencyObject = transform.Find("Currency").gameObject;                                             //currency section
        currency = currencyObject.GetComponent<TextMeshProUGUI>();

        shopButtonObject = transform.Find("ShopButton").gameObject;
        shopButton = shopButtonObject.GetComponent<Button>();                                               // shop section
        shopText = shopButtonObject.GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>();

        loadoutButtonObject = transform.Find("Loadout").gameObject;
        loadoutButton = loadoutButtonObject.GetComponent<Button>();                                         // loadout section
        loadOutButtonText = loadoutButtonObject.GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>();

        toggleShopPanel(true);                      // sets default panel
        items.updatePanel();                        // sets current stage items
    }
    public void Reroll()
    {
        if(rerollCounter != 0)
        {
            rerollCounter--;
            items.updatePanel();
            rerollDisplay.text= (rerollCounter + "/3").ToString();
        }
        
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
            
            shopText.fontStyle = FontStyles.Bold;
            shopButton.GetComponent<Image>().enabled = true;
            
            loadOutButtonText.fontStyle = FontStyles.Normal;
            loadoutButton.GetComponent<Image>().enabled = false;

        } else
        {
            shopItemsPanel.SetActive(false);

            shopText.fontStyle = FontStyles.Normal;
            shopButton.GetComponent<Image>().enabled = false;

            loadOutButtonText.fontStyle = FontStyles.Bold;
            loadoutButton.GetComponent<Image>().enabled = true;
        } 
    }
}
