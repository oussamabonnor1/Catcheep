using System.Collections;
using System.Collections.Generic;
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
}
