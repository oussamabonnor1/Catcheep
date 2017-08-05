using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AlienManager : MonoBehaviour
{
    [Header("In game objects")]
    public Sprite[] SheepSprites;
    public GameObject[] alienShip;
    [Header("UI components")]
    public GameObject rightButton;
    public GameObject leftButton;
    [Header("indication elements")]
    public GameObject wantedText;
    public GameObject sheepHolder;

    private GameObject spaceShipForScript;
    private Vector2 edgeOfScreen;
    private Vector2 oldPosition;
    private Vector2 newPosition;
    private int currentSheepShowed;
    private int sheepyRequested;
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

        spaceShipForScript = Instantiate(alienShip[shipType], alienShip[shipType].transform.localPosition, alienShip[shipType].transform.rotation);
        spaceShipForScript.transform.SetParent(GameObject.Find("Canvas").transform, false);
        spaceShipForScript.GetComponentInChildren<Button>().onClick.AddListener(call: shipClicked);

        Vector3 destination = new Vector3(alienShip[shipType].transform.localPosition.x, -edgeOfScreen.y * 0.05f, 0f);
        do
        {
            spaceShipForScript.transform.localPosition =
                Vector3.Lerp(spaceShipForScript.transform.localPosition, destination, 2.5f * Time.deltaTime);
            yield return new WaitForSeconds(0.02f);
        } while ((int) spaceShipForScript.transform.localPosition.y > 0f);
        activatingButtons();
    }

    void shipClicked()
    {
        if (PlayerPrefs.GetInt("sheepy") >= sheepyRequested)
        {
            spaceShipForScript.transform.GetChild(1).gameObject.SetActive(false);
            spaceShipForScript.transform.GetChild(1).gameObject.SetActive(true);
            spaceShipForScript.transform.GetChild(0).gameObject.SetActive(true);



            //StartCoroutine(shipLeaving());
        }
        else
        {
            
        }
    }
    IEnumerator shipLeaving()
    {
        deactivatingButtons();
        Vector3 destination = new Vector3(alienShip[shipType].transform.localPosition.x, edgeOfScreen.y * 0.5f, 0f);
        do
        {
            spaceShipForScript.transform.localPosition =
                Vector3.Lerp(spaceShipForScript.transform.localPosition, destination, 2.5f * Time.deltaTime);
            yield return new WaitForSeconds(0.02f);
        } while ((int) spaceShipForScript.transform.localPosition.y < (int) destination.y * 0.7f);
        Destroy(spaceShipForScript.gameObject);
        activatingButtons();
    }

    public void shipGoingRightButtonClicked()
    {
        StartCoroutine(shipGoingRight());
    }
    IEnumerator shipGoingRight()
    {
        if (spaceShipForScript != null)
        {
            deactivatingButtons();
            //spaceShipForScript.transform.localRotation = new Quaternion(spaceShipForScript.transform.localRotation.x, spaceShipForScript.transform.rotation.y, -5, spaceShipForScript.transform.rotation.w);
            Vector3 destination = new Vector3(
                edgeOfScreen.x + alienShip[shipType].GetComponentInChildren<Image>().sprite.bounds.size.x,
                spaceShipForScript.transform.localPosition.y,
                spaceShipForScript.transform.localPosition.z);
            do
            {
                spaceShipForScript.transform.localPosition = new Vector3(
                    spaceShipForScript.transform.localPosition.x + 10,
                    spaceShipForScript.transform.localPosition.y, spaceShipForScript.transform.localPosition.z);
                yield return new WaitForSeconds(0.01f);
            } while ((int) spaceShipForScript.transform.position.x < (int) destination.x * 1.3f);
            Destroy(spaceShipForScript.gameObject);
        }
        changingSheepPic(1);
        StartCoroutine(alienSpawner());
    }

    public void shipGoingLeftButtonClicked()
    {
        StartCoroutine(shipGoingLeft());
    }
    IEnumerator shipGoingLeft()
    {
        if (spaceShipForScript != null)
        {
            deactivatingButtons();
            //spaceShipForScript.transform.localRotation = new Quaternion(spaceShipForScript.transform.localRotation.x, spaceShipForScript.transform.rotation.y, -5, spaceShipForScript.transform.rotation.w);
            Vector3 destination = new Vector3(
                -edgeOfScreen.x - alienShip[shipType].GetComponentInChildren<Image>().sprite.bounds.size.x,
                spaceShipForScript.transform.localPosition.y,
                spaceShipForScript.transform.localPosition.z);
            do
            {
                spaceShipForScript.transform.localPosition = new Vector3(
                    spaceShipForScript.transform.localPosition.x - 10,
                    spaceShipForScript.transform.localPosition.y, spaceShipForScript.transform.localPosition.z);
                yield return new WaitForSeconds(0.01f);
            } while ((int) spaceShipForScript.transform.position.x > (int) destination.x * 0.3f);
            Destroy(spaceShipForScript.gameObject);
        }
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