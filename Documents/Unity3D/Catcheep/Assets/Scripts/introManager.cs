using System.Collections;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class introManager : MonoBehaviour
{
    public GameObject mainPanel;
    
    public GameObject textBG;
    public GameObject[] textIteams;

    public GameObject ImagesBG;
    public GameObject[] ImagesIteams;
    private music musicManager;

    public int index;
    // Use this for initialization
    void Start ()
    {
        PlayerPrefs.SetInt("netStock",1);
        PlayerPrefs.SetInt("hayStackStock",1);
        PlayerPrefs.SetInt("loveStock",1);
        musicManager = GameObject.Find("Music Manager").GetComponent<music>();
        musicManager.UISFX(1);
        StartCoroutine(objectOpened(mainPanel));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void manager()
    {
        switch (index)
        {
            case 0:
                textBG.SetActive(true);
                textIteams[index].SetActive(true);
                break;
            case 1:
                textBG.SetActive(true);
                textIteams[index].SetActive(true);
                break;
            case 2:
                ImagesBG.SetActive(true);
                ImagesIteams[0].SetActive(true);
                break;
            case 3:
                ImagesBG.SetActive(true);
                ImagesIteams[1].SetActive(true);
                break;
            case 4:
                textBG.SetActive(true);
                textIteams[2].SetActive(true);
                break;
            case 5:
                textBG.SetActive(true);
                textIteams[3].SetActive(true);
                break;
            case 6:
                ImagesBG.SetActive(true);
                ImagesIteams[2].SetActive(true);
                break;
            case 7:
                textBG.SetActive(true);
                textIteams[4].SetActive(true);
                break;
            case 8:
                ImagesBG.SetActive(true);
                ImagesIteams[3].SetActive(true);
                break;
            case 9:
                textBG.SetActive(true);
                textIteams[5].SetActive(true);
                break;
            case 10:
                textBG.SetActive(true);
                textIteams[6].SetActive(true);
                break;
            default:
                killEverything();
                StartCoroutine(objectClosed(mainPanel));
                break;
        }
    }

    public void forward()
    {
        musicManager.UISFX(0);
        killEverything();
        index += 1;
        manager();
    }
    public void back()
    {
        musicManager.UISFX(0);
        killEverything();
        if(index > 0) index -= 1;
        manager();
    }

    void killEverything()
    {
        textBG.SetActive(false);
        ImagesBG.SetActive(false);
        for (int i = 0; i < textIteams.Length; i++)
        {
         textIteams[i].SetActive(false);   
        }
        for (int i = 0; i < ImagesIteams.Length; i++)
        {
            ImagesIteams[i].SetActive(false);
        }
    }

    IEnumerator objectOpened(GameObject objectToOpen)
    {
        objectToOpen.SetActive(true);
        for (int i = 0; i <= 10; i++)
        {
            float a = (float)i / 10;
            objectToOpen.transform.localScale = new Vector3(a, a, 1);
            yield return new WaitForSeconds(0.01f);
        }
        manager();
    }

    IEnumerator objectClosed(GameObject objectToOpen)
    {
        for (int i = 10; i >= 0; i--)
        {
            float a = (float)i / 10;
            objectToOpen.transform.localScale = new Vector3(a, a, 1);
            yield return new WaitForSeconds(0.01f);
        }
        objectToOpen.SetActive(false);
        PlayerPrefs.SetInt("intro",1);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(1);
    }
}
