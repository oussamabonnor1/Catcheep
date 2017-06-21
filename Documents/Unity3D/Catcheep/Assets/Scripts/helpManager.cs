using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class helpManager : MonoBehaviour
{
    public GameObject[] helpTools;
    public GameObject[] helpButtons;

    private GameObject hayGameObject;
    private GameObject netGameObject;

    private bool helpUsed;

    // Use this for initialization
    void Start()
    {
        helpUsed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && GameObject.FindWithTag("net"))
        {
            GameObject.FindWithTag("net").transform.position =
                Vector3.Lerp(GameObject.FindWithTag("net").transform.position,
                    Camera.main.ScreenToWorldPoint(Input.mousePosition), Time.deltaTime * 5);
        }


        if (Input.GetMouseButton(0))
        {
            PointerEventData pointer = new PointerEventData(EventSystem.current);

            if (pointer.selectedObject == helpButtons[0])
            {
                hayStackDragDrop();
            }
        }

        if (Input.GetMouseButtonUp(0) && helpUsed)
        {
            hayStackCreation(hayGameObject);
        }
    }

    public void hayStackDragDrop()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 spawnPosition = new Vector3(position.x, position.y, 0f);

        if (!helpUsed)
        {
            helpUsed = true;
           
            hayGameObject = Instantiate(helpTools[0],spawnPosition ,
                Quaternion.identity);
        }

        hayGameObject.transform.position = Vector3.Lerp(hayGameObject.transform.position,
          spawnPosition, Time.deltaTime * 5);

    }


    public void hayStackCreation(GameObject hayGameObject)
    {
        StartCoroutine(helpDestroyer(5f, hayGameObject));
    }

    public void net()
    {
        if (!helpUsed)
        {
            helpUsed = true;
            netGameObject = Instantiate(helpTools[1], transform.position, Quaternion.identity);
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