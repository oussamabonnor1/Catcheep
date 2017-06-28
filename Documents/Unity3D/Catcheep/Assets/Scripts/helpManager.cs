using System.Collections;
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

    private bool helpUsed;
    private bool helpToolIsReleased;

    // Use this for initialization
    void Start()
    {
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
            if (GameObject.FindGameObjectWithTag("net") == null && GameObject.FindGameObjectWithTag("hayStack") == null)
            {
                //if no help tool is used then u can create and use one
                //pointer is much like a raycast but UI related
                PointerEventData pointer = new PointerEventData(EventSystem.current);

                if (pointer.selectedObject == helpButtons[0])
                {
                    helpButtons[0].gameObject.SetActive(false);
                    helpToolCreated(0);
                }

                if (pointer.selectedObject == helpButtons[1])
                {
                    helpButtons[1].gameObject.SetActive(false);
                    helpToolCreated(1);
                }
                //}
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
    }

    void helpToolCreated(int index)
    {
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
            StartCoroutine(helpDestroyer(3f, helpToolGameObject));
        }
        if (helpGameObject.tag == "net")
        {
            StartCoroutine(helpDestroyer(1f, helpToolGameObject));
        }
    }

    IEnumerator helpDestroyer(float lifeTime, GameObject gameObjectToDestroy)
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObjectToDestroy);
        helpUsed = false;
        helpToolIsReleased = true;

        helpButtons[0].gameObject.SetActive(true);
        helpButtons[1].gameObject.SetActive(true);
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
}