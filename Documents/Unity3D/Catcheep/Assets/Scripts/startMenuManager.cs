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

    //number of stops is how many images are in the menu
    public int numOfStops;
    //the direction of the swiping
    private bool goingRightDirection;
    //total scroll amount (calculated using vectors and not scroll bar values)
    private float scrollAmount;
    //the value on the scroll bar when scroll started (dah)
    private float currentScrollValue;
    //the destination we need to lerp to when the mouse is released and menu is mid way
    private float desiredDestination;

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
        currentScrollValue = 0f;
        desiredDestination = -1;
    }

    // Update is called once per frame
    void Update()
    {
        float portion = (float) 1 / (numOfStops - 1);
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;
        }
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

                if (goingRightDirection)
                {
                    ScrollBar.value =
                        Mathf.Lerp(ScrollBar.value, currentScrollValue + portion, 0.05f);
                    if(Mathf.Approximately(ScrollBar.value, currentScrollValue + portion)) {
                        currentScrollValue
                        = currentScrollValue + portion;
                        print(currentScrollValue);
                    }
                }
                else
                {
                    ScrollBar.value =
                        Mathf.Lerp(ScrollBar.value, currentScrollValue - portion, 0.05f);
                    if (Mathf.Approximately(ScrollBar.value, currentScrollValue + portion))
                    {
                        currentScrollValue
                            = currentScrollValue - portion;
                        print(currentScrollValue);
                    }
                }
            }
            else
            {
                //TODO fix going back for last case
                //TODO work on current value and menu automatic for some reason (vectors)

                //this is to determine to which portion of the menu we have to go back to
                //made desired destination - 1 at first to not waste frames
                if (desiredDestination == -1)
                {
                    for (int i = 0; i < numOfStops; i++)
                    {
                        //this lines means the value is between two portions of the menu (between 0 and 0.33 for exemple)
                        if (ScrollBar.value < (i + 1) * (portion) &&
                            ScrollBar.value > i * (portion))
                        {
                            //once the destination is assigned, this loop wont be accessed until we reach the destination
                            desiredDestination = i * (portion);
                        }
                    }
                }
                else
                {
                    ScrollBar.value =
                        Mathf.Lerp(ScrollBar.value, desiredDestination, 0.05f);
                    if (Mathf.Approximately(ScrollBar.value, desiredDestination))
                    {
                        desiredDestination = -1;
                        print("destination reached");
                    }
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