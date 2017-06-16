using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class gameManager : MonoBehaviour
{
    public GameObject[] sheeps;
    public GameObject background;

    private bool gameOver;
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
        }

        ResizeBackground();

        scoreText = GameObject.Find("score");
        scoreText.GetComponent<Text>().text = "Score: " + score;

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
            }

            if (catchedSomething)
            {
                catchedSomething = false;
            }
        }
    }

    void ResizeBackground()
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
        //transform.localScale.x = worldScreenWidth / width;
        Vector3 yHeight = background.transform.localScale;
        yHeight.y = worldScreenHeight / height;
        background.transform.localScale = yHeight;
        //transform.localScale.y = worldScreenHeight / height;

    }

    IEnumerator sheepSpawner()
    {
        yield return new WaitForSeconds(1f);
        int size = sheeps.Length;

        while (!gameOver)
        {

            int i = Random.Range(0, 9);

            switch (i)
            {
                case 0:
                    oneSheepyRandom(Random.Range(0, size));
                    yield return new WaitForSeconds(2);
                    break;

                case 1:
                    threeSheepySlidingRightUpDown(1);
                    yield return new WaitForSeconds(3);
                    break;
                case 2:
                    int num = Random.Range(1, 4);
                    twoSheepyHorizontalManySet(num, Random.Range(0, size - 2));
                    yield return new WaitForSeconds(num);
                    break;

                case 3:
                    threeSheepyHorizontalpartScreen();
                    yield return new WaitForSeconds(3);
                    break;

                case 4:
                    vFormeSheepy(Random.Range(2, 4));
                    yield return new WaitForSeconds(3);
                    break;

                case 5:
                    fourSheepyTriangleLookingDownUp(-1);
                    yield return new WaitForSeconds(4);
                    break;

                case 6:
                    slidingSheepy(Random.Range(2, 5));
                    yield return new WaitForSeconds(4);
                    break;

                case 7:
                    threeSheepyHorizontalFullScreen();
                    yield return new WaitForSeconds(4);
                    break;

                case 8:
                    fourSheepyTriangleLookingDownUp(1);
                    yield return new WaitForSeconds(4);
                    break;
            }

        }
    }


    //one sheepy formations:
    void oneSheepyRandom(int index)
    {
        float edges = edgeOfScreen.x - (sheeps[0].GetComponent<SpriteRenderer>().sprite.bounds.extents).x;
        float xPosition = -edges; //Random.Range(-edges, edges);
        Vector3 spawnPosition = new Vector3(xPosition, transform.position.y, transform.position.z);
        Instantiate(sheeps[index], spawnPosition, Quaternion.identity);
    }

    void oneSheepyChosen(float xPosition, float deltaYPosition, int index)
    {
        Vector3 spawnPosition = new Vector3(xPosition, transform.position.y + deltaYPosition, transform.position.z);
        Instantiate(sheeps[index], spawnPosition, Quaternion.identity);
    }
    //end of one sheepy formations;

    //two sheepy formations:
    void twoSheepyHorizontalManySet(int times, int index)
    {
        float sheepyWidth = (sheeps[0].GetComponent<SpriteRenderer>().sprite.bounds.extents).x;
        float edges = edgeOfScreen.x - (sheepyWidth);
        //gap is between a small value of my choice and the total with of screen - the width of number of sheeps together (2 in this case)
        float gap = Random.Range(edgeOfScreen.x * 0.4f, (edgeOfScreen.x * 2) - (sheepyWidth * 2));
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
    void threeSheepyHorizontalFullScreen()
    {
        float sheepyWidth = (sheeps[0].GetComponent<SpriteRenderer>().bounds.size).x;
        float xPosition = -edgeOfScreen.x + sheepyWidth / 2;
        //used one sheepy width in gap cause that s how many sheepys are in between (hate this code)
        float gap = ((edgeOfScreen.x * 2) - (sheepyWidth)) / 2;
        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnPositionVector3 = new Vector3(xPosition + (gap * i), transform.position.y,
                transform.position.z);
            Instantiate(sheeps[0], spawnPositionVector3, Quaternion.identity);
        }
    }

    void threeSheepyHorizontalpartScreen()
    {
        // positioning the first (left) sheepy
        float sheepyWidth = (sheeps[0].GetComponent<SpriteRenderer>().sprite.bounds.extents).x;
        float edges = edgeOfScreen.x - (sheepyWidth);
        //gap is between a small value of my choice and the total with of screen - the width of number of sheeps together (2 in this case)
        float gap = Random.Range(edgeOfScreen.x * 0.5f, (edgeOfScreen.x * 2) - (sheepyWidth * 2));
        // i used edges - gap cause i instantiate from the left and i m making sure the sheep wont go overboard 
        float xPosition = Random.Range(-edges, edges - gap);
        // gap is total gap (previous line) devided by number of gaps in total
        gap /= 2;



        //instantiating the sheepys
        for (int i = 0; i < 3; i++)
        {
            oneSheepyChosen(xPosition + (gap * i), 0f, 0);
        }
    }

    void threeSheepySlidingRightUpDown(int direction)
    {
        // positioning the first (left = 1 or right = -1 depending on the direction) sheepy
        float sheepyWidth = (sheeps[0].GetComponent<SpriteRenderer>().bounds.size).x;
        // edges is the max left position value that a sheepy can have (screen width + concidering the sheepy width)
        float edges = direction * (-edgeOfScreen.x + sheepyWidth);
        //choosing half of screen where to position first sheepy depending on direction
        float xPosition = Random.Range(edges, 0f - direction * (2 * sheepyWidth));

        // finding out how much gap should be between the sheepys (randomly)
        float gap = Random.Range(edgeOfScreen.x * 0.4f, edgeOfScreen.x * 0.7f);

        //instantiating the sheepys
        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnPositionVector3 = new Vector3(xPosition + direction * (gap * i),
                transform.position.y + (gap * i), transform.position.z);
            Instantiate(sheeps[0], spawnPositionVector3, Quaternion.identity);
        }
    }
    //ende of three sheepy formations;

    //four sheepy formations:
    void fourSheepyTriangleLookingDownUp(int direction)
    {
        //some paramtres that we need to instantite correctly
        float sheepyWidth = (sheeps[0].GetComponent<SpriteRenderer>().bounds.extents).x;
        float edges = -edgeOfScreen.x + sheepyWidth;
        float xPosition = Random.Range(edges, 0f - (2 * sheepyWidth));
        float gap = Random.Range(edgeOfScreen.x * 0.4f, edgeOfScreen.x * 0.7f);

        //saving the position of the second sheepy to creat the triangle
        Vector3 secondSheepy = new Vector3();

        //instantiating the sheepys
        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnPositionVector3 =
                new Vector3(xPosition + (gap * i), transform.position.y, transform.position.z);
            Instantiate(sheeps[0], spawnPositionVector3, Quaternion.identity);
            if (i == 1) secondSheepy = spawnPositionVector3;
        }

        oneSheepyChosen(secondSheepy.x, direction * gap, 0);
    }
    //ende of two sheepy formations;

    //V forme sheepy
    void vFormeSheepy(int number)
    {
        // finding out how much gap should be between the sheepys
        float gap = edgeOfScreen.x - (sheeps[0].GetComponent<SpriteRenderer>().sprite.bounds.extents.x * number);
        gap /= number - 1;

        // choosing half of screen where to position first sheepy depending on direction
        float xPosition = 0f; //Random.Range(edges, -edges);

        //instantiating the sheepys
        for (int i = 0; i < number; i++)
        {
            oneSheepyChosen(xPosition + (gap * i), transform.position.y + (gap * i), 0);
        }

        //instantiating the sheepys
        for (int i = 1; i < number; i++)
        {
            oneSheepyChosen(xPosition - (gap * i), transform.position.y + (gap * i), 0);
        }
    }
    //end of V frome sheepy sheepy

    //sliding forme sheepy (choice above 1 always)
    void slidingSheepy(int number)
    {
        //total screen ( x 2) - sheep wahed (prck 2 nssas) : gap howa (gap total (li b9a) - sheepWidth * number - 1) / chaal m gap

        float sheepWidthTotal = sheeps[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        float gapTotal = (edgeOfScreen.x * 2) - (sheepWidthTotal * number - 1);
        float gap = gapTotal / (number - 1);
        float edges = edgeOfScreen.x - (sheepWidthTotal);
        float xPosition = Random.Range(-edges, edges - gapTotal - (sheepWidthTotal / 2));
        for (int i = 0; i < number; i++)
        {
            oneSheepyChosen(xPosition + (i * gap), (i * gap), 0);
        }
    }

    //end of sliding forme sheepy

}