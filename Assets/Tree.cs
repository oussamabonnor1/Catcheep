using TMPro;
//#define PHOTON_MULTIPLAYER

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// The Mini Golf Score state.
/// </summary>
public class Tree : MonoBehaviour
{

    #region variables

    ///a reference to the guitext


    /// <summary>
    /// the audio clip to play when the player gets a hole in one.
    /// </summary>
    public AudioClip holeInOneAC;

    /// <summary>
    /// the text to display when the player gets a hole in 1.
    /// </summary>
    public string holeInOneSTR = "Hole in one";

    /// <summary>
    /// The audio clip to play when the player gets a par or worse.
    /// </summary>
    public AudioClip[] strokeACs;

    /// <summary>
    /// A list of strings to display when the player gets a par or worse
    /// </summary>
    public string[] strokeStrings = {"Par",
        "Boogey",
        "Double Boogey",
        "Triple Boogey",
        "Quadruple Boogey",
        "Big Eight!"};
    /// <summary>
    /// The audio clip to play when the player gets birdie or better
    /// </summary>	
    public AudioClip[] negStrokesAC;

    /// <summary>
    /// A list of strings to display when the player gets a birdie or better
    /// </summary>	
    public string[] negStrokes = { "Birdie", "Eagle", "Albatross" };

    //a ref to the game script

    //our total score check.
    //	private int m_totalScore=-1;

    /// <summary>
    /// The total par text
    /// </summary>
    public string totalParSTR = "Total Par ";
    /// <summary>
    /// The total score text
    /// </summary>
    public string totalScoreSTR = "Total Score ";


    public string nextSceneID = "NextScene";
    private bool m_oneTime = false;

    TextMeshProUGUI[] scoreGTs = new TextMeshProUGUI[18];
    TextMeshProUGUI[] parGTs = new TextMeshProUGUI[18];

    public Text total2GT;
    public GameObject totalBackground2;
    public GameObject startButtonGO;
    public GameObject waitGO;
    public GameObject scorePanel;
    public GameObject playState;
    private AudioSource m_audioSource;
    #endregion
    //	private string m_state;
    public void Start()
    {
        m_audioSource = gameObject.GetComponent<AudioSource>();
#if PHOTON_MULTIPLAYER
		if(PhotonNetwork.offlineMode==false)
		{
		startButtonGO.SetActive(false);
		}else{
		total2GT.gameObject.SetActive(false);
		totalBackground2.SetActive (false);

		waitGO.SetActive(false);
		}
#else
        DestroyObject(GameObject.Find("D_Multiplayer"));
        total2GT.gameObject.SetActive(false);
        totalBackground2.SetActive(false);
        waitGO.SetActive(false);

#endif

        //get out gamescript component
        GameObject go = GameObject.FindWithTag("Game");
        if (go)
        {
        }

    }
    
    public void onGameOver(bool vic)
    {
        if (m_tick < 0)
        {
           
            if (scorePanel)
            {
                scorePanel.SetActive(true);
            }
            if (m_audioSource)
            {
                m_audioSource.PlayOneShot(getAudioClip());
            }
            m_tick = 1;
        }
    }
    public void onEveryoneFinishedHole()
    {
        if (waitGO)
            waitGO.SetActive(false);

        if (startButtonGO)
            startButtonGO.SetActive(true);
    }

    private int m_otherID = -1;
    private float m_tick = 0;
    public TextMeshProUGUI resultGT;
    public TextMeshProUGUI totalGT;
    public void Update()
    {
        GameObject go;
        m_tick -= Time.deltaTime;
        for (int i = 0; i < 18; i++)
        {
            string postfix = (i + 1).ToString("00");
            go = GameObject.Find("Hole" + postfix + "gt");

            if (go)
            {
                scoreGTs[i] = new TextMeshProUGUI();
                scoreGTs[i] = go.GetComponent<TextMeshProUGUI>();
                scoreGTs[i].text = "";
            }

            go = GameObject.Find("Par" + postfix);
            if (go)
            {
                parGTs[i] = go.GetComponent<TextMeshProUGUI>();
                parGTs[i].text = "";
            }
        }

        int nomStrokes = 0;
        int totalScore = 0;
        int par = 0;
        int courseIndex = 0;

        //we want to get the other players id.
        int otherID = 0;
        m_otherID = otherID;
        //		Debug.Log ("otherID" + GolfManager.getOtherScore(otherID).ToString());
        totalGT = new TextMeshProUGUI();
        totalGT.text = totalScore.ToString();
        Debug.Log(totalGT.text);

        int handicap = nomStrokes - par;
        string term = getTerm(nomStrokes, handicap);
        resultGT = new TextMeshProUGUI();
        resultGT.text = term;

        showScore();

    }

    public AudioClip getAudioClip()
    {
        int nomStrokes = 0;
        int par = 0;
        int handicap = nomStrokes - par;

        AudioClip rc = null;
        if (nomStrokes == 1)
        {
            rc = holeInOneAC;
        }
        else
        {
            if (handicap > -1 && handicap < strokeACs.Length)
            {
                rc = strokeACs[handicap];
            }
            else
            {
                int invHandicap = (handicap * -1) - 1;
                //				string str = invHandicap.ToString() + " under par";
                if (invHandicap > -1 && invHandicap < negStrokesAC.Length)
                {
                    rc = negStrokesAC[invHandicap];
                }
            }
        }
        return rc;

    }
    public string getTerm(int nomStrokes, int handicap)
    {
        string term = "";
        if (nomStrokes == 1)
        {
            term = holeInOneSTR;
        }
        else
        {
            if (handicap > -1 && handicap < strokeStrings.Length)
            {
                term = strokeStrings[handicap];
            }
            else
            {
                int invHandicap = (handicap * -1) - 1;
                //				string str = invHandicap.ToString() + " under par";
                if (invHandicap > -1 && invHandicap < negStrokes.Length)
                {
                    term = negStrokes[invHandicap];
                }
            }
        }
        Debug.Log(term);
        return term;

    }
    public void showScore()
    {
        float px = transform.position.x;
        float py = transform.position.y;
        Rect r0 = new Rect(px + 0.09f, py + 0.35f, .825f, .3f);
        Rect r1 = r0;
        //GUI.Box(GUIHelper.screenRect(r1),"",backgroundBoxStyle);
        float dx = r0.width / 9f;
        float dy = r0.height / 2f;

        r1.width = dx;
        r1.height = dy;
        int currentHole = 0;
        int n = 1;
        int m = 0;

        for (int i = 0; i < 2; i++)
        {

            for (int j = 0; j < 9; j++)
            {
                int score = 0;
                int par = 0;
                Rect r2 = r1;

                r2.y += 0.05f;
                Debug.Log("score: " + score + " /par: " + par);
                if (n <= currentHole)
                {
                    if (parGTs[m])
                        parGTs[m].text = par.ToString();

                    r2.y += 0.05f;
                    if (scoreGTs[m])
                        scoreGTs[m].text = score.ToString();
                }
                r1.x += (dx * 1.05f);
                n++;
                m++;
            }

            r1.x = r0.x;
            r1.y += dy * 1.5f;
        }
    }

}