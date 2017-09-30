using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    [Header("Shop Menu UI")]
    public GameObject cashText;
    public GameObject MenuCashText;
    public GameObject BuyShipButtonGameObject;
    public GameObject ShopMenuGameObject;
    public GameObject HelpToolsGameObject;
    public GameObject EnergyGameObject;
    public GameObject WheelOfFortuneGameObject;
    public GameObject DecisionPanel;


    // Use this for initialization
    void Start()
    {
        Advertisement.Initialize("1453095");
        cashUpdate(PlayerPrefs.GetInt("money"));
        HelpToolsGameObject.transform.GetChild(0).transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("hayStackStock");
        HelpToolsGameObject.transform.GetChild(1).transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("netStock");
        HelpToolsGameObject.transform.GetChild(2).transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("loveStock");
        settingShipStatus();
    }

    void settingShipStatus()
    {
        for (int i = 1; i < 4; i++)
        {
            if (PlayerPrefs.GetInt("ship" + i) == 1)
            {
                BuyShipButtonGameObject.transform.GetChild(0).transform.GetChild(i).transform.GetChild(2)
                    .GetComponentInChildren<TextMeshProUGUI>().text = "Sold";
            }    
        }
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void buyShipButton()
    {
        StartCoroutine(objectOpened(BuyShipButtonGameObject));
        StartCoroutine(objectClosed(ShopMenuGameObject));
    }

    public void returnBuyShip()
    {
        StartCoroutine(objectClosed(BuyShipButtonGameObject));
        StartCoroutine(objectOpened(ShopMenuGameObject));
    }

    public void helpToolButton()
    {
        StartCoroutine(objectClosed(ShopMenuGameObject));
        StartCoroutine(objectOpened(HelpToolsGameObject));
    }

    public void returnHelpTool()
    {
        StartCoroutine(objectClosed(HelpToolsGameObject));
        StartCoroutine(objectOpened(ShopMenuGameObject));
    }

    public void wheelOfFortuneButton()
    {
        StartCoroutine(objectClosed(ShopMenuGameObject));
        StartCoroutine(objectOpened(WheelOfFortuneGameObject));
    }

    public void wheelOfFortuneReturn()
    {
        StartCoroutine(objectClosed(WheelOfFortuneGameObject));
        StartCoroutine(objectOpened(ShopMenuGameObject));
    }

    public void energyDringButtonClick()
    {
        StartCoroutine(objectClosed(ShopMenuGameObject));
        StartCoroutine(objectOpened(EnergyGameObject));
    }

    public void energyDringButtonOff()
    {
        StartCoroutine(objectClosed(EnergyGameObject));
        StartCoroutine(objectOpened(ShopMenuGameObject));
    }

    public void buyALifeButton()
    {
        if (PlayerPrefs.GetInt("money") >= 7500 && PlayerPrefs.GetInt("hearts") < PlayerPrefs.GetInt("maxHearts"))
        {
            StartCoroutine(objectOpened(DecisionPanel));
            DecisionPanel.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text =
                "Do you wanna buy an energy drink ?";
            DecisionPanel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(buyALife);
        }
    }

    public void NoDecisionPanel()
    {
        StartCoroutine(objectClosed(DecisionPanel));
    }

    void buyALife()
    {
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - 7500);
        cashUpdate(PlayerPrefs.GetInt("money"));
        PlayerPrefs.SetInt("hearts", PlayerPrefs.GetInt("hearts") + 1);
        GameObject.Find("hearts").GetComponent<TextMeshProUGUI>().text =
            "x" + PlayerPrefs.GetInt("hearts");
        StartCoroutine(objectClosed(DecisionPanel));
    }

    public void killTimeButton()
    {
        if (PlayerPrefs.GetInt("money") >= 3000 && PlayerPrefs.GetFloat("heartTime") >= 1)
        {
            StartCoroutine(objectOpened(DecisionPanel));
            DecisionPanel.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text =
                "Do you wanna set the energy drink timer to 00:00 ?";
            DecisionPanel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(killTime);
        }
    }

    void killTime()
    {
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - 3000);
        PlayerPrefs.SetFloat("heartTime", -1);
        cashUpdate(PlayerPrefs.GetInt("money"));
        StartCoroutine(objectClosed(DecisionPanel));
    }

    public void moreCapacityButton()
    {
        if (PlayerPrefs.GetInt("money") >= 12500 && PlayerPrefs.GetInt("maxHearts") < 10)
        {
            StartCoroutine(objectOpened(DecisionPanel));
            DecisionPanel.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text =
                "Do you wanna buy more energy drinks capacity (+1) ?";
            DecisionPanel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(moreCapacity);
        }
    }

    void moreCapacity()
    {
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - 12500);
        PlayerPrefs.SetInt("maxHearts", PlayerPrefs.GetInt("maxHearts") + 1);
        cashUpdate(PlayerPrefs.GetInt("money"));
        StartCoroutine(objectClosed(DecisionPanel));
    }

    public void ShowAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }

    public void shipOneButton()
    {
        StartCoroutine(objectOpened(DecisionPanel));
        DecisionPanel.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text =
            "Do you Set this Ship as Default ship ?";
        DecisionPanel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(ShipOne);
    }

    void ShipOne()
    {
        PlayerPrefs.SetInt("ship", 1);
        StartCoroutine(objectClosed(DecisionPanel));
    }

    public void shipTwoButton()
    {
        if (PlayerPrefs.GetInt("money") >= 15000)
        {
            StartCoroutine(objectOpened(DecisionPanel));
            DecisionPanel.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text =
                "Do you Set this Ship as Default ship ?";
            DecisionPanel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(ShipTwo);
        }
        else
        {
            cashText.GetComponent<TextMeshProUGUI>().text = " Not enough Ca$h !!!";
            StartCoroutine(objectClosed(DecisionPanel));
        }
    }

    void ShipTwo()
    {
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - 15000);
        cashUpdate(PlayerPrefs.GetInt("money"));
        PlayerPrefs.SetInt("ship", 2);
        PlayerPrefs.SetInt("ship1", 1);
        settingShipStatus();
        StartCoroutine(objectClosed(DecisionPanel));
    }

    public void shipThreeButton()
    {
        if (PlayerPrefs.GetInt("money") >= 25000)
        {
            StartCoroutine(objectOpened(DecisionPanel));
            DecisionPanel.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text =
                "Do you Set this Ship as Default ship ?";
            DecisionPanel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(ShipThree);
        }
        else
        {
            cashText.GetComponent<TextMeshProUGUI>().text = " Not enough Ca$h !!!";
            StartCoroutine(objectClosed(DecisionPanel));
        }
    }

    void ShipThree()
    {
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - 25000);
        cashUpdate(PlayerPrefs.GetInt("money"));
        PlayerPrefs.SetInt("ship", 3);
        PlayerPrefs.SetInt("ship2", 1);
        settingShipStatus();
        StartCoroutine(objectClosed(DecisionPanel));
    }

    public void shipFourButton()
    {
        if (PlayerPrefs.GetInt("money") >= 40000 )
        {
            StartCoroutine(objectOpened(DecisionPanel));
            DecisionPanel.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text =
                "Do you Set this Ship as Default ship ?";
            DecisionPanel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(ShipFour);
        }
        else
        {
            cashText.GetComponent<TextMeshProUGUI>().text = " Not enough Ca$h !!!";
            StartCoroutine(objectClosed(DecisionPanel));
        }
    }

    void ShipFour()
    {
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - 40000);
        cashUpdate(PlayerPrefs.GetInt("money"));
        PlayerPrefs.SetInt("ship", 4);
        PlayerPrefs.SetInt("ship3", 1);
        settingShipStatus();
        StartCoroutine(objectClosed(DecisionPanel));
    }

    public void buyHayButton()
    {
        if (PlayerPrefs.GetInt("money") >= 15000)
        {
            StartCoroutine(objectOpened(DecisionPanel));
            DecisionPanel.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text =
                "Do you wanna buy a HayStack (+1) ?";
            DecisionPanel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(buyHayStack);
        }
        else
        {
            cashText.GetComponent<TextMeshProUGUI>().text = " Not enough Ca$h !!!";
            StartCoroutine(objectClosed(DecisionPanel));
        }
    }

    void buyHayStack()
    {
        PlayerPrefs.SetInt("hayStackStock", PlayerPrefs.GetInt("hayStackStock") + 1);
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - 15000);
        cashUpdate(PlayerPrefs.GetInt("money"));
        HelpToolsGameObject.transform.GetChild(0).transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("hayStackStock");
        StartCoroutine(objectClosed(DecisionPanel));
    }

    public void buyNetButton()
    {
        if (PlayerPrefs.GetInt("money") >= 20000)
        {
            StartCoroutine(objectOpened(DecisionPanel));
            DecisionPanel.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text =
                "Do you wanna buy a Net (+1) ?";
            DecisionPanel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(buyNet);
        }
        else
        {
            cashText.GetComponent<TextMeshProUGUI>().text = " Not enough Ca$h !!!";
            StartCoroutine(objectClosed(DecisionPanel));
        }
    }

    public void buyNet()
    {
        PlayerPrefs.SetInt("netStock", PlayerPrefs.GetInt("netStock") + 1);
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - 20000);
        cashUpdate(PlayerPrefs.GetInt("money"));
        HelpToolsGameObject.transform.GetChild(1).transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("netStock");
        StartCoroutine(objectClosed(DecisionPanel));
    }

    public void buyLoveButton()
    {
        if (PlayerPrefs.GetInt("money") >= 30000)
        {
            StartCoroutine(objectOpened(DecisionPanel));
            DecisionPanel.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text =
                "Do you wanna buy a Female sheepy (+1) ?";
            DecisionPanel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(buyLove);
        }
        else
        {
            cashText.GetComponent<TextMeshProUGUI>().text = " Not enough Ca$h !!!";
            StartCoroutine(objectClosed(DecisionPanel));
        }
    }

    public void buyLove()
    {
        PlayerPrefs.SetInt("loveStock", PlayerPrefs.GetInt("loveStock") + 1);
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - 30000);
        HelpToolsGameObject.transform.GetChild(2).transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("loveStock");
        cashUpdate(PlayerPrefs.GetInt("money"));
        StartCoroutine(objectClosed(DecisionPanel));
    }

    void cashUpdate(int current)
    {
        cashText.GetComponent<TextMeshProUGUI>().text = "$" + current;
        MenuCashText.GetComponent<TextMeshProUGUI>().text = "$" + current;
    }

    public void facebook()
    {
        Application.OpenURL("fb://page/344400029290078");
    }

    IEnumerator objectOpened(GameObject objectToOpen)
    {
        objectToOpen.SetActive(true);
        for (int i = 0; i <= 10; i++)
        {
            float a = (float) i / 10;
            objectToOpen.transform.localScale = new Vector3(a, a, 1);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator objectClosed(GameObject objectToOpen)
    {
        for (int i = 10; i >= 0; i--)
        {
            float a = (float) i / 10;
            objectToOpen.transform.localScale = new Vector3(a, a, 1);
            yield return new WaitForSeconds(0.01f);
        }
        objectToOpen.SetActive(false);
    }

    //ADS SECTION
    public void hayStackAdFunction()
    {
        ShowRewardedVideo(0);
    }
    public void netAdFunction()
    {
        ShowRewardedVideo(1);
    }
    public void loveAdFunction()
    {
        ShowRewardedVideo(2);
    }
    public void shipTwoAdFunction()
    {
        ShowRewardedVideo(3);
    }
    public void shipThreeAdFunction()
    {
        ShowRewardedVideo(4);
    }
    public void shipFourAdFunction()
    {
        ShowRewardedVideo(5);
    }
    public void buyALifeAdFunction()
    {
        ShowRewardedVideo(6);
    }
    public void killTimeAdFunction()
    {
        ShowRewardedVideo(7);
    }
    public void moreCapacityAdFunction()
    {
        ShowRewardedVideo(8);
    }
    void ShowRewardedVideo(int index)
    {
        ShowOptions options = new ShowOptions();
        switch (index)
        {
            case 0:
                options.resultCallback = hayAd;
                break;
            case 1:
                options.resultCallback = netAd;
                break;
            case 2:
                options.resultCallback = loveAd;
                break;
            case 3:
                options.resultCallback = shipTwoAd;
                break;
            case 4:
                options.resultCallback = shipThreeAd;
                break;
            case 5:
                options.resultCallback = shipFourAd;
                break;
            case 6:
                options.resultCallback = buyALifeAd;
                break;
            case 7:
                options.resultCallback = killTimeAd;
                break;
            case 8:
                options.resultCallback = moreCapacityAd;
                break;
        }
        if (Advertisement.IsReady())
        {
            Advertisement.Show("rewardedVideo", options);
        }
    }

    void hayAd(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            PlayerPrefs.SetInt("hayStackStock", PlayerPrefs.GetInt("hayStackStock") + 1);
            HelpToolsGameObject.transform.GetChild(0).transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("hayStackStock");
        }
    }
    void netAd(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            PlayerPrefs.SetInt("netStock", PlayerPrefs.GetInt("netStock") + 1);
            HelpToolsGameObject.transform.GetChild(1).transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("netStock");
        }
    }
    void loveAd(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            PlayerPrefs.SetInt("loveStock", PlayerPrefs.GetInt("loveStock") + 1);
            HelpToolsGameObject.transform.GetChild(2).transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("loveStock");
        }
    }
    void shipTwoAd(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            PlayerPrefs.SetInt("ship", 2);
            PlayerPrefs.SetInt("ship1", 1);
            settingShipStatus();
        }
    }
    void shipThreeAd(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            PlayerPrefs.SetInt("ship", 3);
            PlayerPrefs.SetInt("ship2", 1);
            settingShipStatus();
        }
    }
    void shipFourAd(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            PlayerPrefs.SetInt("ship", 4);
            PlayerPrefs.SetInt("ship3", 1);
            settingShipStatus();
        }
    }
    void moreCapacityAd(ShowResult result)
    {
        if (result == ShowResult.Finished && PlayerPrefs.GetInt("maxHearts") < 10)
        {
            PlayerPrefs.SetInt("maxHearts", PlayerPrefs.GetInt("maxHearts") + 1);
        }
    }
    void buyALifeAd(ShowResult result)
    {
        if (result == ShowResult.Finished && PlayerPrefs.GetInt("hearts") < PlayerPrefs.GetInt("maxHearts"))
        {
            PlayerPrefs.SetInt("hearts", PlayerPrefs.GetInt("hearts") + 1);
            GameObject.Find("hearts").GetComponent<TextMeshProUGUI>().text =
                "x" + PlayerPrefs.GetInt("hearts");
        }
    }
    void killTimeAd(ShowResult result)
    {
        if (result == ShowResult.Finished && PlayerPrefs.GetFloat("heartTime") >= 1)
        {
            PlayerPrefs.SetFloat("heartTime", -1);
            cashUpdate(PlayerPrefs.GetInt("money"));
        }
    }
}