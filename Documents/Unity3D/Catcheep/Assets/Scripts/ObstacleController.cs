using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject obstaclePrefab;

    private bool obstacleCreated;

	// Use this for initialization
	void Start ()
	{
	    obstacleCreated = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator createObstacle()
    {
        yield return new WaitForSeconds(Random.Range(5,15));
        GameObject obstacle = Instantiate(obstaclePrefab, obstaclePrefab.transform.position,
            obstaclePrefab.transform.rotation);
        
    }
}
