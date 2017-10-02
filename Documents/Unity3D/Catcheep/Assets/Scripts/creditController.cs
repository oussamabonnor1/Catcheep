using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class creditController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Escape))
	    {
	        SceneManager.LoadScene("Start");
	    }
    }

    public void facebook()
    {
        Application.OpenURL("fb://page/344400029290078");
    }

    public void siteVisit()
    {
        Application.OpenURL("https://www.jetlight-studio.tk/");
    }

    public void email()
    {
        string t =
            "mailto:jetlightstudio@gmail.com?subject=Your%20Game%20Catcheep";
        Application.OpenURL(t);
    }
    public void googlePlus()
    {
        Application.OpenURL("https://plus.google.com/106078600308560786022");
    }

}
