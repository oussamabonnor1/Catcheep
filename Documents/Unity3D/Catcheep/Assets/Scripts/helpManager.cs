using System.Collections;
using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class helpManager : MonoBehaviour
{
    public GameObject[] helpTools;
    public GameObject[] helpButtons;
    public GameObject slider;

    public float timeToPlay;

    private GameObject helpGameObject;

    //sheepys and blackys are all the sheeps in the scene when the love button is clicked (affected without the others coming afterwards)
    private GameObject[] sheepys;

    private bool helpUsed;
    private bool helpToolIsReleased;
    private bool loveUsed;

    // Use this for initialization
    void Start()
    {
        settingHelpSuplies();
        
        helpUsed = false;
        helpToolIsReleased = true;

        if(slider == null) slider = GameObject.Find("Slider");
        slider.GetComponent<Slider>().value = 0;

        StartCoroutine(farmerHeadAnimation());
    }

    // Update is called once per frame
    void Update()
    {
        //don't use the bool helpUsed cause it won't let us performe a drag and drop when the next frame comes
        if (Input.GetMouseButton(0))
        {
            if (GameObject.FindGameObjectWithTag("net") == null && GameObject.FindGameObjectWithTag("hayStack") == null && GameObject.FindGameObjectWithTag("loveHelp") == null)
            {
                //if no help tool is used then u can create and use one
                //pointer is much like a raycast but UI related
                PointerEventData pointer = new PointerEventData(EventSystem.current);

                if (pointer.selectedObject == helpButtons[0] && PlayerPrefs.GetInt("hayStackStock") > 0)
                {
                    PlayerPrefs.SetInt("hayStackStock", PlayerPrefs.GetInt("hayStackStock") - 1);
                    helpButtons[0].gameObject.SetActive(false);
                    helpToolCreated(0);
                }

                if (pointer.selectedObject == helpButtons[1] && PlayerPrefs.GetInt("netStock") > 0)
                {
                    PlayerPrefs.SetInt("netStock", PlayerPrefs.GetInt("netStock") - 1);
                    helpButtons[1].gameObject.SetActive(false);
                    helpToolCreated(1);
                }

                //bool condition is there just not to instantiate the love 5 times 
                if (pointer.selectedObject == helpButtons[2] && !loveUsed && PlayerPrefs.GetInt("loveStock") > 0)
                {
                    PlayerPrefs.SetInt("loveStock", PlayerPrefs.GetInt("loveStock") - 1);
                    StartCoroutine(loveClickedcall());
                }
            }
            else if(!helpToolIsReleased)
            {
                helpToolDragDrop();
            }

        }
        //help must be already used so that this doesnt get called if player clicks on sheepys
        if (Input.GetMouseButtonUp(0) && helpUsed)
        {
            helpToolReleased(helpGameObject);
        }

        //very bad way to fix the shit of duplication but if it's stupid and it works, it's not stupid
        if(GameObject.FindGameObjectsWithTag("net").Length > 1)
        {
            Destroy(GameObject.FindGameObjectWithTag("net"));
        }
        if(GameObject.FindGameObjectsWithTag("hayStack").Length > 1)
        {
            Destroy(GameObject.FindGameObjectWithTag("hayStack"));
        }

        //just in case one of the tools isnt enabled
        if (helpToolIsReleased && helpGameObject != null)
        {
           if(helpGameObject.GetComponent<Collider2D>().enabled == false) helpGameObject.GetComponent<Collider2D>().enabled = true;
        }

        if (loveUsed)
        {
            loveUsedCall(sheepys);
        }
    }

    void helpToolCreated(int index)
    {
        gameManager.catchedSomething = true;
        GameObject.Find("Music Manager").GetComponent<music>().UISFX(4);
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 spawnPosition = new Vector3(position.x, position.y, 0f);
        
        helpUsed = true;
        helpToolIsReleased = false;

        helpGameObject = Instantiate(helpTools[index], spawnPosition,
            Quaternion.identity);
        helpGameObject.GetComponent<Collider2D>().enabled = false;
    }

    void helpToolDragDrop()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 spawnPosition = new Vector3(position.x, position.y, 0f);

        helpGameObject.transform.position = Vector3.Lerp(helpGameObject.transform.position,
            spawnPosition, Time.deltaTime * 8);
    }


    public void helpToolReleased(GameObject helpToolGameObject)
    {

        helpToolIsReleased = true;
        helpGameObject.GetComponent<Collider2D>().enabled = true;
        
        if (helpGameObject.tag == "hayStack")
        {
            StartCoroutine(helpDestroyer(8f, helpToolGameObject));
        }
        if (helpGameObject.tag == "net")
        {
            StartCoroutine(helpDestroyer(1f, helpToolGameObject));
        }
    }

    IEnumerator helpDestroyer(float lifeTime, GameObject gameObjectToDestroy)
    {
        // helpUsed must stay above wait, this is just to make sure no help tool gets left without destruction
        helpUsed = false;

        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObjectToDestroy);
        
        helpToolIsReleased = true;

        helpButtons[0].gameObject.SetActive(true);
        helpButtons[1].gameObject.SetActive(true);
        settingHelpSuplies();
    }

    IEnumerator farmerHeadAnimation()
    {
        float amoutToAdd = 0.1f / timeToPlay;
        
        while (slider.GetComponent<Slider>().value < 1)
        {
            yield return new WaitForSeconds(0.1f);
            slider.GetComponent<Slider>().value += amoutToAdd;
        }
        
        gameManager.gameOver = true;
    }

    public IEnumerator errorCall()
    {
        slider.GetComponent<Slider>().value += 0.025f;
        Color color = slider.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color;
        slider.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(0.5f);
        slider.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = color;
    }

    IEnumerator loveClickedcall()
    {

        GameObject.Find("Music Manager").GetComponent<music>().UISFX(4);
        gameManager.catchedSomething = true;
        loveUsed = true;

        GameObject loveGameObject = Instantiate(helpTools[2], transform.position, Quaternion.identity);
        yield return new WaitForSeconds(8f);
        Destroy(loveGameObject);

        loveUsed = false;
        sheepys = null;
        settingHelpSuplies();
    }

    //this function gets called once per frame when the help love is used
    void loveUsedCall(GameObject[] sheepys)
    {
        sheepys = GameObject.FindGameObjectsWithTag("sheepy");

        for (int i = 0; i < sheepys.Length; i++)
        {
            if (sheepys[i] != null)
            {
                if (sheepys[i].transform.position.y - transform.position.y < 50)
                {
                    int direction = 0;
                    if (sheepys[i].transform.position.x > transform.position.x)
                    {
                        direction = -1;
                    }
                    if (sheepys[i].transform.position.x < transform.position.x)
                    {
                        direction = 1;
                    }

                    sheepys[i].GetComponent<SheepMovement>().slideSpeed =
                        Mathf.Lerp(sheepys[i].GetComponent<SheepMovement>().slideSpeed, direction * 1, 0.1f);
                }
            }
        }
        
    }

    //this function gets called on start to give player info about how many help tools are left
    void settingHelpSuplies()
    {
        helpButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("hayStackStock");
        helpButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("netStock");
        helpButtons[2].GetComponentInChildren<TextMeshProUGUI>().text = "" + PlayerPrefs.GetInt("loveStock");
    }
}