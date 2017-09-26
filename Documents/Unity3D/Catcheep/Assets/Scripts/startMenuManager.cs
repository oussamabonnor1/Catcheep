using System;
using System.Collections;
using System.Collections.Generic;
using Assets.UTime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class startMenuManager : MonoBehaviour
{
    [Header("in Game objects")]
    public GameObject sceneContent;
    public GameObject ScrollBarGameObject;
    public GameObject vibrationToggle;
    public GameObject bossSheep;
    public GameObject timeText;
    public GameObject levelText;
    public GameObject heartText;
    public GameObject moneyText;
    public GameObject spinText;
    private Scrollbar ScrollBar;

    private Vector2 edgeOfScreen;
    private Vector2 oldPosition;
    private Vector2 newPosition;

    [Header("UI")]
    public int menuCount;
    private float portion;
    private float destination;
    private int value;

    private bool isGoingUp;
    private bool isGoingDown;


    // Use this for initialization
    void Start()
    {
        dailySpinTime();
        isGoingDown = false;
        isGoingUp = false;

        portion = (float) 1 / menuCount;
        portion = (float) Math.Round(portion, 2);

        //making sure all objects are affected and not null
        if (ScrollBarGameObject == null) ScrollBarGameObject = GameObject.Find("Horizontal scrollbar");
        if (sceneContent == null) sceneContent = GameObject.Find("Scene content");

        //reminding player of vibration (delete when finished devloping)
        if (PlayerPrefs.GetString("Vibration") == "True") vibrationToggle.GetComponent<Switch>().isOn = true;
        else vibrationToggle.GetComponent<Switch>().isOn = false;

        //extracting necissary elements for functions
        ScrollBar = ScrollBarGameObject.GetComponent<Scrollbar>();
        edgeOfScreen = new Vector2(Screen.width, Screen.height);

        //starting boss sheep animation a second after the scene loading (explaining "1" as param)
        StartCoroutine(bossPopingUp(Random.Range(5,10)));

        //giving data to texts (as late as possible)
        heartText.GetComponent<TextMeshProUGUI>().text = "x" + PlayerPrefs.GetInt("hearts");
        moneyText.GetComponent<TextMeshProUGUI>().text = "$" + PlayerPrefs.GetInt("money");
        levelText.GetComponent<TextMeshProUGUI>().text = "Level " + PlayerPrefs.GetInt("level");
    }

    // Update is called once per frame
    void Update()
    {
        //updating timer
        if (PlayerPrefs.GetInt("hearts") < PlayerPrefs.GetInt("maxHearts"))
        {
            string minutes = Mathf.Floor(PlayerPrefs.GetFloat("heartTime") / 60).ToString("00");
            string seconds = (PlayerPrefs.GetFloat("heartTime") % 60).ToString("00");
            timeText.GetComponent<TextMeshProUGUI>().text =
                minutes + ":" + seconds;
        }

        if (!Input.GetMouseButton(0))
        {
            for (int i = 1; i <= menuCount; i++)
            {
                if (ScrollBar.value < ((i * portion) - (portion / 2)) && ScrollBar.value > ((i - 1) * portion))
                {
                    ScrollBar.value = Mathf.Lerp(ScrollBar.value, (i - 1) * portion, 3 * Time.deltaTime);
                }
                if (ScrollBar.value > ((i * portion) - (portion / 2)) && ScrollBar.value < (i * portion))
                {
                    ScrollBar.value = Mathf.Lerp(ScrollBar.value, (i) * portion, 3 * Time.deltaTime);
                }
            }
        }
        //sliding menu control
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !isGoingDown && !isGoingUp)
        {
            //when player presses, save that click position
            oldPosition = Input.mousePosition;
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && !isGoingDown && !isGoingUp)
        {
            //when player lets go, save that position
            newPosition = Input.mousePosition;
        }
        //if both positions aren't null
        if (oldPosition != new Vector2(0f, 0f) && newPosition != new Vector2(0f, 0f))
        {
            if (Vector2.Distance(oldPosition, newPosition) >= (edgeOfScreen.x * 0.2f) &&
                Mathf.Abs(newPosition.y - oldPosition.y) < (edgeOfScreen.y * 0.07f))
            {
                if (oldPosition.x > newPosition.x)
                {
                    switch ((int) sceneContent.transform.localPosition.x)
                    {
                        case 800:
                            playButtonClicked();
                            break;
                        case 0:
                            shopButtonClicked();
                            break;
                    }
                }
                else
                {
                    switch ((int) sceneContent.transform.localPosition.x)
                    {
                        case -800:
                            playButtonClicked();
                            break;
                        case 0:
                            settingsButtonClicked();
                            break;
                    }
                }
                oldPosition = new Vector2(0f, 0f);
                newPosition = new Vector2(0f, 0f);
            }
        }

        if (Input.touchCount == 0)
        {
            oldPosition = new Vector2(0f, 0f);
            newPosition = new Vector2(0f, 0f);
        }

        if (isGoingUp)
        {
            ScrollBar.value = Mathf.Lerp(ScrollBar.value, destination, 5 * Time.deltaTime);
            if (Mathf.Approximately(ScrollBar.value, destination) && isGoingUp)
            {
                isGoingUp = false;
            }
        }
        if (isGoingDown)
        {
            ScrollBar.value = Mathf.Lerp(ScrollBar.value, destination, 5 * Time.deltaTime);
            if (Mathf.Approximately(ScrollBar.value, destination) && isGoingDown)
            {
                isGoingDown = false;
            }
        }
    }

    public void goingUpButtonClick()
    {
        destination = ScrollBar.value + portion;
        isGoingUp = true;
        StartCoroutine(goingUp());
    }

    public void goingDownButtonClick()
    {
        destination = ScrollBar.value - portion;
        isGoingDown = true;
        StartCoroutine(goingDown());
    }

    IEnumerator goingUp()
    {
        yield return new WaitForSeconds(0.8f);
        if (isGoingUp) isGoingUp = false;
    }

    IEnumerator goingDown()
    {
        yield return new WaitForSeconds(0.8f);
        if (isGoingDown) isGoingDown = false;
    }

    IEnumerator bossPopingUp(float timeToWait)
    {
        bossSheep.SetActive(true);
        float timeOfBossAnimation = 3;
        yield return new WaitForSeconds(timeOfBossAnimation);
        bossSheep.SetActive(false);
        yield return new WaitForSeconds(timeToWait);
        StartCoroutine(bossPopingUp(Random.Range(5, 10)));
    }

    public void farm()
    {
        if (PlayerPrefs.GetInt("hearts") > 0)
        {
            PlayerPrefs.SetInt("hearts", PlayerPrefs.GetInt("hearts") - 1);
            LoadingScreenManager.sceneToLoad = 3;
            SceneManager.LoadScene(4);
        }
    }

    public void snow()
    {
        if (PlayerPrefs.GetInt("hearts") > 0)
        {
            PlayerPrefs.SetInt("hearts", PlayerPrefs.GetInt("hearts") - 1);
            LoadingScreenManager.sceneToLoad = 2;
            SceneManager.LoadScene(4);
        }
    }

    public void city()
    {
        if (PlayerPrefs.GetInt("hearts") > 0)
        {
            PlayerPrefs.SetInt("hearts", PlayerPrefs.GetInt("hearts") - 1);
            LoadingScreenManager.sceneToLoad = 5;
            SceneManager.LoadScene(4);
        }
    }

    public void alien()
    {
        LoadingScreenManager.sceneToLoad = 6;
        SceneManager.LoadScene(4);
    }

    public void shopButtonClicked()
    {
        if (sceneContent.transform.localPosition.x > -800)
        {
            StartCoroutine(shop());
        }
    }

    IEnumerator shop()
    {
        Vector3 destination = new Vector3(-800, sceneContent.transform.localPosition.y,
            sceneContent.transform.localPosition.z);
        do
        {
            sceneContent.transform.localPosition = new Vector3(sceneContent.transform.localPosition.x - 50, sceneContent.transform.localPosition.y, sceneContent.transform.localPosition.z);
            yield return new WaitForSeconds(0.01f);
        } while ((int) sceneContent.transform.localPosition.x != (int) destination.x);
    }

    public void playButtonClicked()
    {
        StartCoroutine(play());
    }

    IEnumerator play()
    {
        Vector3 destination = new Vector3(0, sceneContent.transform.localPosition.y,
            sceneContent.transform.localPosition.z);
        bool direction = sceneContent.transform.localPosition.x < destination.x;
        do
        {
            if(direction) sceneContent.transform.localPosition = new Vector3(sceneContent.transform.localPosition.x + 50, sceneContent.transform.localPosition.y, sceneContent.transform.localPosition.z);
            else sceneContent.transform.localPosition = new Vector3(sceneContent.transform.localPosition.x - 50, sceneContent.transform.localPosition.y, sceneContent.transform.localPosition.z);
            yield return new WaitForSeconds(0.01f);
        } while ((int) sceneContent.transform.localPosition.x != (int) destination.x);
    }

    public void settingsButtonClicked()
    {
        if (sceneContent.transform.localPosition.x < 800)
        {
            StartCoroutine(settings());
        }
    }

    IEnumerator settings()
    {
        Vector3 destination = new Vector3(800, sceneContent.transform.localPosition.y,
            sceneContent.transform.localPosition.z);
        do
        {
            sceneContent.transform.localPosition = new Vector3(sceneContent.transform.localPosition.x + 50, sceneContent.transform.localPosition.y, sceneContent.transform.localPosition.z);
            yield return new WaitForSeconds(0.01f);
        } while ((int) sceneContent.transform.localPosition.x != (int) destination.x);
    }

    public void vibration()
    {
        PlayerPrefs.SetString("Vibration", vibrationToggle.GetComponent<Switch>().isOn.ToString());
        if (vibrationToggle.GetComponent<Switch>().isOn && (int) sceneContent.transform.localPosition.x != 0) Handheld.Vibrate();
    }

    public void quit()
    {
        Application.Quit();
    }
    
    public void moreGames()
    {
        Application.OpenURL("https://play.google.com/store/apps/dev?id=5638230701137828274");
    }

    public void deleteProgress()
    {
        
    }

    void dailySpinTime()
    {
        if (PlayerPrefs.GetString("spinTime").Equals("")) PlayerPrefs.SetString("spinTime", DateTime.Now.ToString());
        UTime.GetUtcTimeAsync(OnTimeReceived);
        UTime.HasConnection(connection => print(""));
    }
    private void OnTimeReceived(bool success, string error, DateTime time)
    {
        if (success)
        {       
            rewardSpin(time.ToLocalTime().ToString(), PlayerPrefs.GetString("spinTime"));
        }
        else
        {
            rewardSpin(DateTime.Now.ToString(), PlayerPrefs.GetString("spinTime"));
        }
    }

    void rewardSpin(String now,String spinTime)
    {
        DateTime n = Convert.ToDateTime(now);
        DateTime s = Convert.ToDateTime(spinTime);
        TimeSpan result = n - s;
        if (result.Days >= 1)
        {
            PlayerPrefs.SetString("spinTime", now);
            PlayerPrefs.SetInt("spin", PlayerPrefs.GetInt("spin") + 1);
        }
        else
        {
            spinText.GetComponent<TextMeshProUGUI>().text = "Time left: " + (24 - result.Hours)+"h";
        }
    }
}