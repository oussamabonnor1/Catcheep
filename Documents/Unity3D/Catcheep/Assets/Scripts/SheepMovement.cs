using UnityEngine;

public class SheepMovement : MonoBehaviour
{
    [Range(0f, 10)] public int Speed;

    public sheepDestroyer SheepDestroyer;

    private Vector2 destination;
    private Vector3 edgeOfScreen;

    private float sheepWidth;
    // Use this for initialization
    void Start()
    {
        sheepWidth = GetComponent<SpriteRenderer>().sprite.bounds.extents.x;
        edgeOfScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));

        switch (gameObject.tag)
        {
            case "sheepy":
                destination = new Vector2(0f, -1f);
                break;
            case "blacky":
                destination = new Vector2(1f, -1f);
                break;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!SheepDestroyer.caught)
        {
            switch (gameObject.tag)
            {
                case "sheepy":
                    straightMovement(destination);
                    break;
                case "blacky":
                    zigZagMovement(destination);
                    break;
            }
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        }
    }

    void straightMovement(Vector3 destination)
    {
        transform.Translate(destination * Time.deltaTime * Speed);
    }

    void zigZagMovement(Vector3 destination)
    {
        transform.Translate(destination * Time.deltaTime * Speed);
        print("sheepy position: " + transform.position.x);
        print("screen edge " + (edgeOfScreen.x - sheepWidth));
        if (transform.position.x >= edgeOfScreen.x - sheepWidth)
        {
            print("destination changed");
            this.destination = new Vector2(-1f, -1f);
        }
        if (transform.position.x <= -edgeOfScreen.x + sheepWidth)
        {
            this.destination= new Vector2(1f, -1f);
        }
    }
}