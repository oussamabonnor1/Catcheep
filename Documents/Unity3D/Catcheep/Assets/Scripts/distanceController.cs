using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class distanceController : MonoBehaviour
{
    private int speed;

	// Use this for initialization
	void Start ()
	{
	    speed = gameObject.GetComponent<SheepMovement>().Speed;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        
            //so that a break happens: object must be beneths the speeding object, and a collision must be imminant in the X axis
            float width = other.gameObject.GetComponent<SpriteRenderer>().sprite.bounds.extents.x +
            GetComponent<SpriteRenderer>().sprite.bounds.extents.x;

            float distance = Mathf.Abs(transform.position.x - other.transform.position.x);

            if (other.gameObject.transform.position.y < transform.position.y && distance < width)
            {
                gameObject.GetComponent<SheepMovement>().Speed = other.gameObject.GetComponent<SheepMovement>().Speed;
            }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        gameObject.GetComponent<SheepMovement>().Speed = speed;
    }
}
