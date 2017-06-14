using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class distanceController : MonoBehaviour
{
    private int speed;
    private int slideSpeed;
    private GameObject pathCollsionGameObject;

    // Use this for initialization
    void Start()
    {
        speed = gameObject.GetComponent<SheepMovement>().Speed;
        slideSpeed = gameObject.GetComponent<SheepMovement>().slideSpeed;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //not a group (collider below screen
        if (other.gameObject.tag != "group")
        {
            //speed is lower (collision will happen)
            if (other.GetComponent<SheepMovement>().Speed > speed)
            {
                pathCollsionGameObject = other.gameObject;
                goingDownSpeedControl(other);

                if (gameObject.tag == "blacky")
                {
                    goingSidewaysSpeedControl(other);
                }
            }
        }
    }

    void goingDownSpeedControl(Collider2D other)
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

    void goingSidewaysSpeedControl(Collider2D other)
    {
        //so that a break happens: object must be beneths the speeding object, and a collision must be imminant in the Y axis
        float width = other.gameObject.GetComponent<SpriteRenderer>().sprite.bounds.extents.x +
                      GetComponent<SpriteRenderer>().sprite.bounds.extents.x;
        float distance = Mathf.Abs(transform.position.y - other.transform.position.y);

        if (distance < width)
        {
            gameObject.GetComponent<SheepMovement>().slideSpeed = 0;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag != "group")
        {
            if (other.gameObject == pathCollsionGameObject)
            {
                gameObject.GetComponent<SheepMovement>().Speed = speed;

                if (gameObject.tag == "blacky")
                {
                    gameObject.GetComponent<SheepMovement>().slideSpeed = slideSpeed;
                }
            }
        }
    }
}