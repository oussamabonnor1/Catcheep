using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutoriel : MonoBehaviour
{
    public GameObject TutoGameObject;
    public GameObject textBox;
    public GameObject firstImage;
    public GameObject secondImage;
    public GameObject thirdImage;

	// Use this for initialization
    void Start()
    {
        if (PlayerPrefs.GetInt("tuto") < 10)
        {
            //PlayerPrefs.SetInt("tuto",0);
            if (SceneManager.GetActiveScene().name.Equals("Start")) startSceneTutoriel();
            if (SceneManager.GetActiveScene().name.Equals("Farm")) FarmSceneTutoriel();
            if (SceneManager.GetActiveScene().name.Equals("Alien")) AlienSceneTutoriel();
        }
        else
        {
            TutoGameObject.SetActive(false);
        }
}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void startSceneTutoriel()
    {
        textBox.SetActive(true);
        textBox.GetComponentInChildren<TextMeshProUGUI>().text = "Click on the red circle !";
        if (PlayerPrefs.GetInt("tuto") == 0)
        {
            firstImage.SetActive(true);
            StartCoroutine(incrementTuto(true));
            return;
        }
        else if (PlayerPrefs.GetInt("tuto") == 4)
        {
            secondImage.SetActive(true);
            StartCoroutine(incrementTuto(true));
            return;
        }
        else if(PlayerPrefs.GetInt("tuto") == 8)
        {
            thirdImage.SetActive(true);
            StartCoroutine(incrementTuto(true));
            return;
        }
        else
        {
            TutoGameObject.SetActive(false);
        }
    }

    public void FarmSceneTutoriel()
    {
        textBox.SetActive(true);
        textBox.GetComponentInChildren<TextMeshProUGUI>().text = "Click on the red circle !";
        if (PlayerPrefs.GetInt("tuto") == 2)
        {
            firstImage.SetActive(true);
            StartCoroutine(incrementTuto(false));
            return;
        }
        else if (PlayerPrefs.GetInt("tuto") == 3)
        {
            secondImage.SetActive(true);
            if (SceneManager.GetActiveScene().name.Equals("Farm"))
            {
                GameObject.Find("Game Manager").GetComponent<gameManager>().tutorielFinished = true;
            }
            StartCoroutine(incrementTuto(true));
            return;
        }
        else
        {
            TutoGameObject.SetActive(false);
        }
    }

    public void AlienSceneTutoriel()
    {
        textBox.SetActive(true);
        textBox.GetComponentInChildren<TextMeshProUGUI>().text = "Click on the red circle !";
        if (PlayerPrefs.GetInt("tuto") == 5)
        {
            firstImage.SetActive(true);
            StartCoroutine(incrementTuto(false));
            return;
        }
        else if (PlayerPrefs.GetInt("tuto") == 6)
        {
            secondImage.SetActive(true);
            StartCoroutine(incrementTuto(false));
            return;
        }
        else if (PlayerPrefs.GetInt("tuto") == 7)
        {
            thirdImage.SetActive(true);
            StartCoroutine(incrementTuto(true));
            return;
        }
        else
        {
            TutoGameObject.SetActive(false);
        }
    }

    IEnumerator hideTuto()
    {
        yield return new WaitForSeconds(2f);
        firstImage.SetActive(false);
        secondImage.SetActive(false);
        thirdImage.SetActive(false);
        TutoGameObject.SetActive(false);
    }

    IEnumerator incrementTuto(bool state)
    {
        yield return new WaitForSeconds(3);
        PlayerPrefs.SetInt("tuto", PlayerPrefs.GetInt("tuto") + 1);
        if (state) StartCoroutine(hideTuto());
        if (SceneManager.GetActiveScene().name.Equals("Start")) startSceneTutoriel();
        if (SceneManager.GetActiveScene().name.Equals("Farm")) FarmSceneTutoriel();
        if (SceneManager.GetActiveScene().name.Equals("Alien")) AlienSceneTutoriel();

    }
}
