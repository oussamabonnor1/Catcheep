using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class startMenuManager : MonoBehaviour
{
    public GameObject ScrollBarGameObject;
    private Scrollbar ScrollBar;

    private Vector2 edgeOfScreen;

    public int menuCount;
    // Use this for initialization
    void Start()
    {
        if (ScrollBarGameObject == null) ScrollBarGameObject = GameObject.Find("Scrollbar");
        ScrollBar = ScrollBarGameObject.GetComponent<Scrollbar>();
        edgeOfScreen = new Vector2(Screen.width, Screen.height);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
        {
            float portion = (float) 1 / menuCount; 
            for (int i = 1; i <= menuCount; i++)
            {
                
                    if (ScrollBar.value < (i * (portion / 2) ) && ScrollBar.value > ((i - 1) * portion))
                    {
                        ScrollBar.value = Mathf.Lerp(ScrollBar.value, (i - 1) * portion, 0.05f);
                    }
                    if (ScrollBar.value > (i * (portion / 2)) && ScrollBar.value < (i * portion))
                    {
                        ScrollBar.value = Mathf.Lerp(ScrollBar.value, (i) * portion, 0.05f);
                    }
                
            }
        }
        if (Input.touchCount == 1)
        {

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