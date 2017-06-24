using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class sheepDestroyer : MonoBehaviour
{
    private float speed;
    public bool caught;

    public GameObject hitText;

    private GameObject sheepsCaughtText;
    private GameObject scoreText;
    private GameObject sheepCage;

    public CircleCollider2D smallCollider;


    void Start()
    {
        caught = false;
        sheepsCaughtText = GameObject.Find("sheeps caught");
        scoreText = GameObject.Find("score");
        sheepCage = GameObject.Find("sheep cage");
    }

    void Update()
    {
       
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider == smallCollider && hit.collider.transform == transform)
            {
                // raycast hit this gameobject
                gameManager.catchedSomething = true;
                sheepClicked();
            }
        }

        if (caught)
        {
            transform.position = Vector3.Lerp(transform.position, sheepCage.transform.position, Time.deltaTime * speed);
        }
    }

    public void sheepClicked()
    {
        caught = true;
        //creating hit text
        GameObject canvas = GameObject.Find("Canvas");
        GameObject hit = Instantiate(hitText, transform.position, Quaternion.identity);
        Vector3 position = Camera.main.WorldToScreenPoint(transform.position);
        hit.transform.SetParent(canvas.transform, false);
        hit.transform.SetPositionAndRotation(position,Quaternion.identity);

        //deactivating collider so that it doesnt disturb the scene (collision detection)
        smallCollider.enabled = false;

        //rewarding player
        ++gameManager.totalSheepsCaught;
        sheepsCaughtText.GetComponent<Text>().text = " x " + gameManager.totalSheepsCaught;
        ++gameManager.combo;
        gameManager.score += 100;
        scoreText.GetComponent<Text>().text = "Score: " + (gameManager.score + 10 * gameManager.combo);

        //destroying sheep
        speed = Mathf.Clamp(Vector3.Distance(transform.position, sheepCage.transform.position), 1f, 6f);
        
        Destruction();
    }


    IEnumerator deathAnimation()
    {
        float originalScale = transform.localScale.x;

        for (float i = originalScale; i >= 0.1f; i -= 0.5f)
        {
            transform.localScale = new Vector3(i, i);
            yield return new WaitForSeconds(0.08f);
        }

        Destroy(gameObject);
    }

    public void Destruction()
    {
        StartCoroutine(deathAnimation());
    }
}