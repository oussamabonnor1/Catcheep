using System.Collections;
using UnityEngine;

public class SheepMovement : MonoBehaviour
{
    [Range(0f, 10)] public int Speed;
    public float slideSpeed;

    public sheepDestroyer SheepDestroyer;

    private Vector2 destination;

    private Vector3 edgeOfScreen;

    //help tool will be used only for wolf dressed as sheep as they will go towards it
    private GameObject helpTool;

    private float sheepWidth;

    // Use this for initialization
    void Start()
    {
        sheepWidth = GetComponent<SpriteRenderer>().sprite.bounds.extents.x;
        edgeOfScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));

        setDestination();

        //jst to use the same script for other objects that dnt need it really
        if (SheepDestroyer == null) SheepDestroyer = new sheepDestroyer();

        if (tag == "blacky") StartCoroutine(suddenChangeInDirection());
    }

    // Update is called once per frame
    void Update()
    {
        if (helpTool == null)
        {
            if (!SheepDestroyer.caught)
            {
                switch (gameObject.tag)
                {
                    case "sheepy":
                    case "sick":
                        destination = new Vector2(slideSpeed, -1f);
                        straightMovement(destination);
                        break;
                    case "blacky":
                        destination = new Vector2(slideSpeed, -1f);
                        zigZagMovement(destination);
                        break;
                }
            }
            else
            {
               GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
            }
        }
        else
        {
            Vector2 direction = transform.position - helpTool.transform.position;
            destination = -direction.normalized;
            helpTool = null;
        }
    }


    void setDestination()
    {
        switch (gameObject.tag)
        {
            case "sheepy":
            case "sick":
                destination = new Vector2(slideSpeed, -1f);
                break;
            case "blacky":
                slideSpeed = Random.Range(0, 2) * 2 - 1;
                destination = new Vector2(slideSpeed, -1f);
                GetComponent<Animator>().SetInteger("slideSpeedAnimation", (int)slideSpeed);
                break;
        }
    }

    void straightMovement(Vector3 destination)
    {
        transform.Translate(destination * Time.deltaTime * Speed);
    }

    void zigZagMovement(Vector3 destination)
    {
        transform.Translate(destination * Time.deltaTime * Speed);

        if (transform.position.x >= edgeOfScreen.x - sheepWidth)
        {
            slideSpeed = -1;
            this.destination = new Vector2(slideSpeed, -1f);
            GetComponent<Animator>().SetInteger("slideSpeedAnimation", (int)slideSpeed);
        }
        if (transform.position.x <= -edgeOfScreen.x + sheepWidth)
        {
            slideSpeed = 1;
            this.destination = new Vector2(slideSpeed, -1f);
            GetComponent<Animator>().SetInteger("slideSpeedAnimation", (int)slideSpeed);
        }


    }

    IEnumerator suddenChangeInDirection()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(Random.Range(0.8f, 1.6f));
            slideSpeed = Random.Range(-1, 2);

            int holder = GetComponent<Animator>().GetInteger("slideSpeedAnimation");
            if (holder != (int)slideSpeed) GetComponent<Animator>().SetInteger("slideSpeedAnimation", (int)slideSpeed);
        }
    }
    
}