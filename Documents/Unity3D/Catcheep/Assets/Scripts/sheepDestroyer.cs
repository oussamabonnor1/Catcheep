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
        if (Input.GetMouseButtonDown(0) && Vector3.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position) <= 10.05f)
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
            //TODO: fix the damn color effects please ! 
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
