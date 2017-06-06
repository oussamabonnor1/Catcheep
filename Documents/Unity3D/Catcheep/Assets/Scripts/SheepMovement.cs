using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepMovement : MonoBehaviour
{
    [Range(0f,10)]
    public int speed;

    private Vector2 destination;

    // Use this for initialization
    void Start()
    {
        Vector3 height = Camera.main.ScreenToWorldPoint(new Vector3(Screen.height, 0f, 0f));
        destination = new Vector2(0f, transform.position.y - height.x);
        print(destination);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(destination * Time.deltaTime * speed);
    }
}