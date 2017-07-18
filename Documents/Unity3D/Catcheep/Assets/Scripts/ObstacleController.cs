using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject obstaclePrefab;
    private GameObject obstacle;

    private bool obstacleCreated;
    private Vector2 edgeOfScreen;

	// Use this for initialization
	void Start ()
	{
	    edgeOfScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,0f));
	    obstacleCreated = false;
	    StartCoroutine(createObstacle());
	}
	
	// Update is called once per frame
	void Update () {
        
	    if (obstacle != null && obstacleCreated) //this is an ensurance policy, do not touch it
	    {
            //
	        obstacle.transform.position += Time.deltaTime * new Vector3(1f,0f,0f);
	        if (obstacle.transform.position.x >= edgeOfScreen.x +
	            obstacle.GetComponent<SpriteRenderer>().sprite.bounds.size.x)
	        {
	            
	        }
	    }
	}

    IEnumerator createObstacle()
    {
        yield return new WaitForSeconds(Random.Range(5,15));
        obstacle = Instantiate(obstaclePrefab, obstaclePrefab.transform.position,
            obstaclePrefab.transform.rotation);
        obstacleCreated = true;

    }
}
