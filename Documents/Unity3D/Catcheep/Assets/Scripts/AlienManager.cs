using System;
using System.Collections;
using System.Globalization;
using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AlienManager : MonoBehaviour
{
    [Header("In game objects")] public Sprite[] SheepSprites;
    public GameObject[] alienShip;
    public GameObject maskHolder;
    public GameObject SheepHoverGameObject;
    private music musicManager;

    [Header("UI components")] public GameObject rightButton;
    public GameObject leftButton;
    public GameObject timeText;
    public GameObject SheepMapGameObject;
    public GameObject mailPanel;
    public GameObject mailButton;
    public GameObject adButton;
    public GameObject neededPrayPrefab;
    public GameObject slider;
    public GameObject levelText;
    public GameObject levelUpPanel;
    public GameObject AdPanel;
    public GameObject problemPanel;
    public GameObject infoPanel;

    [Header("indication elements")] public GameObject sheepHolder;
    public GameObject sheepNumberText;
    public GameObject moneyAmountText;

    private GameObject spaceShipForScript;

    private Vector2 edgeOfScreen;
    private Vector2 oldPosition;
    private Vector2 newPosition;

    private int currentSheepShowed;
    private int[] sheepyRequested;
    private int shipType;
    private int[] indexes, amounts;


    // Use this for initialization
    void Start()
    {
        musicManager = GameObject.Find("Music Manager").GetComponent<music>();
        if (PlayerPrefs.GetInt("ship") == 0) PlayerPrefs.SetInt("ship", 1);
        shipType = PlayerPrefs.GetInt("ship") - 1;
        edgeOfScreen = new Vector2(Screen.width, Screen.height);
        sheepNumberText.GetComponent<TextMeshProUGUI>().text = " x " + PlayerPrefs.GetInt("sheepy");
        cashUpdate(PlayerPrefs.GetInt("money"));

        sheepyRequested = new[] {0, 0, 0, 0, 0, 0, 0};
        if (PlayerPrefs.GetInt("levelUp") == 0)
        {
            loadTextFile("missions", PlayerPrefs.GetInt("level"));
            PlayerPrefs.SetInt("levelUp", 1);
        }
        else
        {
            loadCurrentMission();
        }
        receivedMail(true);
        slider.GetComponent<Slider>().value = PlayerPrefs.GetInt("level");
        levelText.GetComponent<TextMeshProUGUI>().text = "" + (PlayerPrefs.GetInt("level") + 1);

        currentSheepShowed = -1;
        changingSheepPic(1);
        if (musicManager.timer[currentSheepShowed] <= 0) StartCoroutine(alienSpawner());
        else
        {
            string minutes = Mathf.Floor(musicManager.timer[currentSheepShowed] / 60).ToString("00");
            string seconds = (musicManager.timer[currentSheepShowed] % 60).ToString("00");
            timeText.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                minutes + ":" + seconds;
            timeText.SetActive(true);
            adButton.SetActive(true);
            StartCoroutine(objectOpened(timeText));
        }
    }

    int settingTimes(int currentSheepShowed)
    {
        int discount = 15 * (PlayerPrefs.GetInt("ship") - 1) - 5;
        if ((PlayerPrefs.GetInt("ship") - 1) == 0) discount = 0;
        switch (currentSheepShowed)
        {
            case 0:
                return 30 - (30 * discount / 100);
            case 1:
                return 45 - (45 * discount / 100);
            case 2:
                return 59 - (59 * discount / 100);
            case 3:
                return 45 - (45 * discount / 100);
            case 4:
                return 59 - (59 * discount / 100);
            case 5:
                return 59 - (59 * discount / 100);
            case 6:
                return 89 - (89 * discount / 100);
            default:
                print("Error: sheep index of aliens");
                return 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        updatingTimeText();

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            //when player presses, save that click position
            oldPosition = Input.mousePosition;
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            //when player lets go, save that position
            newPosition = Input.mousePosition;
        }
        //if both positions aren't null
        if (oldPosition != new Vector2(0f, 0f) && newPosition != new Vector2(0f, 0f))
        {
            if (rightButton.GetComponent<Button>().IsActive())
            {
                if (Vector2.Distance(oldPosition, newPosition) >= (edgeOfScreen.x * 0.2f) &&
                    Mathf.Abs(newPosition.y - oldPosition.y) < (edgeOfScreen.y * 0.07f))
                {
                    if (oldPosition.x < newPosition.x)
                    {
                        shipGoingRightButtonClicked();
                    }
                    else
                    {
                        shipGoingLeftButtonClicked();
                    }
                    oldPosition = new Vector2(0f, 0f);
                    newPosition = new Vector2(0f, 0f);
                }
            }
        }

        if (Input.touchCount == 0)
        {
            oldPosition = new Vector2(0f, 0f);
            newPosition = new Vector2(0f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            startMenu();
        }
    }

    void updatingTimeText()
    {
        if (musicManager.timer[currentSheepShowed] > 0)
        {
            string minutes = Mathf.Floor(musicManager.timer[currentSheepShowed] / 60).ToString("00");
            string seconds = (musicManager.timer[currentSheepShowed] % 60).ToString("00");
            timeText.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                minutes + ":" + seconds;
        }
        else if (spaceShipForScript == null)
        {
            adButton.SetActive(false);
            StartCoroutine(objectClosed(timeText));
            StartCoroutine(alienSpawner());
        }
    }

    IEnumerator alienSpawner()
    {
        deactivatingButtons();

        spaceShipForScript = Instantiate(alienShip[shipType], alienShip[shipType].transform.localPosition,
            alienShip[shipType].transform.rotation);
        spaceShipForScript.transform.SetParent(GameObject.Find("Canvas").transform, false);

        //sound of coming
        musicManager.ObjectsSound(0);

        //this next line makes the button of prefab ship clickable, do not alter !
        spaceShipForScript.GetComponentInChildren<Button>().onClick.AddListener(call: shipClicked);

        Vector3 destination = new Vector3(alienShip[shipType].transform.localPosition.x, -200f, 0f);
        do
        {
            spaceShipForScript.transform.localPosition =
                Vector3.Lerp(spaceShipForScript.transform.localPosition, destination, 4f * Time.deltaTime);
            yield return new WaitForSeconds(0.02f);
        } while ((int) spaceShipForScript.transform.localPosition.y > 0f);

        spaceShipForScript.transform.SetParent(maskHolder.transform, true);
        maskHolder.GetComponent<ScrollRect>().content = spaceShipForScript.GetComponent<RectTransform>();
        activatingButtons();
        musicManager.ObjectsSound(1);
    }

    void shipClicked()
    {
        if (sheepyRequested[currentSheepShowed] == 0)
        {
            openProblemPanel("This Sheep Is Not Needed !");
        }
        else
        {
            if (PlayerPrefs.GetInt("sheep" + currentSheepShowed) >= sheepyRequested[currentSheepShowed])
            {
                deactivatingButtons();
                PlayerPrefs.SetInt("sheep" + currentSheepShowed,
                    PlayerPrefs.GetInt("sheep" + currentSheepShowed) - sheepyRequested[currentSheepShowed]);
                PlayerPrefs.SetInt("sheepy", PlayerPrefs.GetInt("sheepy") - sheepyRequested[currentSheepShowed]);
                PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + (sheepyRequested[currentSheepShowed] * 100));
                PlayerPrefs.SetInt("mostMoney", PlayerPrefs.GetInt("mostMoney") + (sheepyRequested[currentSheepShowed] * 100));
                cashUpdate(PlayerPrefs.GetInt("money"));
                sheepNumberText.GetComponent<TextMeshProUGUI>().text = " x " + PlayerPrefs.GetInt("sheepy");
                sheepyRequested[currentSheepShowed] = 0;
                //this long thing will make sure the player will have updated hunting

                for (int i = 0; i < indexes.Length; i++)
                {
                    if (indexes[i] >= 0)
                    {
                        if (indexes[i] == currentSheepShowed)
                        {
                            amounts[i] = 0;
                            PlayerPrefs.SetInt("amount" + i, 0);
                        }
                    }
                }
                loadCurrentMission();
                //end of confusing code
                musicManager.UISFX(3);
                StartCoroutine(shipLeaving());
            }
            else
            {
                openProblemPanel("You Don't Have Enough Sheeps !");
            }
        }
    }

    IEnumerator shipLeaving()
    {
        //setting spaceship UI
        spaceShipForScript.transform.GetChild(2).GetComponent<Button>().enabled = false;
        spaceShipForScript.transform.GetChild(1).gameObject.SetActive(false);
        spaceShipForScript.transform.GetChild(2).gameObject.SetActive(false);
        spaceShipForScript.transform.GetChild(2).gameObject.SetActive(true);
        spaceShipForScript.transform.GetChild(0).gameObject.SetActive(true);
        //sound of going away
        musicManager.ObjectsSound(2);

        //setting sheeps to hover
        GameObject sheepHoverTemp = Instantiate(SheepHoverGameObject, SheepHoverGameObject.transform.localPosition,
            SheepHoverGameObject.transform.localRotation);
        sheepHoverTemp.transform.SetParent(GameObject.Find("Sheep shooter").transform, false);
        sheepHoverTemp.transform.GetChild(0).GetComponent<Image>().sprite = SheepSprites[currentSheepShowed];
        StartCoroutine(objectOpened(sheepHoverTemp));
        do
        {
            sheepHoverTemp.transform.position = new Vector3(
                sheepHoverTemp.transform.position.x,
                sheepHoverTemp.transform.position.y + 5, sheepHoverTemp.transform.position.z);
            yield return new WaitForSeconds(0.01f);
        } while (sheepHoverTemp.transform.position.y < spaceShipForScript.transform.GetChild(1).position.y + 30);
        Destroy(sheepHoverTemp);

        //sending the ship away
        Vector3 destination = new Vector3(alienShip[shipType].transform.localPosition.x, edgeOfScreen.y * 1.5f, 0f);
        do
        {
            spaceShipForScript.transform.localPosition =
                Vector3.Lerp(spaceShipForScript.transform.localPosition, destination, 4f * Time.deltaTime);
            yield return new WaitForSeconds(0.02f);
        } while ((int) spaceShipForScript.transform.localPosition.y < (int) destination.y - destination.y * 0.1f);
        Destroy(spaceShipForScript.gameObject);
        activatingButtons();

        //setting Timer
        timeText.SetActive(true);
        adButton.SetActive(true);
        StartCoroutine(objectOpened(timeText));
        spaceShipForScript = null;
        if (musicManager.timer[currentSheepShowed] <= 0)
        {
            musicManager.timer[currentSheepShowed] = settingTimes(currentSheepShowed);
            PlayerPrefs.SetFloat("time" + currentSheepShowed, musicManager.timer[currentSheepShowed]);
        }
        if (checkingIfMissionCompleted()) setSlider(PlayerPrefs.GetInt("level") + 1);
    }

    public void shipGoingRightButtonClicked()
    {
        StartCoroutine(shipGoingRight());
        musicManager.UISFX(0);
        musicManager.ObjectsSound(2);
    }

    IEnumerator shipGoingRight()
    {
        if (spaceShipForScript != null)
        {
            deactivatingButtons();
            Vector3 destination = new Vector3(
                edgeOfScreen.x + alienShip[shipType].GetComponentInChildren<Image>().sprite.bounds.size.x,
                spaceShipForScript.transform.localPosition.y,
                spaceShipForScript.transform.localPosition.z);

            do
            {
                spaceShipForScript.transform.localPosition = new Vector3(
                    spaceShipForScript.transform.localPosition.x + 20,
                    spaceShipForScript.transform.localPosition.y, spaceShipForScript.transform.localPosition.z);
                yield return new WaitForSeconds(0.01f);
            } while ((int) spaceShipForScript.transform.position.x < (int) destination.x * 1.3f);
            Destroy(spaceShipForScript.gameObject);
        }
        changingSheepPic(1);

        if (musicManager.timer[currentSheepShowed] <= 0) StartCoroutine(alienSpawner());
        else
        {
            activatingButtons();
        }
    }

    public void shipGoingLeftButtonClicked()
    {
        StartCoroutine(shipGoingLeft());
        musicManager.UISFX(0);
        musicManager.ObjectsSound(2);
    }

    IEnumerator shipGoingLeft()
    {
        if (timeText.gameObject.activeSelf)
        {
            adButton.SetActive(false);
            StartCoroutine(objectClosed(timeText));
        }
        if (spaceShipForScript != null)
        {
            deactivatingButtons();
            //spaceShipForScript.transform.localRotation = new Quaternion(spaceShipForScript.transform.localRotation.x, spaceShipForScript.transform.rotation.y, -5, spaceShipForScript.transform.rotation.w);
            Vector3 destination = new Vector3(
                -edgeOfScreen.x - alienShip[shipType].GetComponentInChildren<Image>().sprite.bounds.size.x,
                spaceShipForScript.transform.localPosition.y,
                spaceShipForScript.transform.localPosition.z);
            do
            {
                spaceShipForScript.transform.localPosition = new Vector3(
                    spaceShipForScript.transform.localPosition.x - 20,
                    spaceShipForScript.transform.localPosition.y, spaceShipForScript.transform.localPosition.z);
                yield return new WaitForSeconds(0.01f);
            } while ((int) spaceShipForScript.transform.position.x > (int) destination.x * 0.3f);
            Destroy(spaceShipForScript.gameObject);
        }
        changingSheepPic(-1);
        if (musicManager.timer[currentSheepShowed] <= 0)
        {
            StartCoroutine(alienSpawner());
        }
        else
        {
            activatingButtons();
        }
    }

    void changingSheepPic(int i)
    {
        currentSheepShowed += i;
        if (currentSheepShowed >= SheepSprites.Length)
        {
            currentSheepShowed = 0;
        }
        if (currentSheepShowed < 0)
        {
            currentSheepShowed = SheepSprites.Length - 1;
        }
        if (currentSheepShowed < SheepSprites.Length && SheepSprites[currentSheepShowed] != null)
        {
            sheepHolder.GetComponent<Image>().sprite = SheepSprites[currentSheepShowed];
        }
        String nameOfSheep = "";
        switch (currentSheepShowed)
        {
            case 0:
                nameOfSheep = "sheepy";
                break;
            case 1:
                nameOfSheep = "blacky";
                break;
            case 2:
                nameOfSheep = "flash";
                break;
            case 3:
                nameOfSheep = "S.sheepy";
                break;
            case 4:
                nameOfSheep = "S.blacky";
                break;
            case 5:
                nameOfSheep = "C.sheepy";
                break;
            case 6:
                nameOfSheep = "C.blacky";
                break;
        }
        sheepHolder.GetComponentInChildren<TextMeshProUGUI>().text = nameOfSheep;

        if (musicManager.timer[currentSheepShowed] > 0)
        {
            string minutes = Mathf.Floor(musicManager.timer[currentSheepShowed] / 60).ToString("00");
            string seconds = (musicManager.timer[currentSheepShowed] % 60).ToString("00");
            timeText.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                minutes + ":" + seconds;
            timeText.SetActive(true);
            adButton.SetActive(true);
            StartCoroutine(objectOpened(timeText));
        }
        else
        {
            adButton.SetActive(false);
            timeText.SetActive(false);
        }
    }

    void activatingButtons()
    {
        sheepHolder.GetComponent<Button>().enabled = true;
        rightButton.GetComponent<Button>().enabled = true;
        leftButton.GetComponent<Button>().enabled = true;
        mailButton.GetComponent<Button>().enabled = true;
    }

    void deactivatingButtons()
    {
        mailButton.GetComponent<Button>().enabled = false;
        sheepHolder.GetComponent<Button>().enabled = false;
        rightButton.GetComponent<Button>().enabled = false;
        leftButton.GetComponent<Button>().enabled = false;
    }

    public void openProblemPanel(string problemText)
    {
        musicManager.UISFX(1);
        StartCoroutine(objectOpened(problemPanel));
        problemPanel.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = problemText;
    }

    public void closeProblemPanel()
    {
        musicManager.UISFX(2);
        StartCoroutine(objectClosed(problemPanel));
    }

    public void openinfoPanel()
    {
        musicManager.UISFX(1);
        StartCoroutine(objectOpened(infoPanel));
    }

    //mail related code
    void receivedMail(bool state)
    {
        mailButton.GetComponent<Animator>().enabled = state;
        mailButton.transform.GetChild(0).gameObject.SetActive(state);
    }

    private void loadCurrentMission()
    {
        indexes = new[] {-1, -1, -1};
        amounts = new[] {0, 0, 0};
        for (int i = 0; i < 3; i++)
        {
            if (PlayerPrefs.GetInt("amount" + i) > 0)
            {
                indexes[i] = PlayerPrefs.GetInt("index" + i);
                amounts[i] = PlayerPrefs.GetInt("amount" + i);
            }
        }

        settingDemands(indexes, amounts);
    }

    private void loadTextFile(string fileName, int level)
    {
        //loading a text file
        String line = "";
        TextAsset text = Resources.Load(fileName) as TextAsset;
        line = text.ToString();

        //splitting it to missions (lines)
        String[] missions = line.Split(',');
        //splitting to words (only one line)
        String[] words = missions[level].Split(' ');
        //splitting data
        indexes = new int[words.Length / 2];
        amounts = new int[words.Length / 2];
        //parsing data
        for (int i = 0; i < words.Length; i++)
        {
            //these two line makes sure no space or comma is in parsing to integer
            int a = words[i].IndexOf(" ", StringComparison.Ordinal);
            if (a > -1) words[i] = words[i].Remove(a);
            int b = words[i].IndexOf(",", StringComparison.Ordinal);
            if (b > -1) words[i] = words[i].Remove(b);
            //parsing stuff
            if (i % 2 == 0)
            {
                indexes[i / 2] = int.Parse(words[i]);
                if (i / 2 < 3) PlayerPrefs.SetInt("index" + (i / 2), indexes[i / 2]);
            }
            if (i % 2 == 1)
            {
                amounts[(i - 1) / 2] = int.Parse(words[i]);
                if ((i - 1) / 2 < 3) PlayerPrefs.SetInt("amount" + (i - 1) / 2, amounts[(i - 1) / 2]);
            }
        }

        settingDemands(indexes, amounts);
    }

    void settingDemands(int[] index, int[] demand)
    {
        for (int i = 0; i < index.Length; i++)
        {
            //sheep needed (index) = demand (i is used to make things organised)
            //index and demand must be synched and taken from missions.txt
            //making sure it's not -1 (initialy)
            if (index[i] >= 0 && demand[i] > 0)
            {
                if (sheepyRequested[index[i]] == 0) sheepyRequested[index[i]] = demand[i];
            }
        }
    }

    public void openMailPanel()
    {
        receivedMail(false);
        if (PlayerPrefs.GetInt("levelUp") == 0)
        {
            loadTextFile("missions", PlayerPrefs.GetInt("level"));
            PlayerPrefs.SetInt("levelUp", 1);
        }
        StartCoroutine(fillingMailPanel(indexes, amounts));
        mailButton.GetComponent<Button>().enabled = false;
        mailPanel.SetActive(true);
        musicManager.UISFX(1);
    }

    IEnumerator fillingMailPanel(int[] index, int[] need)
    {
        //function objectOpened rewritten here cause of scale prblem
        //the function fill doesnt wait for the object to open first
        //which means everything will scale up (children of object)
        for (int i = 0; i <= 10; i++)
        {
            float a = (float) i / 10;
            mailPanel.transform.localScale = new Vector3(a, a, 1);
            yield return new WaitForSeconds(0.01f);
        }
        //filling the panel depending on mission
        //don't panic: this jst means centering the mission info in a nice way (excuse me, im tired)
        Vector2 position = new Vector2(mailPanel.transform.GetChild(0).transform.position.x,
            mailPanel.transform.GetChild(0).transform.position.y - (edgeOfScreen.y * 0.04f));

        for (int i = 0; i < need.Length; i++)
        {
            if (PlayerPrefs.GetInt("amount" + i) > 0)
            {
                position = new Vector2(position.x, position.y - (edgeOfScreen.y * 0.10f));
                GameObject obj = Instantiate(neededPrayPrefab, position, Quaternion.identity);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = "x" + need[i];
                obj.GetComponent<Image>().sprite = SheepSprites[index[i]];
                obj.transform.SetParent(mailPanel.transform, true);
                obj.transform.localScale = new Vector3(1, 1, 1);
                obj.transform.localPosition = new Vector2(-50, obj.transform.localPosition.y);
            }
        }
    }

    public void closeMailPanel()
    {
        mailButton.GetComponent<Button>().enabled = true;
        for (int i = 2; i < mailPanel.transform.childCount; i++)
        {
            Destroy(mailPanel.transform.GetChild(i).gameObject);
        }
        StartCoroutine(objectClosed(mailPanel));
        musicManager.UISFX(2);
    }


    //opening and closing objects animations (very important and used everywhere)
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
        SheepMapGameObject.SetActive(false);
    }

    //map panel code
    //sheep map functions
    void sheepMapNumberUpdate()
    {
        for (int i = 0; i < 7; i++)
        {
            SheepMapGameObject.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text =
                "" + PlayerPrefs.GetInt("sheep" + i);
        }
    }

    public void sheepMapClick()
    {
        if (SheepMapGameObject.gameObject.activeInHierarchy)
        {
            StartCoroutine(objectClosed(SheepMapGameObject));
            activatingButtons();
            musicManager.UISFX(2);
        }
        else
        {
            deactivatingButtons();
            sheepHolder.GetComponent<Button>().enabled = true;
            sheepMapNumberUpdate();
            SheepMapGameObject.SetActive(true);
            StartCoroutine(objectOpened(SheepMapGameObject));
            musicManager.UISFX(1);
        }
    }

    public void sheepyClicked()
    {
        if (currentSheepShowed != 0)
        {
            musicManager.UISFX(0);
            StartCoroutine(objectClosed(SheepMapGameObject));
            currentSheepShowed = -1;
            shipGoingRightButtonClicked();
        }
    }

    public void blackyClicked()
    {
        if (currentSheepShowed != 1)
        {
            musicManager.UISFX(0);
            StartCoroutine(objectClosed(SheepMapGameObject));
            currentSheepShowed = 0;
            shipGoingRightButtonClicked();
        }
    }

    public void flashClicked()
    {
        if (currentSheepShowed != 2)
        {
            musicManager.UISFX(0);
            StartCoroutine(objectClosed(SheepMapGameObject));
            currentSheepShowed = 1;
            shipGoingRightButtonClicked();
        }
    }

    public void sheepySnowClicked()
    {
        if (currentSheepShowed != 3)
        {
            musicManager.UISFX(0);
            StartCoroutine(objectClosed(SheepMapGameObject));
            currentSheepShowed = 2;
            shipGoingRightButtonClicked();
        }
    }

    public void blackySnowClicked()
    {
        if (currentSheepShowed != 4)
        {
            musicManager.UISFX(0);
            StartCoroutine(objectClosed(SheepMapGameObject));
            currentSheepShowed = 3;
            shipGoingRightButtonClicked();
        }
    }

    public void sheepyCityClicked()
    {
        if (currentSheepShowed != 5)
        {
            musicManager.UISFX(0);
            StartCoroutine(objectClosed(SheepMapGameObject));
            currentSheepShowed = 4;
            shipGoingRightButtonClicked();
        }
    }

    public void blackyCityClicked()
    {
        if (currentSheepShowed != 6)
        {
            musicManager.UISFX(0);
            StartCoroutine(objectClosed(SheepMapGameObject));
            currentSheepShowed = 5;
            shipGoingRightButtonClicked();
        }
    }

    public void CloseAdsPanel()
    {
        musicManager.UISFX(2);
        StartCoroutine(objectClosed(AdPanel));
    }

    //slider functions
    bool checkingIfMissionCompleted()
    {
        for (int i = 0; i < sheepyRequested.Length; i++)
        {
            if (sheepyRequested[i] != 0)
            {
                return false;
            }
        }
        return true;
    }

    void setSlider(int level)
    {
        if (level <= 49)
        {
            PlayerPrefs.SetInt("level", level);
            slider.GetComponent<Slider>().value = level;
            levelText.GetComponent<TextMeshProUGUI>().text = "" + (PlayerPrefs.GetInt("level") + 1);
            levelUpPanel.SetActive(true);
            levelUpPanel.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text =
                "" + (PlayerPrefs.GetInt("level") + 1);
            StartCoroutine(objectOpened(levelUpPanel));
            loadTextFile("missions", level);
            PlayerPrefs.SetInt("levelUp", 0);
            receivedMail(true);
            mailButton.GetComponent<Button>().enabled = false;
            switch ((PlayerPrefs.GetInt("level")))
            {
                case 9:
                    GameObject.Find("Music Manager").GetComponent<PlayServicesMyVersion>().unlockAchievement(GPGSIds.achievement_reach_level_10);
                    break;
                case 19:
                    GameObject.Find("Music Manager").GetComponent<PlayServicesMyVersion>().unlockAchievement(GPGSIds.achievement_reach_level_20);
                    break;
                case 29:
                    GameObject.Find("Music Manager").GetComponent<PlayServicesMyVersion>().unlockAchievement(GPGSIds.achievement_reach_level_30);
                    break;
                case 39:
                    GameObject.Find("Music Manager").GetComponent<PlayServicesMyVersion>().unlockAchievement(GPGSIds.achievement_reach_level_40);
                    break;
            }
        }
        else
        {
            PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
            GameObject.Find("Music Manager").GetComponent<PlayServicesMyVersion>().unlockAchievement(GPGSIds.achievement_reach_level_50);
            SceneManager.LoadScene("Introduction");
        }
    }

    public void closeLevelUpPanel()
    {
        musicManager.UISFX(2);
        StartCoroutine(objectClosed(levelUpPanel));
        mailButton.GetComponent<Button>().enabled = true;
    }

    public void startMenu()
    {
        musicManager.BackgroundMusic(0);
        GameObject.Find("Music Manager").GetComponents<AudioSource>()[2].Stop();
        SceneManager.LoadScene(1);
    }

    public void ShowRewardedVideo()
    {
        ShowOptions options = new ShowOptions();

        if (Advertisement.IsReady())
        {
            options.resultCallback = killTimeAd;
            Advertisement.Show("rewardedVideo", options);
        }
        else
        {
            musicManager.UISFX(1);
            StartCoroutine(objectOpened(AdPanel));
        }
    }

    void killTimeAd(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            musicManager.timer[currentSheepShowed] = 0f;
        }
    }

    void cashUpdate(int current)
    {
        musicManager.UISFX(3);
        string cash = current.ToString("N0", new NumberFormatInfo()
        {
            NumberGroupSizes = new[] { 3 },
            NumberGroupSeparator = ","
        });
        moneyAmountText.GetComponent<TextMeshProUGUI>().text = "$" + cash;
    }
}