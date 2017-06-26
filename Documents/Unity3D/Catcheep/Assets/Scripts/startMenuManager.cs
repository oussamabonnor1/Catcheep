using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class startMenuManager : MonoBehaviour
{

    public GameObject ScrollRect;


	// Use this for initialization
	void Start () {
		if( ScrollRect == null) ScrollRect = GameObject.Find("Scrollbar");
	}
	
	// Update is called once per frame
	void Update () {
	    if (!Input.GetMouseButton(0))
	    {
	        GameObject itemToScroll = GameObject.Find("seperator").transform.GetChild(1).gameObject;
            
            if (itemToScroll.transform.localPosition.x > 0 && ScrollRect.GetComponent<Scrollbar>().value > 0)
            {
	            ScrollRect.GetComponent<Scrollbar>().value =
	                Mathf.Lerp(ScrollRect.GetComponent<Scrollbar>().value, 0, 0.05f);
	        }
	        else{
                if (ScrollRect.GetComponent<Scrollbar>().value > 0)
                {
                    ScrollRect.GetComponent<Scrollbar>().value =
                        Mathf.Lerp(ScrollRect.GetComponent<Scrollbar>().value, 1, 0.05f);
                }
            }
        }

	    if (ScrollRect.GetComponent<Scrollbar>().value > 1 || ScrollRect.GetComponent<Scrollbar>().value < 0)
	    {
	        print("error: " + ScrollRect.GetComponent<Scrollbar>().value);
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
