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
        }
    }

    void threeSheepyHorizontalFullScreen()
    {
       // float edges = edgeOfScreen.x - (3 * (sheeps[0].GetComponent<SpriteRenderer>().bounds.extents).x);
        float xPosition = -edgeOfScreen.x + (sheeps[0].GetComponent<SpriteRenderer>().bounds.extents).x;// Random.Range(-edgeOfScreen.x, edges);
       
        for (int i = 0; i < 3; i++)
        {
            
            Vector3 spawnPositionVector3 = new Vector3(xPosition + (edgeOfScreen.x * 0.8f * i), transform.position.y, transform.position.z);

            Instantiate(sheeps[0], spawnPositionVector3, Quaternion.identity);
            
        }
        
    }
}