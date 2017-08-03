using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AlienManager : MonoBehaviour
{

    public GameObject alienShip;

    private Vector2 edgeOfScreen;

	// Use this for initialization
	void Start () {
		edgeOfScreen = new Vector2(Screen.width,Screen.height);
	    StartCoroutine(alienSpawner());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator alienSpawner()
    {
        GameObject spaceShip = Instantiate(alienShip, alienShip.transform.localPosition, alienShip.transform.rotation);
        yield return new WaitForSeconds(1);
        spaceShip.transform.parent = GameObject.Find("Canvas").transform;
        //next line is necessary, plz dnt remove it when your bored
        spaceShip.transform.localPosition = alienShip.transform.localPosition;
        /*Vector3 destination = new Vector3(0f,edgeOfScreen.y * 0.2f, 0f);
        do
        {
            spaceShip.transform.localPosition = Vector3.Lerp(spaceShip.transform.localPosition, destination,0.1f * Time.deltaTime);
            yield return new WaitForSeconds(0.5f);
        } while (spaceShip.transform.localPosition.y > (edgeOfScreen.y * 0.2));
        */
    }

    public void startMenu()
    {
        SceneManager.LoadScene(1);
    }
}
