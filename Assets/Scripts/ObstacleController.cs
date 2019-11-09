using System.Collections;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleController : MonoBehaviour
{
    public GameObject obstaclePrefab;
    private GameObject obstacle;

    private Vector2 edgeOfScreen;
    private int i;

    [Range(0, 10)] public int speed;

    // Use this for initialization
    void Start()
    {
        edgeOfScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        if(!gameObject.name.Equals("Sound Collider")) StartCoroutine(createObstacle());
    }

    // Update is called once per frame
    void Update()
    {
        if (obstacle != null) //this is an ensurance policy, do not touch it
        {
            //making the obstacle go...
            if (i == 0)
            {
                obstacle.transform.position += Time.deltaTime * new Vector3(speed, 0f, 0f);
                if(obstacle.gameObject.name.Contains("Snow ball")) obstacle.transform.Rotate(0, 0, (int) -speed);
            }
            else
            {
                obstacle.transform.position -= Time.deltaTime * new Vector3(speed, 0f, 0f);
                if (obstacle.gameObject.name.Contains("Snow ball")) obstacle.transform.Rotate(0, 0, (int) speed);
            }
            //destroying it when it reaches far end of screen...
            if (obstacle.transform.position.x >= edgeOfScreen.x +
                obstacle.GetComponent<SpriteRenderer>().sprite.bounds.size.x && i == 0)
            {
                Destroy(obstacle);
                //making sure we restart the creation AFTER we destroyed the first one
                if(!gameManager.gameOver) StartCoroutine(createObstacle());
            }
            if (obstacle.transform.position.x <= -edgeOfScreen.x -
                obstacle.GetComponent<SpriteRenderer>().sprite.bounds.size.x && i == 1)
            {
                Destroy(obstacle);
                //making sure we restart the creation AFTER we destroyed the first one
                if (!gameManager.gameOver) StartCoroutine(createObstacle());
            }
            //i can make it into one big ass condition but why slow stuff down, right ?
        }
    }

    public IEnumerator createObstacle()
    {
        yield return new WaitForSeconds(Random.Range(1, 5));
        if(SceneManager.GetActiveScene().name.Equals("Snow")) GameObject.Find("Music Manager").GetComponent<music>().ObjectsSound(4);
        if(SceneManager.GetActiveScene().name.Equals("City")) GameObject.Find("Music Manager").GetComponent<music>().ObjectsSound(3);
        i = Random.Range(0, 2);
        Vector3 position = new Vector3(-obstaclePrefab.transform.position.x, obstaclePrefab.transform.position.y,
            obstaclePrefab.transform.position.z);
        if (i == 0)
        {
            position = obstaclePrefab.transform.position;
        }
        obstacle = Instantiate(obstaclePrefab, position, obstaclePrefab.transform.rotation);
        obstacle.SetActive(true);
        if (obstacle.gameObject.name.Contains("Car") && i == 1) obstacle.transform.Rotate(0, 180, 0);
    }

    //This Code section is strictly reserved for the sound collider to make sheeps SFX
    //it ll be used here only not to create and compile more scripts
    void OnTriggerEnter2D(Collider2D other)
    {
        if (PlayerPrefs.GetInt("SFX") == 0 && gameObject.name.Equals("Sound Collider")) GetComponent<AudioSource>().Play();
    }
}