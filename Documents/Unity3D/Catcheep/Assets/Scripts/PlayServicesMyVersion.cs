using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class PlayServicesMyVersion : MonoBehaviour {

	// Use this for initialization
	void Start () {
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration();
	    PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.InitializeInstance(config);
	    PlayGamesPlatform.Activate();
	    GooglePlayGames.OurUtils.Logger.DebugLogEnabled = true;
	    signIn();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SignInCallback(bool success)
    {
        if (success)
        {
            print("yey");
        }
    }

    void signIn()
    {
        if (!Social.localUser.authenticated)
        {
            // Sign in with Play Game Services, showing the consent dialog
            // by setting the second parameter to isSilent=false.
            print("call back...");
            Social.localUser.Authenticate(SignInCallback);
        }
        else
        {
            print("error");
        }

    }
    

    public void unlockAchievement(string id)
    {
        Social.ReportProgress(id, 100, succes => { print("achievement unlocked !"); });
    }

    public void unlockPartialAchievement(string id, int pourcentage)
    {
        Social.ReportProgress(id,pourcentage,succes => {print("achievement unlocked partialy !");});
    }

    public void showAchievementUI()
    {
        Social.ShowAchievementsUI();
    }

    public void addValueToLeaderbord(string id, int score)
    {
        Social.ReportScore(score, id, succes => {print("score added");});
    }

    public void showLeaderbordUI()
    {
        Social.ShowLeaderboardUI();
    }
}
