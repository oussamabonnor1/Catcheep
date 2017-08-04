using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AlienManager : MonoBehaviour
{
    public Sprite[] SheepSprites;
    public GameObject[] alienShip;
    public GameObject rightButton;
    public GameObject leftButton;
    public GameObject sheepHolder;

    private GameObject spaceShipFroScript;
    private Vector2 edgeOfScreen;
    private Vector2 oldPosition;
    private Vector2 newPosition;
    private int currentSheepShowed;

    private int shipType;
    // Use this for initialization
    void Start()
    {
        if (PlayerPrefs.GetInt("ship") == 0)
        {
            PlayerPrefs.SetInt("ship",1);
        }
        shipType = PlayerPrefs.GetInt("ship") - 1;

        currentSheepShowed = -1;
        changingSheepPic(1);
        edgeOfScreen = new Vector2(Screen.width, Screen.height);
        StartCoroutine(alienSpawner());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            //when player presses, save that click position
            oldPosition = Input.mousePosition;
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            //when player lets go, save that position
            newPosition = Input.mousePosition;
        }
        //if both positions aren't null
        if (oldPosition != new Vector2(0f, 0f) && newPosition != new Vector2(0f, 0f))
        {
            if (Vector2.Distance(oldPosition, newPosition) >= (edgeOfScreen.x * 0.2f) &&
                Mathf.Abs(newPosition.y - oldPosition.y) < (edgeOfScreen.y * 0.07f))
            {
                if (oldPosition.x < newPosition.x)
                {
                    shipGoingRightButtonClicked();
                }
                else
                {
                    shipGoingLeftButtonClicked();
                }
                oldPosition = new Vector2(0f, 0f);
                newPosition = new Vector2(0f, 0f);
            }
        }

        if (Input.touchCount == 0)
        {
            oldPosition = new Vector2(0f, 0f);
            newPosition = new Vector2(0f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            startMenu();
        }
    }

    IEnumerator alienSpawner()
    {
        deactivatingButtons();

        spaceShipFroScript = Instantiate(alienShip[shipType], alienShip[shipType].transform.localPosition, alienShip[shipType].transform.rotation);
        spaceShipFroScript.transform.SetParent(GameObject.Find("Canvas").transform, false);

        Vector3 destination = new Vector3(alienShip[shipType].transform.localPosition.x, -edgeOfScreen.y * 0.05f, 0f);
        do
        {
            spaceShipFroScript.transform.localPosition =
                Vector3.Lerp(spaceShipFroScript.transform.localPosition, destination, 2.5f * Time.deltaTime);
            yield return new WaitForSeconds(0.02f);
        } while ((int) spaceShipFroScript.transform.localPosition.y > 0f);
        activatingButtons();
    }
    IEnumerator shipLeaving()
    {
        deactivatingButtons();

        Vector3 destination = new Vector3(alienShip[shipType].transform.localPosition.x, edgeOfScreen.y, 0f);
        do
        {
            spaceShipFroScript.transform.localPosition =
                Vector3.Lerp(spaceShipFroScript.transform.localPosition, destination, 2.5f * Time.deltaTime);
            yield return new WaitForSeconds(0.02f);
        } while (spaceShipFroScript.transform.localPosition.y < destination.y);

        Destroy(spaceShipFroScript.gameObject);
        activatingButtons();
    }

    public void shipGoingRightButtonClicked()
    {
        StartCoroutine(shipGoingRight());
    }
    IEnumerator shipGoingRight()
    {
        deactivatingButtons();
        //spaceShipFroScript.transform.localRotation = new Quaternion(spaceShipFroScript.transform.localRotation.x, spaceShipFroScript.transform.rotation.y, -5, spaceShipFroScript.transform.rotation.w);
        Vector3 destination = new Vector3(
            edgeOfScreen.x + alienShip[shipType].GetComponentInChildren<Image>().sprite.bounds.size.x,
            spaceShipFroScript.transform.localPosition.y,
            spaceShipFroScript.transform.localPosition.z);
        do
        {
            spaceShipFroScript.transform.localPosition = new Vector3(spaceShipFroScript.transform.localPosition.x + 10,
                spaceShipFroScript.transform.localPosition.y, spaceShipFroScript.transform.localPosition.z);
            yield return new WaitForSeconds(0.01f);
        } while ((int)spaceShipFroScript.transform.position.x < (int)destination.x * 1.3f);
        Destroy(spaceShipFroScript.gameObject);
        changingSheepPic(1);
        StartCoroutine(alienSpawner());
    }

    public void shipGoingLeftButtonClicked()
    {
        StartCoroutine(shipGoingLeft());
    }
    IEnumerator shipGoingLeft()
    {
        deactivatingButtons();
        //spaceShipFroScript.transform.localRotation = new Quaternion(spaceShipFroScript.transform.localRotation.x, spaceShipFroScript.transform.rotation.y, -5, spaceShipFroScript.transform.rotation.w);
        Vector3 destination = new Vector3(
           -edgeOfScreen.x - alienShip[shipType].GetComponentInChildren<Image>().sprite.bounds.size.x,
            spaceShipFroScript.transform.localPosition.y,
            spaceShipFroScript.transform.localPosition.z);
        do
        {
            spaceShipFroScript.transform.localPosition = new Vector3(spaceShipFroScript.transform.localPosition.x - 10,
                spaceShipFroScript.transform.localPosition.y, spaceShipFroScript.transform.localPosition.z);
            yield return new WaitForSeconds(0.01f);
        } while ((int) spaceShipFroScript.transform.position.x > (int) destination.x * 0.3f);
        Destroy(spaceShipFroScript.gameObject);
        changingSheepPic(-1);
        StartCoroutine(alienSpawner());
    }

    void changingSheepPic(int i)
    {
        currentSheepShowed += i;
        if (currentSheepShowed >= SheepSprites.Length)
        {
            currentSheepShowed = 0;
        }
        if (currentSheepShowed < 0)
        {
            currentSheepShowed = SheepSprites.Length - 1;
        }
        if (currentSheepShowed < SheepSprites.Length && SheepSprites[currentSheepShowed] != null)
        {
            sheepHolder.GetComponent<Image>().sprite = SheepSprites[currentSheepShowed];
        }
        
    }

    void activatingButtons()
    {
        
        rightButton.GetComponent<Button>().enabled = true;
        leftButton.GetComponent<Button>().enabled = true;
    }
    void deactivatingButtons()
    {
        rightButton.GetComponent<Button>().enabled = false;
        leftButton.GetComponent<Button>().enabled = false;
    }
    
    public void startMenu()
    {
        SceneManager.LoadScene(1);
    }
}