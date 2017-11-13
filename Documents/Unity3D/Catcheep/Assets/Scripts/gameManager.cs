using System.Collections;
using System.Globalization;
using Assets.Scripts;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class gameManager : MonoBehaviour
{
    [Header("Sheep related objects")] public GameObject tutoSheep;
    public GameObject[] sheeps;
    public GameObject[] formationSheepys;
    public AudioClip[] sheepSound;

    [Header("UI components")] public GameObject background;
    public GameObject menuBackground;
    public GameObject backgroundOfTrees;
    public GameObject finishPanel;
    public GameObject flare;
    public GameObject helpToolsPlate;
    public GameObject pausePanel;
    public GameObject BadViewGameObject;
    public Sprite[] sfxImages;
    public Sprite[] musicImages;

    public static bool gameOver;
    private Vector3 edgeOfScreen;
    private GameObject scoreText;

    public static int totalSheepsCaught;
    public static int combo;
    public static int score;
    public static int sheepyCaught;
    public static int snowSheepyCaught;
    public static int citySheepyCaught;
    public static int flashCaught;
    public static int blackyCaught;
    public static int snowBlackyCaught;
    public static int cityBlackyCaught;
    public static bool catchedSomething;
    public static bool pukeShowed;
    public bool tutorielFinished;

    private int originalScore;
    private int finishPanelSafetyNet; //this var is here to make sure we dont wait for more than 3 seconds
    private music musicManager;


    // Use this for initialization
    void Start()
    {
        //intialising the vars of caught sheeps (huge problem happens on finish panel if this isnt here)
        sheepyCaught = 0;
        blackyCaught = 0;
        flashCaught = 0;
        snowSheepyCaught = 0;
        snowBlackyCaught = 0;
        citySheepyCaught = 0;
        cityBlackyCaught = 0;
        finishPanelSafetyNet = 0;
        tutorielFinished = false;
        catchedSomething = false;
        pukeShowed = false;
        totalSheepsCaught = PlayerPrefs.GetInt("sheepy");
        combo = 0;
        originalScore = PlayerPrefs.GetInt("money");
        score = PlayerPrefs.GetInt("money");
        musicManager = GameObject.Find("Music Manager").GetComponent<music>();

        if (background == null)
        {
            background = GameObject.Find("background");
            ResizeBackground(background);
            ResizeBackground(menuBackground);
        }
        if (backgroundOfTrees == null && GameObject.Find("Trees") != null)
        {
            backgroundOfTrees = GameObject.Find("Trees");
            ResizeBackground(backgroundOfTrees);
        }

        scoreText = GameObject.Find("score");
        cashUpdate(score);
        GameObject.Find("sheeps caught").GetComponent<TextMeshProUGUI>().text = "x " + totalSheepsCaught;

        //edge of screen is a vector3 that holds the screens width (can't get it directly cause of Screen/World point difference)
        Vector3 helpToolsPlateWidth = helpToolsPlate.GetComponent<RectTransform>().rect.size;
        edgeOfScreen =
            Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - helpToolsPlateWidth.x, Screen.height, 0f));
        gameOver = false;
        StartCoroutine(animatedBackgrounds(background, menuBackground));
    }

    IEnumerator animatedBackgrounds(GameObject a, GameObject b)
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 12; i++)
        {
            Vector3 v1 = a.transform.position;
            a.transform.position = new Vector3(v1.x, v1.y + 1, v1.z);
            Vector3 v2 = b.transform.position;
            b.transform.position = new Vector3(v2.x, v2.y + 1, v2.z);
            yield return new WaitForSeconds(0.03f);
        }
        if (backgroundOfTrees != null) backgroundOfTrees.SetActive(true);
        if (PlayerPrefs.GetInt("tuto") == 2 || PlayerPrefs.GetInt("tuto") == 3)
        {
            Instantiate(tutoSheep);
        }
        else
        {
            StartCoroutine(sheepSpawner());
        }
    }

    void Update()
    {
        if (tutorielFinished)
        {
            StartCoroutine(sheepSpawner());
            tutorielFinished = false;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!catchedSomething && combo > 0)
            {
                combo = 0;
                StartCoroutine(flareMaker(0.45f));
                StartCoroutine(GameObject.Find("Help Manager").GetComponent<helpManager>().errorCall());
                if (PlayerPrefs.GetString("Vibration") == "True") Handheld.Vibrate();
            }

            if (catchedSomething)
            {
                catchedSomething = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pausePanel.gameObject.activeSelf)
            {
                musicManager.UISFX(1);
                PlayerPrefs.SetInt("sheepy", totalSheepsCaught);
                pausePanel.SetActive(true);
                pausePanel.transform.GetChild(4).GetComponent<Image>().sprite =
                    musicImages[PlayerPrefs.GetInt("music")];
                pausePanel.transform.GetChild(5).GetComponent<Image>().sprite =
                    sfxImages[PlayerPrefs.GetInt("SFX")];
                Time.timeScale = 0;
            }
            else
            {
                ContinueGame();
            }
        }
    }

    public void SFX()
    {
        if (PlayerPrefs.GetInt("SFX") == 0)
        {
            PlayerPrefs.SetInt("SFX", 1);
        }
        else
        {
            PlayerPrefs.SetInt("SFX", 0);
            musicManager.UISFX(1);
        }
        pausePanel.transform.GetChild(5).GetComponent<Image>().sprite =
            sfxImages[PlayerPrefs.GetInt("SFX")];
    }

    public void music()
    {
        if (PlayerPrefs.GetInt("music") == 0)
        {
            PlayerPrefs.SetInt("music", 1);
        }
        else
        {
            PlayerPrefs.SetInt("music", 0);
        }
        pausePanel.transform.GetChild(4).GetComponent<Image>().sprite =
            musicImages[PlayerPrefs.GetInt("music")];
        if (SceneManager.GetActiveScene().name.Equals("Farm"))
        {
            musicManager.BackgroundMusic(1);
        }
        if (SceneManager.GetActiveScene().name.Equals("Snow"))
        {
            musicManager.BackgroundMusic(2);
        }
        if (SceneManager.GetActiveScene().name.Equals("City"))
        {
            musicManager.BackgroundMusic(3);
        }
    }

    void ResizeBackground(GameObject background)
    {
        SpriteRenderer sr = background.GetComponent<SpriteRenderer>();

        if (sr == null) return;

        transform.localScale = new Vector3(1, 1, 1);

        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        Vector3 xWidth = background.transform.localScale;
        xWidth.x = worldScreenWidth / width;
        background.transform.localScale = xWidth;

        Vector3 yHeight = background.transform.localScale;
        yHeight.y = worldScreenHeight / height;
        background.transform.localScale = yHeight;
    }

    //THE MAIN SHEEP CREATOR, DO NOT ULTER UNLESS YOU UNDERSTAND THE CODE 100% 
    //this bit is in relation with many scripts
    public IEnumerator sheepSpawner()
    {
        //yield return new WaitForSeconds(1f);
        int size = sheeps.Length - 1;
        float taux = (float) PlayerPrefs.GetInt("level") / 100;
        while (!gameOver)
        {
            Collider2D collisions = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y),
                new Vector2(edgeOfScreen.x, 2.5f), 0f);

            if (collisions == null)
            {
                if (taux < 0.9f) taux += 0.05f;
                int i = Random.Range(-6, 10);
                if (PlayerPrefs.GetInt("level") < 5) i = Random.Range(-5, 1);
                else if (PlayerPrefs.GetInt("level") < 7) i = Random.Range(-5, 2);
                else if (PlayerPrefs.GetInt("level") < 10) i = Random.Range(-4, 9);
                switch (i)
                {
                    case -6:
                        //making sure boss sheepy is rare af
                        int j = Random.Range(0, 30);
                        if (j == 1) StartCoroutine(bossSheepy());
                        break;
                    case -5:
                        oneSheepyRandom(1);
                        break;
                    case -4:
                        oneSheepyRandom(2);
                        break;
                    case -3:
                        oneSheepyRandom(0);
                        break;
                    case -2:
                        oneSheepyRandom(0);
                        break;
                    case -1:
                        oneSheepyRandom(4);
                        break;
                    case 0:
                        oneSheepyRandom(3);
                        //yield return new WaitForSeconds(2 - taux);
                        break;

                    case 1:
                        int num = Random.Range(1, 4);
                        twoSheepyHorizontalManySet(num, Random.Range(0, size));
                        //yield return new WaitForSeconds(3 - taux);
                        break;
                    case 2:
                        threeSheepySlidingRightUpDown(1, Random.Range(0, size));
                        //yield return new WaitForSeconds(num - taux);
                        break;

                    case 3:
                        SheepyHorizontalpartScreen(Random.Range(0, size), Random.Range(2, 5));
                        //yield return new WaitForSeconds(3 - taux);
                        break;

                    case 4:
                        vFormeSheepy(Random.Range(2, 4), Random.Range(0, size));
                        yield return new WaitForSeconds(2f - taux);
                        break;

                    case 5:
                        fourSheepyTriangleLookingDownUp(-1, Random.Range(0, size));
                        //yield return new WaitForSeconds(3 - taux);
                        break;

                    case 6:
                        slidingSheepy(Random.Range(2, 5), Random.Range(0, size));
                        //yield return new WaitForSeconds(3 - taux);
                        break;

                    case 7:
                        threeSheepyHorizontalFullScreen(Random.Range(0, size));
                        //yield return new WaitForSeconds(3 - taux);
                        break;

                    case 8:
                        fourSheepyTriangleLookingDownUp(1, Random.Range(0, size));
                        //yield return new WaitForSeconds(3 - taux);
                        break;
                    case 9:
                        preMadeFormation(Random.Range(0, formationSheepys.Length));
                        //yield return new WaitForSeconds();
                        break;
                }
            }
            else
            {
                yield return new WaitForSeconds(1f - taux);
            }
        }

        if (gameOver)
        {
            StartCoroutine(finishPanelShowing());
        }
    }

    IEnumerator finishPanelShowing()
    {
        ++finishPanelSafetyNet;
        yield return new WaitForSeconds(1f);
        if ((!GameObject.FindGameObjectWithTag("sheepy") && !GameObject.FindGameObjectWithTag("blacky"))
            || finishPanelSafetyNet > 2)
        {
            musicManager.GetComponent<music>().UISFX(4);
            PlayServicesMyVersion psvm = GameObject.Find("Music Manager").GetComponent<PlayServicesMyVersion>();
            psvm.addValueToLeaderbord(GPGSIds.leaderboard_most_money_earned,PlayerPrefs.GetInt("mostMoney"));
            psvm.addValueToLeaderbord(GPGSIds.leaderboard_most_sheep_caught,PlayerPrefs.GetInt("mostSheepy"));
            psvm.unlockPartialAchievement(GPGSIds.achievement_car_crasher, (double)PlayerPrefs.GetInt("carsCrushed") / 50);
            psvm.unlockPartialAchievement(GPGSIds.achievement_car_crasher, (double)PlayerPrefs.GetInt("snowCrushed") / 50);
            psvm.addValueToLeaderbord(GPGSIds.leaderboard_obstacles_destroyed, PlayerPrefs.GetInt("snowCrushed") + PlayerPrefs.GetInt("carsCrushed"));
            psvm.unlockPartialAchievement(GPGSIds.achievement_catch_1000_sheeps, (double)PlayerPrefs.GetInt("mostSheepy") / 1000);
            psvm.unlockPartialAchievement(GPGSIds.achievement_catch_2500_sheeps, (double)PlayerPrefs.GetInt("mostSheepy") / 2500);
            psvm.unlockPartialAchievement(GPGSIds.achievement_catch_5000_sheeps, (double)PlayerPrefs.GetInt("mostSheepy") / 5000);
            psvm.unlockPartialAchievement(GPGSIds.achievement_catch_7500_sheeps, (double)PlayerPrefs.GetInt("mostSheepy") / 7500);
            psvm.unlockPartialAchievement(GPGSIds.achievement_catche_10000_sheeps, (double)PlayerPrefs.GetInt("mostSheepy") / 10000);

            PlayerPrefs.SetInt("sheepy", totalSheepsCaught);
            //i wish i was focused enough to find a more beautiful way of assigning values but...
            //it's been a long 3 months working on this 'game' and i honestly jst wanna get it over with.
            //update: it s been 5 month now and it s publishing time, i hope it was worth it !
            StartCoroutine(objectOpened(finishPanel));
            finishPanel.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text =
                "+" + (score - originalScore);
            if (SceneManager.GetActiveScene().name.Equals("Farm"))
            {
                finishPanel.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text =
                    "+" + sheepyCaught;
                finishPanel.transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>().text =
                    "+" + blackyCaught;
            }
            if (SceneManager.GetActiveScene().name.Equals("Snow"))
            {
                finishPanel.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text =
                    "+" + snowSheepyCaught;
                finishPanel.transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>().text =
                    "+" + snowBlackyCaught;
            }
            if (SceneManager.GetActiveScene().name.Equals("City"))
            {
                finishPanel.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text =
                    "+" + citySheepyCaught;
                finishPanel.transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>().text =
                    "+" + cityBlackyCaught;
            }
            finishPanel.transform.GetChild(4).GetComponentInChildren<TextMeshProUGUI>().text =
                "+" + flashCaught;
        }
        else
        {
            StartCoroutine(finishPanelShowing());
        }
    }

    public void quit()
    {
        Time.timeScale = 1;
        PlayerPrefs.SetInt("sheepy", totalSheepsCaught);
        SceneManager.LoadScene(1);
    }

    public void ContinueGame()
    {
        musicManager.GetComponent<music>().UISFX(2);
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void restart()
    {
        if (PlayerPrefs.GetInt("hearts") > 0)
        {
            musicManager.GetComponent<music>().UISFX(0);
            PlayerPrefs.SetInt("hearts", PlayerPrefs.GetInt("hearts") - 1);
            Time.timeScale = 1;
            PlayerPrefs.SetInt("sheepy", totalSheepsCaught);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    #region sheep formations
    //one sheepy formations
    void oneSheepyRandom(int index)
    {
        float edges = edgeOfScreen.x - (sheeps[index].GetComponent<SpriteRenderer>().sprite.bounds.extents).x;
        float xPosition = Random.Range(-edges, edges);
        Vector3 spawnPosition = new Vector3(xPosition, transform.position.y, transform.position.z);
        Instantiate(sheeps[index], spawnPosition, Quaternion.identity);
    }

    void oneSheepyChosen(float xPosition, float deltaYPosition, int index)
    {
        Vector3 spawnPosition = new Vector3(xPosition, transform.position.y + deltaYPosition, transform.position.z);
        Instantiate(sheeps[index], spawnPosition, Quaternion.identity);
    }

    IEnumerator bossSheepy()
    {
        Vector2 spawnPosition = new Vector2(Random.Range(-edgeOfScreen.x, edgeOfScreen.x),
            Random.Range(-edgeOfScreen.y / 2, edgeOfScreen.y / 2));
        for (int i = 0; i < 3; i++)
        {
            Collider2D collisions = Physics2D.OverlapBox(spawnPosition, new Vector2(2.5f, 2.5f), 0f);

            if (collisions)
            {
                GameObject boss = Instantiate(sheeps[sheeps.Length - 1], spawnPosition, Quaternion.identity);
                yield return new WaitForSeconds(1.5f);
                boss.GetComponent<sheepDestroyer>().Destruction();
                break;
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }
    //end of one sheepy formations

    //two sheepy formations:
    void twoSheepyHorizontalManySet(int times, int index)
    {
        float sheepyWidth = sheeps[index].GetComponent<SpriteRenderer>().sprite.bounds.extents.x;
        float edges = edgeOfScreen.x - sheepyWidth;
        //gap is between a small value of my choice and the total with of screen - the width of number of sheeps together (2 in this case)
        float gap = Random.Range(edgeOfScreen.x * 0.5f, (edgeOfScreen.x * 2) - (sheepyWidth * 2));
        // i used edges - gap cause i instantiate from the left and i m making sure the sheep wont go overboard 
        float xPosition = Random.Range(-edges, edges - gap);

        for (int i = 0; i < times; i++)
        {
            oneSheepyChosen(xPosition, i * gap, index);
            oneSheepyChosen(xPosition + gap, i * gap, index);
        }
    }
    //ende of two sheepy formations;

    //three sheepy formations:
    void threeSheepyHorizontalFullScreen(int index)
    {
        float sheepyWidth = (sheeps[index].GetComponent<SpriteRenderer>().bounds.size).x;
        float xPosition = -edgeOfScreen.x + sheepyWidth / 2;
        //used one sheepy width in gap cause that s how many sheepys are in between (hate this code)
        float gap = ((edgeOfScreen.x * 2) - (sheepyWidth)) / 2;
        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnPositionVector3 = new Vector3(xPosition + (gap * i), transform.position.y,
                transform.position.z);
            Instantiate(sheeps[index], spawnPositionVector3, Quaternion.identity);
        }
    }

    void threeSheepySlidingRightUpDown(int direction, int index)
    {
        // positioning the first (left = 1 or right = -1 depending on the direction) sheepy
        float sheepyWidth = (sheeps[index].GetComponent<SpriteRenderer>().bounds.size).x;
        // edges is the max left position value that a sheepy can have (screen width + concidering the sheepy width)
        float edges = direction * (-edgeOfScreen.x + sheepyWidth);
        //choosing half of screen where to position first sheepy depending on direction
        float xPosition = Random.Range(edges, 0f - direction * (2 * sheepyWidth));

        // finding out how much gap should be between the sheepys (randomly)
        float gap = Random.Range(edgeOfScreen.x * 0.55f, edgeOfScreen.x * 0.7f);

        //instantiating the sheepys
        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnPositionVector3 = new Vector3(xPosition + direction * (gap * i),
                transform.position.y + (gap * i), transform.position.z);
            Instantiate(sheeps[index], spawnPositionVector3, Quaternion.identity);
        }
    }
    //ende of three sheepy formations;

    //four sheepy formations:
    void fourSheepyTriangleLookingDownUp(int direction, int index)
    {
        //some paramtres that we need to instantite correctly
        float sheepyWidth = (sheeps[index].GetComponent<SpriteRenderer>().bounds.extents).x;
        float edges = -edgeOfScreen.x + sheepyWidth;
        float xPosition = Random.Range(edges, 0f - (2 * sheepyWidth));
        float gap = Random.Range(edgeOfScreen.x * 0.5f, edgeOfScreen.x * 0.7f);

        //saving the position of the second sheepy to creat the triangle
        Vector3 secondSheepy = new Vector3();

        //instantiating the sheepys
        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnPositionVector3 =
                new Vector3(xPosition + (gap * i), transform.position.y, transform.position.z);
            Instantiate(sheeps[index], spawnPositionVector3, Quaternion.identity);
            if (i == 1) secondSheepy = spawnPositionVector3;
        }

        oneSheepyChosen(secondSheepy.x, direction * (gap + 0.3f), index);
    }
    //ende of two sheepy formations;

    //V forme sheepy
    void vFormeSheepy(int number, int index)
    {
        float sheepyWidth = (sheeps[index].GetComponent<SpriteRenderer>().bounds.extents).x;
        //gap is between a small value of my choice and the total with of screen - the width of number of sheeps together (2 in this case)
        float gap = Random.Range(edgeOfScreen.x * 0.8f, ((edgeOfScreen.x) - (sheepyWidth * (number - 1))));
        gap /= number - 1;

        // choosing half of screen where to position first sheepy depending on direction
        float xPosition = 0f; //Random.Range(edges, -edges);

        //instantiating the sheepys
        for (int i = 0; i < number; i++)
        {
            oneSheepyChosen(xPosition + (gap * i), transform.position.y + (gap * i), index);
        }

        //instantiating the sheepys
        for (int i = 1; i < number; i++)
        {
            oneSheepyChosen(xPosition - (gap * i), transform.position.y + (gap * i), index);
        }
    }
    //end of V frome sheepy sheepy

    //sliding forme sheepy (choice above 1 always)
    void slidingSheepy(int number, int index)
    {
        //info needed (very similar to the sheepyPartScreen function
        float sheepyWidth = (sheeps[index].GetComponent<SpriteRenderer>().bounds.extents).x;
        float edges = edgeOfScreen.x - (sheepyWidth);
        //gap is between a small value of my choice and the total with of screen - the width of number of sheeps together (2 in this case)
        float gap = Random.Range(edgeOfScreen.x * 0.8f, ((edgeOfScreen.x * 2) - (sheepyWidth * (number - 1))));
        //for more info go to sheepyHorizontalPartScreen function (line 224)
        float xPosition = -edges + Random.Range(0f, (edges * 2) - gap);
        // gap is total gap (previous line) devided by number of gaps in total
        gap /= number - 1;

        //instantiating the sheepys
        for (int i = 0; i < number; i++)
        {
            oneSheepyChosen(xPosition + (gap * i), gap * i, index);
        }
    }

    void SheepyHorizontalpartScreen(int index, int num)
    {
        // calculating needed info
        float sheepyWidth = (sheeps[index].GetComponent<SpriteRenderer>().bounds.extents).x;
        float edges = edgeOfScreen.x - (sheepyWidth);
        //gap is between a small value of my choice and the total with of screen - the width of number of sheeps together (2 in this case)
        float gap = Random.Range(edgeOfScreen.x * 0.8f, ((edgeOfScreen.x * 2) - (sheepyWidth * (num - 1))));

        // position is calculated from the very edge of the screen and 
        //we add a value that is between 0 and total with of FREE screen (width of sheepy calculated)
        //minus total gap (which means acctualy width of sheepys) [just don't touch it, it works fine for other cases]
        float xPosition = -edges + Random.Range(0f, (edges * 2) - gap);

        // gap is total gap (previous line) devided by number of gaps in total
        gap /= num - 1;

        //instantiating the sheepys
        for (int i = 0; i < num; i++)
        {
            oneSheepyChosen(xPosition + (gap * i), 0f, index);
        }
    }
    //end of sliding forme sheepy
    #endregion

    public void ShowAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }

    void preMadeFormation(int index)
    {
        Instantiate(formationSheepys[index], formationSheepys[index].transform.position,
            formationSheepys[index].transform.rotation);
    }

    IEnumerator flareMaker(float time)
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject flareGameObject = Instantiate(flare, new Vector3(position.x, position.y, transform.position.z),
            Quaternion.identity);
        yield return new WaitForSeconds(time);
        Destroy(flareGameObject);
    }

    public void badView()
    {
        if (!BadViewGameObject.gameObject.activeSelf) StartCoroutine(badViewCoroutine());
    }

    IEnumerator badViewCoroutine()
    {
        pukeShowed = true;
        BadViewGameObject.SetActive(true);
        StartCoroutine(objectOpened(BadViewGameObject));
        yield return new WaitForSeconds(2);
        StartCoroutine(objectClosed(BadViewGameObject));
        yield return new WaitForSeconds(0.3f);
        BadViewGameObject.SetActive(false);
        pukeShowed = false;
    }

    IEnumerator objectOpened(GameObject objectToOpen)
    {
        objectToOpen.SetActive(true);
        for (int i = 0; i <= 10; i++)
        {
            float a = (float) i / 10;
            objectToOpen.transform.localScale = new Vector3(a, a, 1);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator objectClosed(GameObject objectToOpen)
    {
        for (int i = 10; i >= 0; i--)
        {
            float a = (float) i / 10;
            objectToOpen.transform.localScale = new Vector3(a, a, 1);
            yield return new WaitForSeconds(0.01f);
        }
        objectToOpen.SetActive(false);
    }

    void cashUpdate(int current)
    {
        string cash = current.ToString("N0", new NumberFormatInfo()
        {
            NumberGroupSizes = new[] { 3 },
            NumberGroupSeparator = ","
        });
        scoreText.GetComponent<TextMeshProUGUI>().text = "$" + cash;
    }
}