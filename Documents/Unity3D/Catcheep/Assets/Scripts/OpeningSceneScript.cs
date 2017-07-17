using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningSceneScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    StartCoroutine(opening());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator opening()
    {
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene(1);
    }
}
