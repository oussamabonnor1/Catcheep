using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class helpManager : MonoBehaviour
{
    public GameObject[] helpTools;
    public GameObject[] helpButtons;

    private GameObject helpGameObject;

    private bool helpUsed;
    private bool helpToolIsReleased;

    // Use this for initialization
    void Start()
    {
        helpUsed = false;
        helpToolIsReleased = true;
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
                // if (!helpUsed && helpToolIsReleased)
                //{
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
                //if help tool is released you can't drag and drop anything till it s destroyed
                //if (!helpToolIsReleased && helpGameObject) 
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
            spawnPosition, Time.deltaTime * 5);
    }


    public void helpToolReleased(GameObject helpToolGameObject)
    {
        helpToolIsReleased = true;
        helpGameObject.GetComponent<Collider2D>().enabled = true;
        

        if (helpGameObject.tag == "hayStack")
        {
            StartCoroutine(helpDestroyer(5f, helpToolGameObject));
        }
        if (helpGameObject.tag == "net")
        {
            StartCoroutine(helpDestroyer(3f, helpToolGameObject));
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
}