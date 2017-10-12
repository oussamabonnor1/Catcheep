using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
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
	    i = j = 0;
        showData();
	}

    void OnEnable()
    {
        i = j = 0;
        showData();
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
            if ((i >= 4 && i < 6) || i == 10)
            {
                print("stady");
            }
            else
            {
                j++;
            }
            showData();
        }
        else
        {
            gameObject.SetActive(false);
        }
        GameObject.Find("Music Manager").GetComponent<music>().UISFX(0);
    }
    public void back()
    {
        if (i > 0)
        {
            killEverything();
            i--;
            switch (i)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                    j = i;
                    break;
                case 5:
                    j = 4;
                    break;
                case 6:
                case 7:
                case 8:
                case 9:
                    j = i - 2;
                    break;
                case 10:
                    j = 7;
                    break;
                default:
                    i = 0;
                    j = 0;
                    break;
            }
            showData();
        }
        GameObject.Find("Music Manager").GetComponent<music>().UISFX(0);
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
