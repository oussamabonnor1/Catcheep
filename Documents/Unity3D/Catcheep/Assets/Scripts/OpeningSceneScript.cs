using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class OpeningSceneScript : MonoBehaviour
{
    private int i;

    public GameObject Text;
    public string[] hints;

	// Use this for initialization
	void Start ()
	{
	    GameObject.Find("Music Manager").GetComponent<PlayServicesMyVersion>().signIn();
        Text.GetComponent<TextMeshProUGUI>().text = hints[Random.Range(0,hints.Length)];
            StartCoroutine(opening());
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    IEnumerator opening()
    {
        yield return new WaitForSeconds(3.5f);
        if (PlayerPrefs.GetInt("intro") == 0)
        {
            SceneManager.LoadScene(8);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }
}