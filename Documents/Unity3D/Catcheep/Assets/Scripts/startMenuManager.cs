using System;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class startMenuManager : MonoBehaviour
{
    public GameObject sceneContent;
    public GameObject ScrollBarGameObject;
    private Scrollbar ScrollBar;

    private Vector2 edgeOfScreen;

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
        ScrollBar = ScrollBarGameObject.GetComponent<Scrollbar>();
        edgeOfScreen = new Vector2(Screen.width, Screen.height);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.touchCount == 0)
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
        if (Input.touchCount == 1)
        {
        }
        if (isGoingUp)
        {
            ScrollBar.value = Mathf.Lerp(ScrollBar.value, destination, 0.08f);
            if (Mathf.Approximately(ScrollBar.value, destination) && isGoingUp) isGoingUp = false;
        }
        if (isGoingDown)
        {
            ScrollBar.value = Mathf.Lerp(ScrollBar.value, destination, 0.08f);
            if (Mathf.Approximately(ScrollBar.value, destination) && isGoingDown) isGoingDown = false;
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

    public void shopButtonClicked()
    {
        StartCoroutine(shop());
    }

    IEnumerator shop()
    {
        Vector3 destination = new Vector3(-800, sceneContent.transform.localPosition.y,
            sceneContent.transform.localPosition.z);
        do
        {
            sceneContent.transform.localPosition = Vector3.Lerp(sceneContent.transform.localPosition, destination,
                0.5f);
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
        do
        {
            sceneContent.transform.localPosition = Vector3.Lerp(sceneContent.transform.localPosition, destination,
                0.5f);
            yield return new WaitForSeconds(0.01f);
        } while ((int) sceneContent.transform.localPosition.x != (int) destination.x);
    }

    public void settingsButtonClicked()
    {
        StartCoroutine(settings());
    }

    IEnumerator settings()
    {
        Vector3 destination = new Vector3(800, sceneContent.transform.localPosition.y,
            sceneContent.transform.localPosition.z);
        do
        {
            sceneContent.transform.localPosition = Vector3.Lerp(sceneContent.transform.localPosition, destination,
                0.5f);
            yield return new WaitForSeconds(0.01f);
        } while ((int) sceneContent.transform.localPosition.x != (int) destination.x);
    }

    public void quit()
    {
        Application.Quit();
    }

    public void likePage()
    {
        Application.OpenURL("https://www.facebook.com/JetLightstudio/?ref=bookmarks");
    }
}