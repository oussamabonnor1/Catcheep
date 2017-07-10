using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FbManager : MonoBehaviour
{

    public GameObject loggedIn;
    public GameObject loggedOut;
    public GameObject profilePic;

    private bool logged;
    void Awake()
    {
        logged = false;
        if(!FB.IsLoggedIn) FB.Init(setInit, OnHideUnity);
        else
        {
            logCondtion();
        }
    }

    void setInit()
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("fb is logged");
        }
        else
        {
            Debug.Log("fb is not logged");
        }

        logCondtion();
    }

    void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void FBLogin()
    {
        List<String> permissions = new List<string>();
        permissions.Add("public_profile,publish_actions");

        FB.LogInWithPublishPermissions(permissions, AuthCallBack);
    }

    void AuthCallBack(IResult result)
    {
        if (result.Error != null)
        {
            Debug.Log(result.Error);
        }
        else
        {
            setInit();
        }
    }

    void logCondtion()
    {
        if (SceneManager.GetActiveScene().name == "Start")
        {
            loggedIn.SetActive(FB.IsLoggedIn);
            loggedOut.SetActive(!FB.IsLoggedIn);
            profilePic.SetActive(FB.IsLoggedIn);

            if (FB.IsLoggedIn && !logged)
            {
                logged = true;
                FB.API("/me?fields=first_name", HttpMethod.GET, displayUserName);

                FB.API("/me/picture?type=square&height=130&width=130", HttpMethod.GET, displayPicture);
            }
        }
    }

    void displayUserName(IResult result)
    {
        if (result.Error == null)
        {
            print("welcome "+ result.ResultDictionary["first_name"]);
           // loggedIn.GetComponentInChildren<Text>().text = "welcome " + result.ResultDictionary["first_name"];
        }
        else
        {
            print(result.Error);
        }
    }

    void displayPicture(IGraphResult result)
    {
        if (result.Texture != null)
        {
            profilePic.GetComponent<Image>().sprite = Sprite.Create(result.Texture, new Rect(0,0,128,128),new Vector2());
        }
        else
        {
            loggedIn.GetComponentInChildren<Text>().text = result.Error;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
