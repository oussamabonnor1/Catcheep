using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutoriel : MonoBehaviour
{
    public GameObject TutoGameObject;
    public GameObject textBox;
    public GameObject shopSettingsImage;
    public GameObject sheepImage;

	// Use this for initialization
	void Start ()
	{
        if (PlayerPrefs.GetInt("tuto") < 10)
	    {
	        //PlayerPrefs.SetInt("tuto",0);
            TutoGameObject.SetActive(true);
            if(SceneManager.GetActiveScene().name.Equals("Start")) startSceneTutoriel();
	        if (SceneManager.GetActiveScene().name.Equals("Farm")) FarmSceneTutoriel();
	        if (SceneManager.GetActiveScene().name.Equals("Alien")) AlienSceneTutoriel();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void startSceneTutoriel()
    {
        if (PlayerPrefs.GetInt("tuto") == 0)
        {
            textBox.GetComponentInChildren<TextMeshProUGUI>().text = "Hey There !";
            PlayerPrefs.SetInt("tuto", PlayerPrefs.GetInt("tuto") + 1);
            return;
        }
        if (PlayerPrefs.GetInt("tuto") == 1)
        {
            textBox.GetComponentInChildren<TextMeshProUGUI>().text = "To Catch sheeps: Click On the Barn Below...";
            return;
        }
        if (PlayerPrefs.GetInt("tuto") == 5)
        {
            textBox.GetComponentInChildren<TextMeshProUGUI>().text = "You should have some sheeps by Now !";
            PlayerPrefs.SetInt("tuto", PlayerPrefs.GetInt("tuto") + 1);
            return;
        }
        if (PlayerPrefs.GetInt("tuto") == 6)
        {
            textBox.GetComponentInChildren<TextMeshProUGUI>().text = "To Sell Sheeps: Click on The Space Ship Below...";
            return;
        }
        if (PlayerPrefs.GetInt("tuto") == 9)
        {
            textBox.GetComponentInChildren<TextMeshProUGUI>().text = "Visit the shop or setting using the panels below...";
            PlayerPrefs.SetInt("tuto", PlayerPrefs.GetInt("tuto") + 1);
            shopSettingsImage.SetActive(true);
            return;
        }
    }

    public void FarmSceneTutoriel()
    {
        if (PlayerPrefs.GetInt("tuto") == 2)
        {
            textBox.GetComponentInChildren<TextMeshProUGUI>().text = "To Catch sheeps: Click on Them !";
            PlayerPrefs.SetInt("tuto", PlayerPrefs.GetInt("tuto") + 1);
            return;
        }
        if (PlayerPrefs.GetInt("tuto") == 3)
        {
            textBox.GetComponentInChildren<TextMeshProUGUI>().text = "You can use help tools to catch them";
            PlayerPrefs.SetInt("tuto", PlayerPrefs.GetInt("tuto") + 1);
            shopSettingsImage.SetActive(true);
            return;
        }
        if (PlayerPrefs.GetInt("tuto") == 4)
        {
            textBox.GetComponentInChildren<TextMeshProUGUI>().text = "You can Buy help tools at the shop !";
            PlayerPrefs.SetInt("tuto", PlayerPrefs.GetInt("tuto") + 1);
            StartCoroutine(hideTuto());
            return;
        }

    }

    public void AlienSceneTutoriel()
    {
        if (PlayerPrefs.GetInt("tuto") == 7)
        {
            textBox.GetComponentInChildren<TextMeshProUGUI>().text = "To Sell sheeps: Click on Ship !";
            PlayerPrefs.SetInt("tuto", PlayerPrefs.GetInt("tuto") + 1);
            return;
        }
        if (PlayerPrefs.GetInt("tuto") == 8)
        {
            textBox.GetComponentInChildren<TextMeshProUGUI>().text = "Your Mission: Click on Mail";
            PlayerPrefs.SetInt("tuto", PlayerPrefs.GetInt("tuto") + 1);
            shopSettingsImage.SetActive(true);
            return;
        }
        if (PlayerPrefs.GetInt("tuto") == 9)
        {
            textBox.GetComponentInChildren<TextMeshProUGUI>().text = "Click On Sign To see your Sheeps !";
            sheepImage.SetActive(true);
            StartCoroutine(hideTuto());
            return;
        }

    }

    IEnumerator hideTuto()
    {
        yield return new WaitForSeconds(2f);
        if(SceneManager.GetActiveScene().name.Equals("Farm")) StartCoroutine(GameObject.Find("Game Manager").GetComponent<gameManager>().sheepSpawner());
        TutoGameObject.SetActive(false);
    }
}
