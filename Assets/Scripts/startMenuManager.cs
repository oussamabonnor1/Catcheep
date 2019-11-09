using System;
using System.Collections;
using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class startMenuManager : MonoBehaviour
{
    [Header("in Game objects")] public GameObject quitPanel;
    public GameObject sceneContent;
    public GameObject ScrollBarGameObject;
    public GameObject vibrationToggle;
    public GameObject notificationToggle;
    public GameObject sfxToggle;
    public GameObject musicToggle;
    public GameObject bossSheep;
    public GameObject timeText;
    public GameObject levelText;
    public GameObject heartText;
    public GameObject moneyText;
    public GameObject ProgressPanel;
    public GameObject noHeartsPanel;
    public GameObject rateUsPanel;
    public GameObject donatePanel;
    public GameObject AlienButton;
    private Scrollbar ScrollBar;

    private music musicManager;

    private Vector2 edgeOfScreen;
    private Vector2 oldPosition;
    private Vector2 newPosition;

    [Header("UI")] public int menuCount;
    private float portion;
    private float destination;
    private int value;

    private bool isGoingUp;
    private bool isGoingDown;
    private bool isGoingSideWays;

    // Use this for initialization
    void Start()
    {
        isGoingDown = false;
        isGoingUp = false;
        isGoingSideWays = false;

        portion = (float) 1 / menuCount;
        portion = (float) Math.Round(portion, 2);
        musicManager = GameObject.Find("Music Manager").GetComponent<music>();
        GameObject.Find("Music Manager").GetComponent<PlayServicesMyVersion>().signIn();
        //first time playing: giving the player some hearts 
        if (PlayerPrefs.GetInt("maxHearts") == 0)
        {
            PlayerPrefs.SetInt("maxHearts", 5);
            PlayerPrefs.SetInt("hearts", 3);
        }
        //making sure all objects are affected and not null
        if (ScrollBarGameObject == null) ScrollBarGameObject = GameObject.Find("Horizontal scrollbar");
        if (sceneContent == null) sceneContent = GameObject.Find("Scene content");

        //reminding player of vibration (delete when finished devloping)
        //removing listeners to not call functions on start, then putting them back on

        if (PlayerPrefs.GetString("Vibration") == "True") vibrationToggle.GetComponent<Switch>().isOn = true;
        else vibrationToggle.GetComponent<Switch>().isOn = false;
        if (PlayerPrefs.GetInt("notifications") == 0) notificationToggle.GetComponent<Switch>().isOn = true;
        else notificationToggle.GetComponent<Switch>().isOn = false;
        if (PlayerPrefs.GetInt("SFX") == 0) sfxToggle.GetComponent<Switch>().isOn = true;
        else sfxToggle.GetComponent<Switch>().isOn = false;
        if (PlayerPrefs.GetInt("music") == 0) musicToggle.GetComponent<Switch>().isOn = true;
        else musicToggle.GetComponent<Switch>().isOn = false;

        //Sounds
        musicManager.BackgroundMusic(0);

        //extracting necissary elements for functions
        ScrollBar = ScrollBarGameObject.GetComponent<Scrollbar>();
        edgeOfScreen = new Vector2(Screen.width, Screen.height);

        //starting boss sheep animation a second after the scene loading (explaining "1" as param)
        StartCoroutine(bossPopingUp(Random.Range(5, 10)));

        //rate us panel potentiely showing up
        if (PlayerPrefs.GetInt("rateUs") == 0)
        {
            int a = Random.Range(0, 20);
            if (a == 12) openRateUsPanel();
        }
        //donate panel potentiely showing up
        if (PlayerPrefs.GetInt("donate") == 0 && !rateUsPanel.activeSelf)
        {
            int b = Random.Range(0, 20);
            if (b == 12) openDonatePanel();
        }

        //giving data to texts (as late as possible)
        heartText.GetComponent<TextMeshProUGUI>().text = "x" + PlayerPrefs.GetInt("hearts");
        moneyText.GetComponent<TextMeshProUGUI>().text = "$" + PlayerPrefs.GetInt("money");
        if(PlayerPrefs.GetInt("level") >49) AlienButton.SetActive(false);
        levelText.GetComponent<TextMeshProUGUI>().text = "Level " + (PlayerPrefs.GetInt("level")+1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && !quitPanel.activeSelf)
        {
            musicManager.UISFX(1);
            StartCoroutine(objectOpened(quitPanel));
        }

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
        musicManager.UISFX(0);
        destination = ScrollBar.value + portion;
        isGoingUp = true;
        StartCoroutine(goingUp());
    }

    public void goingDownButtonClick()
    {
        musicManager.UISFX(0);
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
            musicManager.GetComponent<music>().BackgroundMusic(1);
            musicManager.UISFX(0);
            if (PlayerPrefs.GetInt("tuto") == 1) PlayerPrefs.SetInt("tuto", PlayerPrefs.GetInt("tuto") + 1);
            PlayerPrefs.SetInt("hearts", PlayerPrefs.GetInt("hearts") - 1);
            LoadingScreenManager.sceneToLoad = 3;
            SceneManager.LoadScene(4);
        }
        else
        {
            openNoHeartPanel();
        }
    }

    public void snow()
    {
        if (PlayerPrefs.GetInt("hearts") > 0)
        {
            musicManager.GetComponent<music>().BackgroundMusic(2);
            musicManager.UISFX(0);
            PlayerPrefs.SetInt("hearts", PlayerPrefs.GetInt("hearts") - 1);
            LoadingScreenManager.sceneToLoad = 2;
            SceneManager.LoadScene(4);
        }
        else
        {
            openNoHeartPanel();
        }
    }

    public void city()
    {
        if (PlayerPrefs.GetInt("hearts") > 0)
        {
            musicManager.GetComponent<music>().BackgroundMusic(3);
            musicManager.UISFX(0);
            PlayerPrefs.SetInt("hearts", PlayerPrefs.GetInt("hearts") - 1);
            LoadingScreenManager.sceneToLoad = 5;
            SceneManager.LoadScene(4);
        }
        else
        {
            openNoHeartPanel();
        }
    }

    public void alien()
    {
        musicManager.UISFX(0);
        if (PlayerPrefs.GetInt("tuto") == 4) PlayerPrefs.SetInt("tuto", PlayerPrefs.GetInt("tuto") + 1);
        LoadingScreenManager.sceneToLoad = 6;
        SceneManager.LoadScene(4);
    }

    public void shopButtonClicked()
    {
        if (sceneContent.transform.localPosition.x > -800 && !isGoingSideWays)
        {
            musicManager.UISFX(0);
            StartCoroutine(shop());
        }
    }

    IEnumerator shop()
    {
        isGoingSideWays = true;
        Vector3 destination = new Vector3(-800, sceneContent.transform.localPosition.y,
            sceneContent.transform.localPosition.z);
        do
        {
            sceneContent.transform.localPosition = new Vector3(sceneContent.transform.localPosition.x - 50,
                sceneContent.transform.localPosition.y, sceneContent.transform.localPosition.z);
            yield return new WaitForSeconds(0.01f);
        } while ((int) sceneContent.transform.localPosition.x != (int) destination.x);
        isGoingSideWays = false;
    }

    public void playButtonClicked()
    {
        musicManager.UISFX(0);
        StartCoroutine(play());
    }

    IEnumerator play()
    {
        isGoingSideWays = true;
        Vector3 destination = new Vector3(0, sceneContent.transform.localPosition.y,
            sceneContent.transform.localPosition.z);
        bool direction = sceneContent.transform.localPosition.x < destination.x;
        do
        {
            if (direction)
                sceneContent.transform.localPosition = new Vector3(sceneContent.transform.localPosition.x + 50,
                    sceneContent.transform.localPosition.y, sceneContent.transform.localPosition.z);
            else
                sceneContent.transform.localPosition = new Vector3(sceneContent.transform.localPosition.x - 50,
                    sceneContent.transform.localPosition.y, sceneContent.transform.localPosition.z);
            yield return new WaitForSeconds(0.01f);
        } while ((int) sceneContent.transform.localPosition.x != (int) destination.x);
        isGoingSideWays = false;
    }

    public void settingsButtonClicked()
    {
        if (sceneContent.transform.localPosition.x < 800)
        {
            musicManager.UISFX(0);
            StartCoroutine(settings());
        }
    }

    IEnumerator settings()
    {
        Vector3 destination = new Vector3(800, sceneContent.transform.localPosition.y,
            sceneContent.transform.localPosition.z);
        do
        {
            sceneContent.transform.localPosition = new Vector3(sceneContent.transform.localPosition.x + 50,
                sceneContent.transform.localPosition.y, sceneContent.transform.localPosition.z);
            yield return new WaitForSeconds(0.01f);
        } while ((int) sceneContent.transform.localPosition.x != (int) destination.x);
    }

    public void vibration()
    {
        vibrationToggle.GetComponent<Switch>().isOn = !vibrationToggle.GetComponent<Switch>().isOn;
        PlayerPrefs.SetString("Vibration", vibrationToggle.GetComponent<Switch>().isOn.ToString());
        if (vibrationToggle.GetComponent<Switch>().isOn &&
            (int) sceneContent.transform.localPosition.x != 0)
        {
            musicManager.UISFX(1);
            Handheld.Vibrate();
        }
        else
        {
            musicManager.UISFX(2);
        }
    }

    public void quit()
    {
        Application.Quit();
    }

    public void moreGames()
    {
        GameObject.Find("Music Manager").GetComponent<PlayServicesMyVersion>().unlockAchievement(GPGSIds.achievement_seeking_our_games);
        Application.OpenURL("https://play.google.com/store/apps/dev?id=5638230701137828274");
    }

    public void deleteProgress()
    {
        musicManager.UISFX(0);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("maxHearts", 5);
        PlayerPrefs.SetInt("hearts", 3);
        closeDeleteProgress();
        SceneManager.LoadScene(8);
    }

    public void deleteProgressButton()
    {
        musicManager.UISFX(1);
        StartCoroutine(objectOpened(ProgressPanel));
    }
    public void closeDeleteProgress()
    {
        musicManager.UISFX(2);
        StartCoroutine(objectClosed(ProgressPanel));
    }
    void openNoHeartPanel()
    {
        musicManager.UISFX(1);
        StartCoroutine(objectOpened(noHeartsPanel));
    }
    public void closeNoHeartPanel()
    {
        musicManager.UISFX(2);
        StartCoroutine(objectClosed(noHeartsPanel));
    }

    #region rate us panel
    
    public void openRateUsPanel()
    {
        musicManager.UISFX(1);
        StartCoroutine(objectOpened(rateUsPanel));
    }
    public void closeRateUsPanel()
    {
        musicManager.UISFX(2);
        StartCoroutine(objectClosed(rateUsPanel));
    }
    
    public void rateUsYes()
    {
        GameObject.Find("Music Manager").GetComponent<PlayServicesMyVersion>().unlockAchievement(GPGSIds.achievement_thanks_for_rating_us);
        PlayerPrefs.SetInt("rateUs",1);
        closeRateUsPanel();
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.JetLightstudio.Catcheep");
    }

    public void rateUsMaybe()
    {
        closeRateUsPanel();
    }

    public void rateUsNo()
    {
        PlayerPrefs.SetInt("rateUs", 1);
        closeRateUsPanel();
    }
    #endregion

    #region donate panel

    public void openDonatePanel()
    {
        musicManager.UISFX(1);
        StartCoroutine(objectOpened(donatePanel));
    }
    public void closeDonatePanel()
    {
        musicManager.UISFX(2);
        StartCoroutine(objectClosed(donatePanel));
    }
    public void donateYes()
    {
        GameObject.Find("Music Manager").GetComponent<PlayServicesMyVersion>().unlockAchievement(GPGSIds.achievement_donator);
        PlayerPrefs.SetInt("donate", 1);
        closeRateUsPanel();
        Application.OpenURL("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=ZSFVQGH5P7SQS");
    }

    public void donateMaybe()
    {
        closeRateUsPanel();
    }

    public void donateNo()
    {
        PlayerPrefs.SetInt("donate", 1);
        closeRateUsPanel();
    }
    #endregion

    public void notifications()
    {
        if (PlayerPrefs.GetInt("notifications") == 0)
        {
            notificationToggle.GetComponent<Switch>().isOn = false;
            musicManager.UISFX(2);
            PlayerPrefs.SetInt("notifications", 1);
        }
        else
        {
            notificationToggle.GetComponent<Switch>().isOn = true;
            musicManager.UISFX(1);
            PlayerPrefs.SetInt("notifications", 0);
        }
    }
    public void SFX()
    {
        if (PlayerPrefs.GetInt("SFX") == 0)
        {
            sfxToggle.GetComponent<Switch>().isOn = false;
            PlayerPrefs.SetInt("SFX", 1);
        }
        else
        {
            PlayerPrefs.SetInt("SFX", 0);
            sfxToggle.GetComponent<Switch>().isOn = true;
            musicManager.UISFX(1);
        }
    }
     public void music()
    {
        if (PlayerPrefs.GetInt("music") == 0)
        {
            musicToggle.GetComponent<Switch>().isOn = false;
            PlayerPrefs.SetInt("music", 1);
        }
        else
        {
            musicToggle.GetComponent<Switch>().isOn = true;
            PlayerPrefs.SetInt("music", 0);
        }
        musicManager.BackgroundMusic(0);
    }
    
    IEnumerator objectOpened(GameObject objectToOpen)
    {
        objectToOpen.SetActive(true);
        for (int i = 0; i <= 10; i++)
        {
            float a = (float)i / 10;
            objectToOpen.transform.localScale = new Vector3(a, a, 1);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator objectClosed(GameObject objectToOpen)
    {
        for (int i = 10; i >= 0; i--)
        {
            float a = (float)i / 10;
            objectToOpen.transform.localScale = new Vector3(a, a, 1);
            yield return new WaitForSeconds(0.01f);
        }
        objectToOpen.SetActive(false);
    }

    public void closeQuitPanel()
    {
        musicManager.UISFX(2);
        StartCoroutine(objectClosed(quitPanel));
    }
}