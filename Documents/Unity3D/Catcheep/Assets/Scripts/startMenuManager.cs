using System;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.iOS;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class startMenuManager : MonoBehaviour
{
    public GameObject sceneContent;
    public GameObject ScrollBarGameObject;
    public GameObject vibrationToggle;
    private Scrollbar ScrollBar;

    private Vector2 edgeOfScreen;
    private Vector2 oldPosition;
    private Vector2 newPosition;


    public int menuCount;
    private float portion;
    private float destination;
    private int value;

    private bool isGoingUp;
    private bool isGoingDown;


    // Use this for initialization
    void Start()
    {
        isGoingDown = false;
        isGoingUp = false;

        portion = (float) 1 / menuCount;
        portion = (float) Math.Round(portion, 2);
        if (ScrollBarGameObject == null) ScrollBarGameObject = GameObject.Find("Horizontal scrollbar");
        if (sceneContent == null) sceneContent = GameObject.Find("Scene content");
        if (PlayerPrefs.GetString("Vibration") == "True") vibrationToggle.GetComponent<Toggle>().isOn = true;
        else vibrationToggle.GetComponent<Toggle>().isOn = false;
        ScrollBar = ScrollBarGameObject.GetComponent<Scrollbar>();
        edgeOfScreen = new Vector2(Screen.width, Screen.height);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!Input.GetMouseButton(0))
        {
            for (int i = 1; i <= menuCount; i++)
            {
                if (ScrollBar.value < ((i * portion) - (portion / 2)) && ScrollBar.value > ((i - 1) * portion))
                {
                    ScrollBar.value = Mathf.Lerp(ScrollBar.value, (i - 1) * portion, 0.05f);
                }
                if (ScrollBar.value > ((i * portion) - (portion / 2)) && ScrollBar.value < (i * portion))
                {
                    ScrollBar.value = Mathf.Lerp(ScrollBar.value, (i) * portion, 0.05f);
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
            ScrollBar.value = Mathf.Lerp(ScrollBar.value, destination, 0.08f);
            if (Mathf.Approximately(ScrollBar.value, destination) && isGoingUp)
            {
                isGoingUp = false;
            }
        }
        if (isGoingDown)
        {
            ScrollBar.value = Mathf.Lerp(ScrollBar.value, destination, 0.08f);
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


    public void farm()
    {
        LoadingScreenManager.sceneToLoad = 3;
        SceneManager.LoadScene(4);
    }

    public void snow()
    {
        LoadingScreenManager.sceneToLoad = 2;
        SceneManager.LoadScene(4);
        //SceneManager.LoadScene("snow");
    }

    public void city()
    {
        LoadingScreenManager.sceneToLoad = 5;
        SceneManager.LoadScene(4);
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
        if (sceneContent.transform.localPosition.x > -800)
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
        PlayerPrefs.SetString("Vibration", vibrationToggle.GetComponent<Toggle>().isOn.ToString());
        if (vibrationToggle.GetComponent<Toggle>().isOn && (int) sceneContent.transform.localPosition.x != 0) Handheld.Vibrate();
    }

    public void quit()
    {
        Application.Quit();
    }

    public void likePage()
    {
        Application.OpenURL("https://www.facebook.com/JetLightstudio/?ref=bookmarks");
    }

    public void ShowAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }
}