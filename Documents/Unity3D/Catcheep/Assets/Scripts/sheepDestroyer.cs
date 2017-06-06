using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sheepDestroyer : MonoBehaviour
{
    public bool caught;

    void Start()
    {
        caught = false;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))// && Vector3.Distance(Input.mousePosition,transform.position) <= 10)
        {
            print("mouse clicked");
            caught = true;
            Destruction();
        }
    }

    IEnumerator deathAnimation()
    {
        Color sheepColor = GetComponent<SpriteRenderer>().color;
        for (int i = 255; i >= 0; i-=15)
        {
            print(sheepColor);
            sheepColor = new Color(i,i,i, i);
            yield return new WaitForSeconds(.1f);
        }

        Destroy(gameObject);
    }

    public void Destruction()
    {
        StartCoroutine(deathAnimation());
    }
}
