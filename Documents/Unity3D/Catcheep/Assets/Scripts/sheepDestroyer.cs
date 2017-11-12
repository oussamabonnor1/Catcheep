using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Assets.Scripts;
using TMPro;
using UnityEngine;

public class sheepDestroyer : MonoBehaviour
{
    private float speed;
    public bool caught;

    public GameObject hitText;
    public GameObject explosion;

    private GameObject sheepsCaughtText;
    private GameObject scoreText;
    private GameObject sheepCage;

    public CircleCollider2D smallCollider;
    public Collider2D BigCollider;

    private Touch touchHolder;
    private float[] timeOfTouch;

    void Start()
    {
        timeOfTouch = new float[2];
        caught = false;
        sheepsCaughtText = GameObject.Find("sheeps caught");
        scoreText = GameObject.Find("score");
        cashUpdate(gameManager.score);
        sheepCage = GameObject.Find("sheep cage");
    }


    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (tag != "group")
            {
                if (hit.collider == smallCollider && hit.collider.transform == transform)
                {
                    if (gameObject.tag.Equals("sick"))
                    { 
                        sickClicked();
                    }
                    else
                    {
                        // raycast hit this gameobject
                        gameManager.catchedSomething = true;
                        sheepClicked();
                    }
                }
            }

            if (tag == "group")
            {
                if (hit.collider == BigCollider)
                {
                    gameManager.catchedSomething = true;
                    obstacleClicked(hit.collider.gameObject);
                }
            }
        }*/
        if (Input.touchCount < 3 && Input.touchCount > 0 && !gameManager.pukeShowed)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    timeOfTouch[touch.fingerId] = 0;
                }
                if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                {
                    timeOfTouch[touch.fingerId] += Time.deltaTime;
                }

                if (timeOfTouch[touch.fingerId] <= 0.042)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

                    if (tag != "group")
                    {
                        if (hit.collider == smallCollider && hit.collider.transform == transform)
                        {
                            if(PlayerPrefs.GetInt("SFX") == 0 ) GetComponent<AudioSource>().Play();
                            if (gameObject.tag.Equals("sick"))
                            {
                                sickClicked();
                            }
                            else
                            {
                                // raycast hit this gameobject
                                gameManager.catchedSomething = true;
                                sheepClicked();
                            }
                        }
                    }

                    if (tag == "group")
                    {
                        if (hit.collider == BigCollider)
                        {
                            gameManager.catchedSomething = true;
                            obstacleClicked(hit.collider.gameObject);
                        }
                    }
                }
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    timeOfTouch[touch.fingerId] = 0;
                }
            }
        }
        if (caught)
        {
            transform.position = Vector3.Lerp(transform.position, sheepCage.transform.position, Time.deltaTime * speed);
        }
    }

    void obstacleClicked(GameObject obstacle)
    {
        caught = true;
        StartCoroutine(GameObject.Find("Game Manager").GetComponent<ObstacleController>().createObstacle());
        gameManager.score += 200 + 10 * gameManager.combo;
        cashUpdate(gameManager.score);
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + 200 + 10 * gameManager.combo);
        PlayerPrefs.SetInt("mostMoney", PlayerPrefs.GetInt("mostMoney") + 200 + 10 * gameManager.combo);
        if (obstacle.gameObject.name == "Car(Clone)")
        {
            PlayerPrefs.SetInt("carsCrushed", PlayerPrefs.GetInt("carsCrushed") + 1);
             GameObject.Find("Music Manager").GetComponent<music>().ObjectsSound(5);
            GameObject explosionGameObject = Instantiate(explosion, obstacle.transform.position, Quaternion.identity);
            Destroy(explosionGameObject, 1f);
            obstacle.gameObject.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("snowCrushed", PlayerPrefs.GetInt("snowCrushed") + 1);
            StartCoroutine(deathAnimation());
        }
    }

    public void sheepClicked()
    {
        if (gameObject.tag.Contains("Boss"))
        {
            //Giving the player a great reward for an amazing opening
            gameManager.score += 10000 + 1000 * gameManager.combo;
        }

        if (gameObject.tag.Equals("sheepy")) {

            if (gameObject.name.Contains("skiing"))
            {
                gameManager.snowSheepyCaught++;
                PlayerPrefs.SetInt("sheep3", PlayerPrefs.GetInt("sheep3") + 1);
            }
            else if (gameObject.name.Contains("city"))
            {
                gameManager.citySheepyCaught++;
                PlayerPrefs.SetInt("sheep5", PlayerPrefs.GetInt("sheep5") + 1);
            }
            else if (gameObject.name.Contains("flash"))
            {
                gameManager.flashCaught++;
                PlayerPrefs.SetInt("sheep2", PlayerPrefs.GetInt("sheep2") + 1);
            }
            else
            {
            ++gameManager.sheepyCaught;
            PlayerPrefs.SetInt("sheep0", PlayerPrefs.GetInt("sheep0") + 1);
            }
        }
        if (gameObject.tag.Equals("blacky"))
        {
            if (gameObject.name.Contains("skiing"))
            {
                gameManager.snowBlackyCaught++;
                PlayerPrefs.SetInt("sheep4", PlayerPrefs.GetInt("sheep4") + 1);
            }
            else if (gameObject.name.Contains("city"))
            {
                gameManager.cityBlackyCaught++;
                PlayerPrefs.SetInt("sheep6", PlayerPrefs.GetInt("sheep6") + 1);
            }
            else
            {
                gameManager.blackyCaught++;
                PlayerPrefs.SetInt("sheep1", PlayerPrefs.GetInt("sheep1") + 1);
            }
        }

        caught = true;
        //creating hit text
        GameObject canvas = GameObject.Find("Canvas");
        GameObject hit = Instantiate(hitText, transform.position, Quaternion.identity);
        Vector3 position = Camera.main.WorldToScreenPoint(transform.position);
        hit.transform.SetParent(canvas.transform, false);
        hit.transform.SetPositionAndRotation(position, Quaternion.identity);

        //deactivating collider so that it doesnt disturb the scene (collision detection)
        smallCollider.enabled = false;

        //rewarding player
        ++gameManager.totalSheepsCaught;
        PlayerPrefs.SetInt("sheepy", PlayerPrefs.GetInt("sheepy") + 1);
        PlayerPrefs.SetInt("mostSheepy", PlayerPrefs.GetInt("mostSheepy") + 1);
        sheepsCaughtText.GetComponent<TextMeshProUGUI>().text = " x " + gameManager.totalSheepsCaught;
        ++gameManager.combo;
        gameManager.score += 10 * gameManager.combo;
        PlayerPrefs.SetInt("money", gameManager.score);
        PlayerPrefs.SetInt("mostMoney", PlayerPrefs.GetInt("mostMoney") + 10 * gameManager.combo);
        cashUpdate(gameManager.score);

        //destroying sheep
        speed = Mathf.Clamp(Vector3.Distance(transform.position, sheepCage.transform.position), 1f, 6f);

        Destruction();
    }

    public void sickClicked()
    {
        //deactivating collider so that it doesnt disturb the scene (collision detection)
        smallCollider.enabled = false;

        //punishing player
        GameObject.Find("Game Manager").GetComponent<gameManager>().badView();
        if(gameManager.score  > 500 ) gameManager.score -= 500;
        PlayerPrefs.SetInt("money", gameManager.score);
        cashUpdate(gameManager.score);

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

        if (transform.parent != null && transform.parent.childCount == 1)
        {
            Destroy(transform.parent.gameObject);
        }

        if (gameObject.name != "Snow ball(Clone)") Destroy(gameObject);
        else gameObject.SetActive(false);
    }

    public void Destruction()
    {
        StartCoroutine(deathAnimation());
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