using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public GameObject[] sheeps;
    public int[] waitTimeForEachSpawn;
    private bool gameOver;
    private Vector3 edgeOfScreen;
    private GameObject scoreText;

    public static int sheepsCaught;
    public static int combo;
    public static int score;


    // Use this for initialization
    void Start()
    {
        sheepsCaught = 0;
        combo = 0;
        score = 0;

        scoreText = GameObject.Find("score");
        scoreText.GetComponent<Text>().text = "Score: " + score;

        //edge of screen is a vector3 that holds the screens width (can't get it directly cause of Screen/World point difference)
        edgeOfScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        gameOver = false;
        StartCoroutine(sheepSpawner());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator sheepSpawner()
    {
        yield return new WaitForSeconds(1f);

        while (!gameOver)
        {
            threeSheepyHorizontalFullScreen();
            yield return new WaitForSeconds(3);

            threeSheepyHorizontalpartScreen();
            yield return new WaitForSeconds(3);

            oneSheepyRandom();
            yield return new WaitForSeconds(1);

            fourSheepyTriangleLookingDown();
            yield return new WaitForSeconds(4);
        }
    }

    void oneSheepyRandom()
    {
        float edges = edgeOfScreen.x - (sheeps[0].GetComponent<SpriteRenderer>().sprite.bounds.extents).x;
        float xPosition = Random.Range(-edges, edges);
        Vector3 spawnPosition = new Vector3(xPosition,transform.position.y,transform.position.z);
        Instantiate(sheeps[0], spawnPosition, Quaternion.identity);
    }

    void oneSheepyChosen(float xPosition, float deltaYPosition)
    {
        Vector3 spawnPosition = new Vector3(xPosition, transform.position.y + deltaYPosition, transform.position.z);
        Instantiate(sheeps[0], spawnPosition, Quaternion.identity);
    }

    void threeSheepyHorizontalFullScreen()
    {
        float xPosition = -edgeOfScreen.x + (sheeps[0].GetComponent<SpriteRenderer>().bounds.extents).x;

        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnPositionVector3 = new Vector3(xPosition + (edgeOfScreen.x * 0.8f * i), transform.position.y, transform.position.z);
            Instantiate(sheeps[0], spawnPositionVector3, Quaternion.identity);
        } 
    }

    void threeSheepyHorizontalpartScreen()
    {
        // positioning the first (left) sheepy
        // sheepy width is half the width of the sheep sprite
        float sheepyWidth = (sheeps[0].GetComponent<SpriteRenderer>().bounds.extents).x;
        // edges is the max left position value that a sheepy can have (screen width + concidering the sheepy width)
        float edges = -edgeOfScreen.x + sheepyWidth;
        // multiplied by 2 cause sheepy width is HALF of the sprite width and we dont want a sheepy showing in half on the right side J.I.C
        float xPosition = Random.Range(edges,0f -(2 *sheepyWidth));

        // finding out how much gap should be between the sheepys (randomly)
        float gap = Random.Range(edgeOfScreen.x * 0.4f, edgeOfScreen.x * 0.7f);

        //instantiating the sheepys
        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnPositionVector3 = new Vector3(xPosition + (gap * i), transform.position.y, transform.position.z);
            Instantiate(sheeps[0], spawnPositionVector3, Quaternion.identity);
        }
    }

    void fourSheepyTriangleLookingDown()
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
            Vector3 spawnPositionVector3 = new Vector3(xPosition + (gap * i), transform.position.y, transform.position.z);
            Instantiate(sheeps[0], spawnPositionVector3, Quaternion.identity);
            if (i == 1) secondSheepy = spawnPositionVector3;
        }
        
        oneSheepyChosen(secondSheepy.x, -1.5f * gap);
    }
}