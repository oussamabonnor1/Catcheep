using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class helpManager : MonoBehaviour
{
    public GameObject[] helpTools;
    private bool helpUsed;

	// Use this for initialization
	void Start ()
	{
	    helpUsed = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void hayStack()
    {
        if (!helpUsed)
        {
            helpUsed = true;
            GameObject heyGameObject = Instantiate(helpTools[0], transform.position, Quaternion.identity);
            StartCoroutine(helpDestroyer(3f, heyGameObject));
        }
    }

    public void net()
    {
        if (!helpUsed)
        {
            helpUsed = true;
            GameObject neGameObject = Instantiate(helpTools[1], transform.position, Quaternion.identity);
            StartCoroutine(helpDestroyer(1f, neGameObject));
        }
    }

    IEnumerator helpDestroyer(float lifeTime, GameObject gameObjectToDestroy)
    {
        yield return new WaitForSeconds(lifeTime);
        SpriteRenderer renderer = gameObjectToDestroy.GetComponent<SpriteRenderer>();

        for (int i = 0; i < 8; i++)
        {
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 255);
            yield return new WaitForSeconds(0.1f);

            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 10);
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(gameObjectToDestroy);
        helpUsed = false;
    }
}
