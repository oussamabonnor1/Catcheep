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
    public GameObject fourthImage;

	// Use this for initialization
    void Start()
    {
        if (PlayerPrefs.GetInt("tuto") < 10)
        {
            if (SceneManager.GetActiveScene().name.Equals("Start")) startSceneTutoriel();
            else if (SceneManager.GetActiveScene().name.Equals("Farm")) FarmSceneTutoriel();
            else if (SceneManager.GetActiveScene().name.Equals("Alien")) AlienSceneTutoriel();
            else TutoGameObject.SetActive(false);
        }
        else
        {
            if(!SceneManager.GetActiveScene().name.Equals("Alien")) firstImage.SetActive(false);
            secondImage.SetActive(false);
            thirdImage.SetActive(false);
            TutoGameObject.SetActive(false);
        }
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
            textBox.GetComponentInChildren<TextMeshProUGUI>().text = "Click on the space ship !";
            secondImage.SetActive(true);
            StartCoroutine(incrementTuto(true));
            return;
        }
        else if(PlayerPrefs.GetInt("tuto") == 8)
        {
            thirdImage.SetActive(true);
            StartCoroutine(incrementTuto(false));
            return;
        }
        else if (PlayerPrefs.GetInt("tuto") == 9)
        {
            fourthImage.SetActive(true);
            StartCoroutine(incrementTuto(true));
            return;
        }
        else
        {
            firstImage.SetActive(false);
            secondImage.SetActive(false);
            thirdImage.SetActive(false);
            fourthImage.SetActive(false);
            TutoGameObject.SetActive(false);
        }
    }

    public void FarmSceneTutoriel()
    {
        textBox.SetActive(true);
        textBox.GetComponentInChildren<TextMeshProUGUI>().text = "Click on the SHEEPS !";
        if (PlayerPrefs.GetInt("tuto") == 2)
        {
            firstImage.SetActive(true);
            StartCoroutine(incrementTuto(false));
            return;
        }
        else if (PlayerPrefs.GetInt("tuto") == 3)
        {
            textBox.GetComponentInChildren<TextMeshProUGUI>().text = "Drag the help tools to use them";
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
            firstImage.SetActive(false);
            secondImage.SetActive(false);
            thirdImage.SetActive(false);
            TutoGameObject.SetActive(false);
        }
    }

    public void AlienSceneTutoriel()
    {
        if (PlayerPrefs.GetInt("tuto") == 5)
        {
            GameObject.Find("Manager").GetComponent<AlienManager>().infoPanel.SetActive(true);
            StartCoroutine(incrementTuto(false));
            return;
        }
        else if (PlayerPrefs.GetInt("tuto") == 6)
        {
            PlayerPrefs.SetInt("tuto", PlayerPrefs.GetInt("tuto") + 1);
            AlienSceneTutoriel();
            return;
        }
        else if (PlayerPrefs.GetInt("tuto") == 7)
        {
            PlayerPrefs.SetInt("tuto", PlayerPrefs.GetInt("tuto") + 1);
            AlienSceneTutoriel();
            return;
        }
        else
        {
            //firstImage.SetActive(false);
            secondImage.SetActive(false);
            thirdImage.SetActive(false);
            fourthImage.SetActive(false);
            TutoGameObject.SetActive(false);
        }
    }

    IEnumerator hideTuto()
    {
        yield return new WaitForSeconds(2f);
        firstImage.SetActive(false);
        secondImage.SetActive(false);
        thirdImage.SetActive(false);
        fourthImage.SetActive(false);
        TutoGameObject.SetActive(false);
    }

    IEnumerator incrementTuto(bool state)
    {
        PlayerPrefs.SetInt("tuto", PlayerPrefs.GetInt("tuto") + 1);
        yield return new WaitForSeconds(3);
        if (state) StartCoroutine(hideTuto());
        if (SceneManager.GetActiveScene().name.Equals("Start")) startSceneTutoriel();
        if (SceneManager.GetActiveScene().name.Equals("Farm")) FarmSceneTutoriel();
        if (SceneManager.GetActiveScene().name.Equals("Alien")) AlienSceneTutoriel();

    }
}
