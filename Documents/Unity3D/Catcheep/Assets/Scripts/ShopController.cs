using System.Collections;
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
        StartCoroutine(objectClosed(DecisionPanel));
    }

    public void shipFourButton()
    {
        if (PlayerPrefs.GetInt("money") >= 40000)
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
        cashUpdate(PlayerPrefs.GetInt("money"));
        StartCoroutine(objectClosed(DecisionPanel));
    }

    void cashUpdate(int current)
    {
        cashText.GetComponent<TextMeshProUGUI>().text = "$" + current;
    }

    public void facebook()
    {
        Application.OpenURL("www.facebook.com/JetLightstudio/");
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
}