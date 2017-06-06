using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepMovement : MonoBehaviour
{
    // private Rigidbody2D myRigidbody2D;

    private Vector2 destination;

    // Use this for initialization
    void Start()
    {
        //   myRigidbody2D = GetComponent<Rigidbody2D>();

        Vector3 height = Camera.main.ScreenToWorldPoint(new Vector3(Screen.height, 0f, 0f));
        print(height);
        destination = new Vector2(transform.position.x, transform.position.y - height.x - 5);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime);
    }
}