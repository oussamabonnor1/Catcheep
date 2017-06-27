using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class startMenuManager : MonoBehaviour
{
    public GameObject ScrollBarGameObject;
    private Scrollbar ScrollBar;

    private Vector2 startPosition;
    private Vector2 endPosition;
    private Vector2 edgeOfScreen;

    private bool goingRightDirection;
    private float scrollAmount;

    // Use this for initialization
    void Start()
    {
        if (ScrollBarGameObject == null) ScrollBarGameObject = GameObject.Find("Scrollbar");
        startPosition = new Vector2(0, 0);
        endPosition = new Vector2(0, 0);
        edgeOfScreen = new Vector2(Screen.width, Screen.height);
        ScrollBar = ScrollBarGameObject.GetComponent<Scrollbar>();

        //basicaly this is true cause we always scroll right first
        goingRightDirection = true;
        scrollAmount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) startPosition = Input.mousePosition;
        if (Input.GetMouseButtonUp(0)) endPosition = Input.mousePosition;

        if (!Input.GetMouseButton(0))
        {
            scrollAmount = Vector2.Distance(startPosition, endPosition);

            if (scrollAmount > edgeOfScreen.x / 6)
            {
                //if start is bigger than end that means the scroll is to the right 
                //(cnt use scroll amount cause it s a positive distance)
                if (startPosition.x - endPosition.x > 0)
                {
                    goingRightDirection = true;
                }
                else
                {
                    goingRightDirection = false;
                }
            }
            if (ScrollBar.value > 0 && ScrollBar.value < 0.3)
            {
                ScrollBar.value =
                    Mathf.Lerp(ScrollBar.value, 0, 0.05f);
            }
            else
            {
                if (ScrollBar.value < 1)
                {
                    ScrollBar.value =
                        Mathf.Lerp(ScrollBar.value, 1, 0.05f);
                }
            }
        }

        if (ScrollBar.value > 1 || ScrollBar.value < 0)
        {
            print("error: " + ScrollBar.value);
        }
    }

    public void farm()
    {
        SceneManager.LoadScene("Farm");
    }

    public void snow()
    {
        SceneManager.LoadScene("snow");
    }

    public void quit()
    {
        Application.Quit();
    }
}