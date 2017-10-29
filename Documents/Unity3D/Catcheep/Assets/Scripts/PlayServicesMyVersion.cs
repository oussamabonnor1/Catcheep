using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine;

public class PlayServicesMyVersion : MonoBehaviour {

	// Use this for initialization
	void Start () {
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
	    PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.InitializeInstance(config);
	    PlayGamesPlatform.Activate();
	    GooglePlayGames.OurUtils.Logger.DebugLogEnabled = true;
	}

    public void SignInCallback(bool success)
    {
        if (success)
        {
            print("yey");
        }
        else
        {
            print("err");
        }
    }

    public void signIn()
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
            print("already In");
        }

    }
    

    public void unlockAchievement(string id)
    {
        Social.ReportProgress(id, 100, succes => { print("achievement unlocked !"); });
    }

    public void unlockPartialAchievement(string id, double pourcentage)
    {
        Social.ReportProgress(id,pourcentage,succes => {print("achievement partialy unlocked !");});
    }

    public bool showAchievementUI()
    {
        Social.ShowAchievementsUI();
        return Social.localUser.authenticated;
    }

    public void addValueToLeaderbord(string id, int score)
    {
        Social.ReportScore(score, id, succes => {print("score added to leaderbord");});
    }

    public bool showLeaderbordUI()
    {
        Social.ShowLeaderboardUI();
        return Social.localUser.authenticated;
    }

    public void showQuest()
    {
    }
}
