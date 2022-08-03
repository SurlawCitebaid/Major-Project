using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class shopScript : MonoBehaviour
{
    [SerializeField] GameObject currencyObject;
    [SerializeField] GameObject shopButtonObject;
    [SerializeField] GameObject loadoutButtonObject;
    [SerializeField] GameObject shopItemsPanel;
    Text currency;
    Button shopButton;
    Button loadoutButton;
    TextMeshProUGUI shopText;
    
    private void Start()
    {
        currency = currencyObject.GetComponent<Text>();
        shopButton = shopButtonObject.GetComponent<Button>();
        loadoutButton = loadoutButtonObject.GetComponent<Button>();
        shopText = shopButtonObject.GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>();

    }
    private void Update()
    {
        int myInt = 100;
        currency.text = myInt.ToString();
    }
    public void bass()
    {
        shopItemsPanel.SetActive(false);
        shopText.fontStyle = FontStyles.Normal;
        shopButton.GetComponent<Image>().enabled = false;
    }
    public void ass()
    {
        shopItemsPanel.SetActive(true);
        shopText.fontStyle = FontStyles.Bold;
        shopButton.GetComponent<Image>().enabled = true;
    }
}
