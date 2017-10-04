using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningSceneScript : MonoBehaviour
{
    private int i;

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        i++;
        if (i > 5 && i < 7) StartCoroutine(opening());
    }

    IEnumerator opening()
    {
        yield return new WaitForSeconds(0.5f);
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
