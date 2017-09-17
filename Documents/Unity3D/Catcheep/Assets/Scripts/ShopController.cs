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
    public GameObject WheelOfFortuneGameObject;
    

    // Use this for initialization
    void Start ()
    {
        cashUpdate(PlayerPrefs.GetInt("money"));
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

    public void wheelOfFortuneButton()
    {
        WheelOfFortuneGameObject.SetActive(true);
        ShopMenuGameObject.SetActive(false);
    }
    public void wheelOfFortuneReturn()
    {
        WheelOfFortuneGameObject.SetActive(false);
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
            cashUpdate(PlayerPrefs.GetInt("money"));
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
            PlayerPrefs.SetInt("ship", 2);
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - 4000);
            cashUpdate(PlayerPrefs.GetInt("money"));
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
            PlayerPrefs.SetInt("ship", 3);
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - 8000);
            cashUpdate(PlayerPrefs.GetInt("money"));
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
            PlayerPrefs.SetInt("ship", 4);
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - 15000);
            cashUpdate(PlayerPrefs.GetInt("money"));
        }
        else
        {
            cashText.GetComponent<TextMeshProUGUI>().text = " Not enough Ca$h !!!";
        }
    }

    public void buyHayStack()
    {
        if (PlayerPrefs.GetInt("money") >= 15000)
        {
            PlayerPrefs.SetInt("hayStackStock",PlayerPrefs.GetInt("hayStackStock") + 1);
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - 15000);
            cashUpdate(PlayerPrefs.GetInt("money"));
        }
        else
        {
            cashText.GetComponent<TextMeshProUGUI>().text = " Not enough Ca$h !!!";
        }
    }

    public void buyNet()
    {
        if (PlayerPrefs.GetInt("money") >= 20000)
        {
            PlayerPrefs.SetInt("netStock", PlayerPrefs.GetInt("netStock") + 1);
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - 20000);
            cashUpdate(PlayerPrefs.GetInt("money"));
        }
        else
        {
            cashText.GetComponent<TextMeshProUGUI>().text = " Not enough Ca$h !!!";
        }
    }

    public void buyLove()
    {
        if (PlayerPrefs.GetInt("money") >= 30000)
        {
            PlayerPrefs.SetInt("loveStock", PlayerPrefs.GetInt("loveStock") + 1);
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - 30000);
            cashUpdate(PlayerPrefs.GetInt("money"));
        }
        else
        {
            cashText.GetComponent<TextMeshProUGUI>().text = " Not enough Ca$h !!!";
        }
    }

    void cashUpdate(int current)
    {
        cashText.GetComponent<TextMeshProUGUI>().text = "$" +current;
    }
}
