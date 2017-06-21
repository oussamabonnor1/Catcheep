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

    // Use this for initialization
    void Start()
    {
        helpUsed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButton(0))
        {
            //pointer is much like a raycast but UI related
            PointerEventData pointer = new PointerEventData(EventSystem.current);

            if (pointer.selectedObject == helpButtons[0])
            {
                helpToolDragDrop(0);
            }

            if (pointer.selectedObject == helpButtons[1])
            {
                helpToolDragDrop(1);
            }
        }
        //help must be already used so that this doesnt get called if player clicks on sheepys
        if (Input.GetMouseButtonUp(0) && helpUsed)
        {
            helpToolCreation(helpGameObject);
        }
    }

    public void helpToolDragDrop(int index)
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 spawnPosition = new Vector3(position.x, position.y, 0f);

        if (!helpUsed)
        {
            helpUsed = true;
            helpGameObject = Instantiate(helpTools[index],spawnPosition ,
                Quaternion.identity);
        }

        helpGameObject.transform.position = Vector3.Lerp(helpGameObject.transform.position,
          spawnPosition, Time.deltaTime * 5);
    }


    public void helpToolCreation(GameObject helpToolGameObject)
    {
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
    }
}