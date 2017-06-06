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
            caught = true;
            Destruction();
        }
    }

    IEnumerator deathAnimation()
    {
        SpriteRenderer sheepColor = GetComponent<SpriteRenderer>();
        for (int i = 255; i >= 0; i-=50)
        {
            sheepColor.color = new Color(sheepColor.color.r, sheepColor.color.g, sheepColor.color.b, i);
            yield return new WaitForSeconds(.5f);
        }
        
    }

    public void Destruction()
    {
        //StartCoroutine(deathAnimation());
        Destroy(gameObject);
    }
}
