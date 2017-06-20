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
	    if (Input.GetMouseButton(0) && GameObject.FindWithTag("net"))
	    {
	        GameObject.FindWithTag("net").transform.position = Vector3.Lerp(GameObject.FindWithTag("net").transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Time.deltaTime * 5);
	    }

	    if (GameObject.FindWithTag("hayStack"))
	    {
	        //GameObject.FindWithTag("hayStack").transform.position = Vector3.Lerp(GameObject.FindWithTag("hayStack").transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Time.deltaTime * 5);
	    }
    }

    public void hayStack()
    {
        if (!helpUsed)
        {
            helpUsed = true;
            GameObject heyGameObject = Instantiate(helpTools[0], transform.position, Quaternion.identity);
            StartCoroutine(helpDestroyer(5f, heyGameObject));
        }
    }

    public void net()
    {
        if (!helpUsed)
        {
            helpUsed = true;
            GameObject netGameObject = Instantiate(helpTools[1], transform.position, Quaternion.identity);
            StartCoroutine(helpDestroyer(3f, netGameObject));
        }
    }

    IEnumerator helpDestroyer(float lifeTime, GameObject gameObjectToDestroy)
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObjectToDestroy);
        helpUsed = false;
    }
}
