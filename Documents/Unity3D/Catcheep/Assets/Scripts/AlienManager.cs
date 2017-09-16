using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AlienManager : MonoBehaviour
{
    [Header("In game objects")] public Sprite[] SheepSprites;
    public GameObject[] alienShip;
    public GameObject maskHolder;
    public GameObject SheepHoverGameObject;

    [Header("UI components")] public GameObject rightButton;
    public GameObject leftButton;
    public GameObject timeText;
    public GameObject SheepMapGameObject;
    public GameObject mailPanel;
    public GameObject mailButton;
    public GameObject neededPrayPrefab;

    [Header("indication elements")] public GameObject sheepHolder;
    public GameObject sheepNumberText;
    public GameObject moneyAmountText;

    private GameObject spaceShipForScript;

    private Vector2 edgeOfScreen;
    private Vector2 oldPosition;
    private Vector2 newPosition;

    private int currentSheepShowed;
    private int sheepyRequested;
    private int shipType;
    private float[] timer;
    private int[] indexes, amounts;


    // Use this for initialization
    void Start()
    {
        timer = new float[SheepSprites.Length];
        shipType = PlayerPrefs.GetInt("ship") - 1;
        edgeOfScreen = new Vector2(Screen.width, Screen.height);

        //setting the sheeps demands info
        //JsonData json = JsonReader.getJsonFile("sheepy.json");
        //moneyAmountText.GetComponent<TextMeshProUGUI>().text = JsonReader.getDataFromJson(json, "time");//PlayerPrefs.GetInt("money") + " $";
        settingDemands();
        settingTimerOnStart();

        currentSheepShowed = -1;
        changingSheepPic(1);
        if (timer[currentSheepShowed] <= 0) StartCoroutine(alienSpawner());
    }

    void settingTimerOnStart()
    {
        for (int i = 0; i < 1; i++)
        {
            String url = JsonReader.getDataByIndex("sheeps", i).ToString();
            JsonData json = JsonReader.getJsonFile(url);
            int time = int.Parse(JsonReader.getDataFromJson(json, "time"));
            timer[i] = gettingRemainingTime(url, time);
        }
    }

    int settingTimes(int currentSheepShowed)
    {
        switch (currentSheepShowed)
        {
            case 0:
                return settingTimeOfClick("sheepy.json");
            case 1:
                return 59;
            case 2:
                return 59;
            case 3:
                return 59;
            case 4:
                return 59;
            case 5:
                return 59;
            case 6:
                return 59;
            default:
                print("Error: sheep index of aliens");
                return 0;
        }
    }

    int settingTimeOfClick(string url)
    {
        JsonReader.timeModifier(url);
        JsonData json = JsonReader.getJsonFile(url);
        return int.Parse(JsonReader.getDataFromJson(json, "time"));
    }

    int gettingRemainingTime(string url, int time)
    {
        JsonData json = JsonReader.getJsonFile(url);
        DateTime startTime = DateTime.Parse(JsonReader.getDataFromJson(json, "timeOfSell"));
        DateTime endTime = DateTime.Now;
        TimeSpan amounTime = endTime - startTime;
        if (time - (int) amounTime.TotalSeconds > 0) return time - (int) amounTime.TotalSeconds;
        else return 0;
    }

    // Update is called once per frame
    void Update()
    {
        updatingTimers();

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

    void updatingTimers()
    {
        for (int i = 0; i < timer.Length; i++)
        {
            if (timer[i] > 0)
            {
                string minutes = Mathf.Floor(timer[currentSheepShowed] / 60).ToString("00");
                string seconds = (timer[currentSheepShowed] % 60).ToString("00");
                timeText.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                    minutes + ":" + seconds;
                timer[i] -= Time.deltaTime;
            }
        }
    }

    IEnumerator alienSpawner()
    {
        deactivatingButtons();

        spaceShipForScript = Instantiate(alienShip[shipType], alienShip[shipType].transform.localPosition,
            alienShip[shipType].transform.rotation);
        spaceShipForScript.transform.SetParent(GameObject.Find("Canvas").transform, false);

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
    }

    IEnumerator notEnoughtSheeps()
    {
        yield return new WaitForSeconds(1);
    }

    void settingDemands()
    {
        sheepyRequested = Random.Range(2, 10);
        sheepNumberText.GetComponent<TextMeshProUGUI>().text = " x " + PlayerPrefs.GetInt("sheepy");
    }


    void shipClicked()
    {
        if (PlayerPrefs.GetInt("sheepy") >= sheepyRequested)
        {
            PlayerPrefs.SetInt("sheepy", PlayerPrefs.GetInt("sheepy") - sheepyRequested);
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + (sheepyRequested * 100));
            moneyAmountText.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("money") + " $";
            sheepNumberText.GetComponent<TextMeshProUGUI>().text = " x " + PlayerPrefs.GetInt("sheepy");
            sheepyRequested = 0;
            deactivatingButtons();
            StartCoroutine(shipLeaving());
        }
        else
        {
            StartCoroutine(notEnoughtSheeps());
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
        StartCoroutine(objectOpened(timeText));
        if (timer[currentSheepShowed] <= 0) timer[currentSheepShowed] = settingTimes(currentSheepShowed);
    }

    public void shipGoingRightButtonClicked()
    {
        settingDemands();
        StartCoroutine(shipGoingRight());
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

        if (timer[currentSheepShowed] <= 0) StartCoroutine(alienSpawner());
        else
        {
            activatingButtons();
        }
    }

    public void shipGoingLeftButtonClicked()
    {
        settingDemands();
        StartCoroutine(shipGoingLeft());
    }

    IEnumerator shipGoingLeft()
    {
        if (timeText.gameObject.activeSelf)
        {
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
        if (timer[currentSheepShowed] <= 0)
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

        if (timer[currentSheepShowed] > 0)
        {
            string minutes = Mathf.Floor(timer[currentSheepShowed] / 60).ToString("00");
            string seconds = (timer[currentSheepShowed] % 60).ToString("00");
            timeText.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                minutes + ":" + seconds;
            timeText.SetActive(true);
            StartCoroutine(objectOpened(timeText));
        }
        else
        {
            timeText.SetActive(false);
        }
    }

    void activatingButtons()
    {
        rightButton.GetComponent<Button>().enabled = true;
        leftButton.GetComponent<Button>().enabled = true;
    }

    void deactivatingButtons()
    {
        rightButton.GetComponent<Button>().enabled = false;
        leftButton.GetComponent<Button>().enabled = false;
    }


    //mail related code
    void receivedMail(bool state)
    {
        mailButton.GetComponent<Animator>().enabled = state;
        mailButton.transform.GetChild(0).gameObject.SetActive(state);
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
            if (i % 2 == 0) indexes[i / 2] = int.Parse(words[i]);
            if (i % 2 == 1) amounts[(i - 1) / 2] = int.Parse(words[i]);
        }
        StartCoroutine(fillingMailPanel(indexes, amounts));
    }


    public void openMailPanel()
    {
        receivedMail(false);
        loadTextFile("missions", 0);
        mailButton.GetComponent<Button>().enabled = false;
        mailPanel.SetActive(true);
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
            position = new Vector2(position.x, position.y - (edgeOfScreen.y * 0.10f));
            GameObject obj = Instantiate(neededPrayPrefab, position, Quaternion.identity);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = "x" + need[i];
            obj.GetComponent<Image>().sprite = SheepSprites[index[i]];
            obj.transform.SetParent(mailPanel.transform, true);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.transform.localPosition = new Vector2(-50, obj.transform.localPosition.y);
        }
    }

    public void closeMailPanel()
    {
        receivedMail(true);
        mailButton.GetComponent<Button>().enabled = true;
        for (int i = 2; i < mailPanel.transform.childCount; i++)
        {
            Destroy(mailPanel.transform.GetChild(i).gameObject);
        }
        StartCoroutine(objectClosed(mailPanel));
        mailPanel.SetActive(false);
    }


    //opening and closing objects animations (very important and used everywhere)
    IEnumerator objectOpened(GameObject objectToOpen)
    {
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
        SheepMapGameObject.SetActive(false);
    }

    //map panel code
    //sheep map functions
    public void sheepMapClick()
    {
        if (SheepMapGameObject.gameObject.activeInHierarchy)
        {
            StartCoroutine(objectClosed(SheepMapGameObject));
            activatingButtons();
        }
        else
        {
            deactivatingButtons();
            SheepMapGameObject.SetActive(true);
            StartCoroutine(objectOpened(SheepMapGameObject));
        }
    }

    public void sheepyClicked()
    {
        StartCoroutine(objectClosed(SheepMapGameObject));
        currentSheepShowed = -1;
        shipGoingRightButtonClicked();
    }

    public void blackyClicked()
    {
        StartCoroutine(objectClosed(SheepMapGameObject));
        currentSheepShowed = 0;
        shipGoingRightButtonClicked();
    }

    public void flashClicked()
    {
        StartCoroutine(objectClosed(SheepMapGameObject));
        currentSheepShowed = 1;
        shipGoingRightButtonClicked();
    }

    public void sheepySnowClicked()
    {
        StartCoroutine(objectClosed(SheepMapGameObject));
        currentSheepShowed = 2;
        shipGoingRightButtonClicked();
    }

    public void blackySnowClicked()
    {
        StartCoroutine(objectClosed(SheepMapGameObject));
        currentSheepShowed = 3;
        shipGoingRightButtonClicked();
    }

    public void sheepyCityClicked()
    {
        StartCoroutine(objectClosed(SheepMapGameObject));
        currentSheepShowed = 4;
        shipGoingRightButtonClicked();
    }

    public void blackyCityClicked()
    {
        StartCoroutine(objectClosed(SheepMapGameObject));
        currentSheepShowed = 5;
        shipGoingRightButtonClicked();
    }

    public void startMenu()
    {
        SceneManager.LoadScene(1);
    }
}