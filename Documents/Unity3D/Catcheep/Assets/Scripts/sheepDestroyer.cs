using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sheepDestroyer : MonoBehaviour
{
    public bool caught;
    private GameObject scoreText;

    void Start()
    {
        caught = false;
        scoreText = GameObject.Find("Score");

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Vector3.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position) <= 10.05f)
        {
            ++gameManager.score;
            scoreText.GetComponent<Text>().text = " x " + gameManager.score;
            caught = true;
            Destruction();
        }
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
