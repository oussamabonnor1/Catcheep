using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alienTuto : MonoBehaviour
{

    public GameObject textBox;
    public GameObject imageBox;

    private int i, j;

	// Use this for initialization
	void Start ()
	{
        textBox.SetActive(true);
        imageBox.SetActive(true);
        showData();
	    i = j = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void showData()
    {
        textBox.transform.GetChild(i).gameObject.SetActive(true);
        imageBox.transform.GetChild(j).gameObject.SetActive(true);
    }

    public void forward()
    {
        killEverything();
        if (i < 10)
        {
            i++;
            if (j != 4 && j != 7)
            {
                j++;
            }
            else
            {
                if (i == 7) j++;
            }
            showData();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    public void back()
    {
        killEverything();
        if (i > 0)
        {
            i--;
            if (j != 4 && j != 7)
            {
                j--;
            }
            else
            {
                if (i == 4) j--;
            }
            showData();
        }
    }
    void killEverything()
    {
        
        for (int i = 0; i < textBox.transform.childCount; i++)
        {
            textBox.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < imageBox.transform.childCount; i++)
        {
            imageBox.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
