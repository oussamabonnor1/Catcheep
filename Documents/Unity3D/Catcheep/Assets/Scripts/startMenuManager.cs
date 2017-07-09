﻿using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class startMenuManager : MonoBehaviour
{
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
        if (ScrollBarGameObject == null) ScrollBarGameObject = GameObject.Find("Scrollbar");
        ScrollBar = ScrollBarGameObject.GetComponent<Scrollbar>();
        edgeOfScreen = new Vector2(Screen.width, Screen.height);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.touchCount == 0)
        /* if (!Input.GetMouseButton(0))
         {

             for (int i = 1; i <= menuCount; i++)
             {
                 if (ScrollBar.value < ((i * portion) - portion) && ScrollBar.value > ((i - 1) * portion))
                 {
                     ScrollBar.value = Mathf.Lerp(ScrollBar.value, (i - 1) * portion, 0.05f);
                 }
                 if (ScrollBar.value > ((i * portion) - portion) && ScrollBar.value < (i * portion))
                 {
                     ScrollBar.value = Mathf.Lerp(ScrollBar.value, (i) * portion, 0.05f);
                 }
             }
         }
         if (Input.touchCount == 1)
         {
         }*/
        if (isGoingUp)
        {
            ScrollBar.value = Mathf.Lerp(ScrollBar.value, destination, 0.05f);
            
        }
        if (isGoingDown)
        {
            ScrollBar.value = Mathf.Lerp(ScrollBar.value, destination, 0.05f);
           
        }
    }

    public void goingUp()
    {
        destination = ScrollBar.value - (ScrollBar.value % portion) + portion;
        print("to : " + destination + " / " + ScrollBar.value + " / " + portion + " / " + (ScrollBar.value % portion));
        isGoingUp = true;
    }

    public void goingDown()
    {
        destination = ScrollBar.value + (portion - (ScrollBar.value % portion)) - portion;
        isGoingDown = true;
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