using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    [Header("Shop Menu UI")] public GameObject cashText;
    public GameObject BuyShipButtonGameObject;
    public GameObject ShopMenuGameObject;
    public GameObject HelpToolsGameObject;
    public GameObject EnergyGameObject;
    public GameObject WheelOfFortuneGameObject;
    public GameObject DecisionPanel;


    // Use this for initialization
    void Start()
    {
        cashUpdate(PlayerPrefs.GetInt("money"));
    }

    // Update is called once per frame
    void Update()
    {
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

    public void energyDringButtonClick()
    {
        ShopMenuGameObject.SetActive(false);
        EnergyGameObject.SetActive(true);
    }

    public void energyDringButtonOff()
    {
        ShopMenuGameObject.SetActive(true);
        EnergyGameObject.SetActive(false);
    }

    public void buyALifeButton()
    {
        if (PlayerPrefs.GetInt("money") >= 7500 && PlayerPrefs.GetInt("hearts") < PlayerPrefs.GetInt("maxHearts"))
        {
            DecisionPanel.SetActive(true);
            DecisionPanel.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text =
                "Do you wanna buy an energy drink ?";
            DecisionPanel.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(buyALife);
        }
    }

    public void NoDecisionPanel()
    {
        DecisionPanel.SetActive(false);
    }

    void buyALife()
    {
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - 7500);
        cashUpdate(PlayerPrefs.GetInt("money"));
        PlayerPrefs.SetInt("hearts", PlayerPrefs.GetInt("hearts") + 1);
        GameObject.Find("hearts").GetComponent<TextMeshProUGUI>().text =
            "x" + PlayerPrefs.GetInt("hearts");
        NoDecisionPanel();
    }

    public void killTimeButton()
    {
        if (PlayerPrefs.GetInt("money") >= 3000 && PlayerPrefs.GetFloat("heartTime") >= 1)
        {
            DecisionPanel.SetActive(true);
            DecisionPanel.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text =
                "Do you wanna set the energy drink timer to 00:00 ?";
            DecisionPanel.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(killTime);
        }
    }

    void killTime()
    {
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - 3000);
        PlayerPrefs.SetFloat("heartTime", -1);
        NoDecisionPanel();
    }

    public void moreCapacityButton()
    {
        if (PlayerPrefs.GetInt("money") >= 12500 && PlayerPrefs.GetInt("maxHearts") < 10)
        {
            DecisionPanel.SetActive(true);
            DecisionPanel.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text =
                "Do you wanna buy more energy drinks capacity (+1) ?";
            DecisionPanel.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(moreCapacity);
        }
    }

    public void moreCapacity()
    {
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - 12500);
        PlayerPrefs.SetInt("maxHearts", PlayerPrefs.GetInt("maxHearts") + 1);
        NoDecisionPanel();
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
            PlayerPrefs.SetInt("hayStackStock", PlayerPrefs.GetInt("hayStackStock") + 1);
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
        cashText.GetComponent<TextMeshProUGUI>().text = "$" + current;
    }

    public void facebook()
    {
        Application.OpenURL("https://www.facebook.com/JetLightstudio/");
    }
}