using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;

public class ShopController : MonoBehaviour
{
    [Header("Shop Menu UI")]
    public GameObject cashText;
    public GameObject BuyShipButtonGameObject;
    public GameObject ShopMenuGameObject;
    public GameObject HelpToolsGameObject;
    

    // Use this for initialization
    void Start ()
    {
        cashText.GetComponent<TextMeshProUGUI>().text = " Ca$h: < i >< b >"+PlayerPrefs.GetInt("money")+"</ b ></ i >";
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void buyShipButton()
    { 
        BuyShipButtonGameObject.SetActive(true);
        ShopMenuGameObject.SetActive(false);
    }
    public void returnBuyShip()
    {
        BuyShipButtonGameObject.SetActive(false);
        ShopMenuGameObject.SetActive(true);
    }

    public void helpToolButton()
    {
        HelpToolsGameObject.SetActive(true);
        ShopMenuGameObject.SetActive(false);
    }
    public void returnHelpTool()
    {
        HelpToolsGameObject.SetActive(false);
        ShopMenuGameObject.SetActive(true);
    }

    public void ShowAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }

    public void ShipOne()
    {
        if (PlayerPrefs.GetInt("money") >= 2500)
        {
            PlayerPrefs.SetInt("ship", 1);
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - 2500);
            cashText.GetComponent<TextMeshProUGUI>().text = " Ca$h: < i >< b >" + PlayerPrefs.GetInt("money") + "</ b ></ i >";
        }
        else
        {

            cashText.GetComponent<TextMeshProUGUI>().text = " Not enough Ca$h !!!";

        }
    }
    public void ShipTwo()
    {
        if (PlayerPrefs.GetInt("money") >= 4000)
        {
            PlayerPrefs.SetInt("ship", 1);
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - 4000);
            cashText.GetComponent<TextMeshProUGUI>().text = " Ca$h: < i >< b >" + PlayerPrefs.GetInt("money") + "</ b ></ i >";
        }
        else
        {

            cashText.GetComponent<TextMeshProUGUI>().text = " Not enough Ca$h !!!";

        }
    }
    public void ShipThree()
    {
        if (PlayerPrefs.GetInt("money") >= 8000)
        {
            PlayerPrefs.SetInt("ship", 1);
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - 8000);
            cashText.GetComponent<TextMeshProUGUI>().text = " Ca$h: < i >< b >" + PlayerPrefs.GetInt("money") + "</ b ></ i >";
        }
        else
        {

            cashText.GetComponent<TextMeshProUGUI>().text = " Not enough Ca$h !!!";

        }
    }
    public void ShipFour()
    {
        if (PlayerPrefs.GetInt("money") >= 15000)
        {
            PlayerPrefs.SetInt("ship", 1);
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - 15000);
            cashText.GetComponent<TextMeshProUGUI>().text = " Ca$h: < i >< b >" + PlayerPrefs.GetInt("money") + "</ b ></ i >";
        }
        else
        {

            cashText.GetComponent<TextMeshProUGUI>().text = " Not enough Ca$h !!!";

        }
    }
}
