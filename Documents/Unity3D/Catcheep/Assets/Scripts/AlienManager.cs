using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AlienManager : MonoBehaviour
{

    public GameObject alienShip;

    private GameObject spaceShip;
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
        spaceShip = Instantiate(alienShip, alienShip.transform.localPosition, alienShip.transform.rotation);
        spaceShip.transform.SetParent(GameObject.Find("Canvas").transform, false);

        Vector3 destination = new Vector3(alienShip.transform.localPosition.x, -edgeOfScreen.y * 0.05f, 0f);
        do
        {
            spaceShip.transform.localPosition = Vector3.Lerp(spaceShip.transform.localPosition, destination, 2.5f * Time.deltaTime);
            yield return new WaitForSeconds(0.02f);
        } while ((int) spaceShip.transform.localPosition.y > (int) destination.y + 1);
        
        StartCoroutine(shipGoingRight());
    }

    IEnumerator shipLeaving()
    {
        Vector3 destination = new Vector3(alienShip.transform.localPosition.x, edgeOfScreen.y, 0f);
        do
        {
            spaceShip.transform.localPosition = Vector3.Lerp(spaceShip.transform.localPosition, destination, 2.5f * Time.deltaTime);
            yield return new WaitForSeconds(0.02f);
        } while (spaceShip.transform.localPosition.y < destination.y);

        Destroy(spaceShip.gameObject);
    }

    public IEnumerator shipGoingRight()
    {
        Vector3 destination = new Vector3(edgeOfScreen.x + alienShip.GetComponentInChildren<Image>().sprite.bounds.size.x, spaceShip.transform.localPosition.y,
            spaceShip.transform.localPosition.z);
        //print(spaceShip.transform.localPosition.x +" / " + spaceShip.transform.position.x);
       // destination = Camera.main.WorldToScreenPoint(destination);
        do
        {
            spaceShip.transform.localPosition = new Vector3(spaceShip.transform.localPosition.x + 10, spaceShip.transform.localPosition.y, spaceShip.transform.localPosition.z);
            yield return new WaitForSeconds(0.01f);
        } while ((int) spaceShip.transform.position.x < (int) destination.x * 1.5f);
    }

    public void startMenu()
    {
        SceneManager.LoadScene(1);
    }
}
