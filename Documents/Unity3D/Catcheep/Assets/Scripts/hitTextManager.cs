using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class hitTextManager : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    GetComponent<TextMeshProUGUI>().text = "Hit x " + gameManager.combo;
	    StartCoroutine(destroyText());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator destroyText()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);

    }
}
