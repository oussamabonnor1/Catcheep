using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject obstaclePrefab;
    private GameObject obstacle;
    
    private Vector2 edgeOfScreen;

    [Range(0, 10)] public int speed;

	// Use this for initialization
	void Start ()
	{
	    edgeOfScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,0f));
	    StartCoroutine(createObstacle());
	}
	
	// Update is called once per frame
	void Update () {
        
	    if (obstacle != null) //this is an ensurance policy, do not touch it
	    {
            //making the obstacle go...
	        obstacle.transform.position += Time.deltaTime * new Vector3(speed,0f,0f);
            //destroying it when it reaches far end of screen...
	        if (obstacle.transform.position.x >= edgeOfScreen.x +
	            obstacle.GetComponent<SpriteRenderer>().sprite.bounds.size.x)
	        {
	            Destroy(obstacle);
                //making sure we restart the creation AFTER we destroyed the first one
	            StartCoroutine(createObstacle());
	        }
	    }
	}

    public IEnumerator createObstacle()
    {
        yield return new WaitForSeconds(Random.Range(1,3));
        obstacle = Instantiate(obstaclePrefab, obstaclePrefab.transform.position,
            obstaclePrefab.transform.rotation);
        obstacle.SetActive(true);
    }
}
