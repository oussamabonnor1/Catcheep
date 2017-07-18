using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject obstaclePrefab;
    private GameObject obstacle;

    private bool obstacleCreated;

	// Use this for initialization
	void Start ()
	{
	    obstacleCreated = false;
	    StartCoroutine(createObstacle());
	}
	
	// Update is called once per frame
	void Update () {
        
	    if (obstacle != null && obstacleCreated) //this is an ensurance policy, do not touch it
	    {
	        
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
