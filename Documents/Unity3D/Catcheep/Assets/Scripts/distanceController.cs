using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class distanceController : MonoBehaviour
{
    private int speed;
    private float slideSpeed;
    private GameObject pathCollsionGameObject;
    private bool isTouching;
    private bool helpCouroutineCalled;

    // Use this for initialization
    void Start()
    {
        isTouching = false;
        helpCouroutineCalled = false;
        speed = gameObject.GetComponent<SheepMovement>().Speed;
        slideSpeed = gameObject.GetComponent<SheepMovement>().slideSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTouching)
        {
            gameObject.GetComponent<SheepMovement>().Speed = speed;
        }
        else
        {
            isTouching = false;
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        //not a group (collider below screen)
        if (other.gameObject.tag != "group" && !other.gameObject.name.Equals("Sound Collider"))
        {
            //speed is lower (collision will happen)
            if (other.GetComponent<SheepMovement>().Speed <= speed)
            {
                pathCollsionGameObject = other.gameObject;
                goingDownSpeedControl(other);

                if (tag == "blacky")
                {
                    goingSidewaysSpeedControl(other);
                }
            }
        }
    }

    void goingDownSpeedControl(Collider2D other)
    {
        if (other.gameObject.tag == "hayStack" || other.gameObject.tag == "net" || other.gameObject.tag == "loveHelp")
        {
            StartCoroutine(sheepyCaughtByHelpTool(other.gameObject));
        }
        else
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
    }

    void goingSidewaysSpeedControl(Collider2D other)
    {
        if (other.gameObject.tag == "hayStack")
        {
            gameObject.GetComponent<SheepMovement>().slideSpeed = 0;
        }
        else if (other.gameObject.tag == "net")
        {
            GetComponent<SheepMovement>().SheepDestroyer.caught = true;
        }
        else
        {
            //so that a break happens: object must be beneths the speeding object, and a collision must be imminant in the Y axis
            float width = other.gameObject.GetComponent<SpriteRenderer>().sprite.bounds.extents.x +
                          GetComponent<SpriteRenderer>().sprite.bounds.extents.x;
            float distance = Mathf.Abs(transform.position.y - other.transform.position.y);

            if (distance < width)
            {
                gameObject.GetComponent<SheepMovement>().slideSpeed = 0;
                gameObject.GetComponent<Animator>().SetInteger("slideSpeedAnimation", 0);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag != "group")
        {
            if (other.gameObject == pathCollsionGameObject || other.gameObject.tag == "heyStack")
            {
                isTouching = false;
                gameObject.GetComponent<SheepMovement>().Speed = speed;

                if (gameObject.tag == "blacky")
                {
                    gameObject.GetComponent<SheepMovement>().slideSpeed = slideSpeed;
                    gameObject.GetComponent<Animator>().SetInteger("slideSpeedAnimation", (int) slideSpeed);
                }
            }
            
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        isTouching = true;
        //not a group (collider below screen)
        if (other.gameObject.tag != "group" && !helpCouroutineCalled && !other.gameObject.name.Equals("Sound Collider"))
        {
            //speed is lower (collision will happen)
            if (other.GetComponent<SheepMovement>().Speed <= speed)
            {
                pathCollsionGameObject = other.gameObject;
                if (other.gameObject.tag != "hayStack" && other.gameObject.tag != "net" &&
                    other.gameObject.tag != "loveHelp")
                {
                    //so that a break happens: object must be beneths the speeding object, and a collision must be imminant in the X axis
                    float width = other.gameObject.GetComponent<SpriteRenderer>().sprite.bounds.extents.x +
                                  GetComponent<SpriteRenderer>().sprite.bounds.extents.x;

                    float distance = Mathf.Abs(transform.position.x - other.transform.position.x);

                    if (other.gameObject.transform.position.y < transform.position.y && distance < width)
                    {
                        gameObject.GetComponent<SheepMovement>().Speed =
                            other.gameObject.GetComponent<SheepMovement>().Speed;
                    }
                }
                else if (!helpCouroutineCalled)
                {
                    StartCoroutine(sheepyCaughtByHelpTool(other.gameObject));
                    helpCouroutineCalled = true;
                }

                if (tag == "blacky")
                {
                    goingSidewaysSpeedControl(other);
                }
            }
        }
    }

    IEnumerator sheepyCaughtByHelpTool(GameObject other)
    {
        if (PlayerPrefs.GetInt("SFX") == 0) GetComponent<AudioSource>().Play();
        if (gameObject.tag.Equals("sick"))
        {
            GetComponent<SheepMovement>().SheepDestroyer.Destruction();
        }
        else
        {
            GetComponent<SheepMovement>().SheepDestroyer.caught = true;
            if (other.gameObject.tag != "loveHelp") yield return new WaitForSeconds(0.5f);
            GetComponent<SheepMovement>().SheepDestroyer.sheepClicked();
        }
    }
}