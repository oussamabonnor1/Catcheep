using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sheepDestroyer : MonoBehaviour
{
    private float speed;
    public bool caught;

    private GameObject sheepsCaughtText;
    private GameObject scoreText;
    private GameObject sheepCage;

    void Start()
    {
        caught = false;
        sheepsCaughtText = GameObject.Find("sheeps caught");
        scoreText = GameObject.Find("score");
        sheepCage = GameObject.Find("sheep cage");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Vector3.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position) <= 10.05f)
        {
            sheepClicked();
        }
        //TODO: figure out a way to communicate scripts combo hits

        if (caught)
        {
            transform.position = Vector3.Lerp(transform.position, sheepCage.transform.position, Time.deltaTime * speed);
        }
    }

    void sheepClicked()
    {
        ++gameManager.sheepsCaught;
        sheepsCaughtText.GetComponent<Text>().text = " x " + gameManager.sheepsCaught;

        ++gameManager.combo;
        gameManager.score += 100;
        scoreText.GetComponent<Text>().text = "Score: " + (gameManager.score + 10 * gameManager.combo);

        speed = Mathf.Clamp(Vector3.Distance(transform.position, sheepCage.transform.position), 1f, 6f);

        caught = true;
        Destruction();
    }
   

    IEnumerator deathAnimation()
    {
        float originalScale = transform.localScale.x;

        for (float i = originalScale; i >= 0.1f; i-=0.5f)
        {
            transform.localScale = new Vector3(i,i);
            yield return new WaitForSeconds(0.05f);
        }

        Destroy(gameObject);
    }

    public void Destruction()
    {
        StartCoroutine(deathAnimation());
    }

}
