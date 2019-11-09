using UnityEngine;
using UnityEngine.SceneManagement;

public class creditController : MonoBehaviour {
    
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Escape))
	    {
	        SceneManager.LoadScene("Start");
	    }
    }

    public void facebook()
    {
        GameObject.Find("Music Manager").GetComponent<PlayServicesMyVersion>().unlockAchievement(GPGSIds.achievement_social_media);
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

    public void donate()
    {
        Application.OpenURL("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=ZSFVQGH5P7SQS");
    }
}
