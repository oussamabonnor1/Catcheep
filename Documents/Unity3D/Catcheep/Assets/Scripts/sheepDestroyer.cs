using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sheepDestroyer : MonoBehaviour
{
    public bool caught;
    private GameObject sheepsCaughtText;
    private GameObject scoreText;

    void Start()
    {
        caught = false;
        sheepsCaughtText = GameObject.Find("sheeps caught");
        scoreText = GameObject.Find("score");

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Vector3.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position) <= 10.05f)
        {
            ++gameManager.sheepsCaught;
            sheepsCaughtText.GetComponent<Text>().text = " x " + gameManager.sheepsCaught;

            ++gameManager.combo;
            gameManager.score += 100;
            scoreText.GetComponent<Text>().text = "Score: " + (gameManager.score + 10 * gameManager.combo);

            caught = true;
            Destruction();
        }
        //TODO: figure out a way to communicate scripts combo hits
    }

    IEnumerator deathAnimation()
    {
        SpriteRenderer sheepColor = GetComponent<SpriteRenderer>();
        for (int i = 255; i >= 0; i-=50)
        {
            //TODO: fix the damn color effects please ! 
            sheepColor.color = new Color(sheepColor.color.r, sheepColor.color.g, sheepColor.color.b, i);
            yield return new WaitForSeconds(.5f);
        }
        
    }

    public void Destruction()
    {
        //StartCoroutine(deathAnimation());
        Destroy(gameObject);
    }
}
