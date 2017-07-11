using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;


public class gameManager : MonoBehaviour
{
    public GameObject[] sheeps;
    public GameObject[] formationSheepys;
    public GameObject background;
    public GameObject backgroundOfTrees;
    public AudioClip[] sheepSound;
    public GameObject winText;
    public GameObject flare;
    
    public static bool gameOver;
    private Vector3 edgeOfScreen;
    private GameObject scoreText;

    public static int totalSheepsCaught;
    public static int combo;
    public static int score;
    public static bool catchedSomething;



    // Use this for initialization
    void Start()
    {
        catchedSomething = false;
        totalSheepsCaught = 0;
        combo = 0;
        score = 0;

        if (background == null)
        {
            background = GameObject.Find("background");
            ResizeBackground(background);
        }

        if (backgroundOfTrees == null && GameObject.Find("Trees") != null)
        {
            backgroundOfTrees = GameObject.Find("Trees");
            ResizeBackground(backgroundOfTrees);
        }
        
        scoreText = GameObject.Find("score");
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + score;

        //edge of screen is a vector3 that holds the screens width (can't get it directly cause of Screen/World point difference)
        edgeOfScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        gameOver = false;
        StartCoroutine(sheepSpawner());
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            if (!catchedSomething && combo > 0)
            {
                combo = 0;
                StartCoroutine(flareMaker(0.45f));
                // Handheld.Vibrate();
            }

            if (catchedSomething)
            {
                catchedSomething = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowAd();
            SceneManager.LoadScene("Start");
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

    IEnumerator sheepSpawner()
    {
        //yield return new WaitForSeconds(1f);
        int size = sheeps.Length -1;
        float taux = 0;
        
        while (!gameOver)
        {
            Collider2D collisions = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y),
                new Vector2(edgeOfScreen.x, edgeOfScreen.y), 0f);

            if (collisions == null)
            {

                if (taux < 2) taux += 0.065f;
                int i = Random.Range(-5, 10);

                switch (i)
                {
                    case -5:
                    case -4:
                    case -3:
                    case -2:
                    case -1:
                    case  0:
                        oneSheepyRandom(Random.Range(0, size + 1));
                        //yield return new WaitForSeconds(2 - taux);
                        break;

                    case 1:
                        threeSheepySlidingRightUpDown(1, Random.Range(0, size));
                        //yield return new WaitForSeconds(3 - taux);
                        break;
                    case 2:
                        int num = Random.Range(1, 4);
                        twoSheepyHorizontalManySet(num, Random.Range(0, size));
                        //yield return new WaitForSeconds(num - taux);
                        break;

                    case 3:
                        SheepyHorizontalpartScreen(Random.Range(0, size), Random.Range(2, 5));
                        //yield return new WaitForSeconds(3 - taux);
                        break;

                    case 4:
                        vFormeSheepy(Random.Range(2, 4), Random.Range(0, size));
                        yield return new WaitForSeconds(2 - taux);
                        break;

                    case 5:
                        fourSheepyTriangleLookingDownUp(-1, Random.Range(0, size));
                        //yield return new WaitForSeconds(3 - taux);
                        break;

                    case 6:
                        slidingSheepy(Random.Range(2, 5), Random.Range(0, size + 1));
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
                        preMadeFormation(Random.Range(0,formationSheepys.Length));
                        //yield return new WaitForSeconds();
                        break;
                }

            }
            else
            {
                yield return new WaitForSeconds(2 - taux);
            }
        }

        if (gameOver)
        {
            winText.SetActive(true);
        }
    }


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

    void threeSheepySlidingRightUpDown(int direction ,int index)
    {
        // positioning the first (left = 1 or right = -1 depending on the direction) sheepy
        float sheepyWidth = (sheeps[index].GetComponent<SpriteRenderer>().bounds.size).x;
        // edges is the max left position value that a sheepy can have (screen width + concidering the sheepy width)
        float edges = direction * (-edgeOfScreen.x + sheepyWidth);
        //choosing half of screen where to position first sheepy depending on direction
        float xPosition = Random.Range(edges, 0f - direction * (2 * sheepyWidth));

        // finding out how much gap should be between the sheepys (randomly)
        float gap = Random.Range(edgeOfScreen.x * 0.5f, edgeOfScreen.x * 0.7f);

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


    public void ShowAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }

    void preMadeFormation(int index)
    {
        Instantiate(formationSheepys[index], transform.position, formationSheepys[index].transform.rotation);
    }

    IEnumerator flareMaker(float time)
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
       GameObject flareGameObject =  Instantiate(flare,new Vector3(position.x,position.y, transform.position.z) , Quaternion.identity);
        yield return new WaitForSeconds(time);
        Destroy(flareGameObject);
    }
}