using System.Collections;
using TMPro;
using UnityEngine;


public class hitTextManager : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    GetComponent<TextMeshProUGUI>().text = "H I T x " + gameManager.combo;
	    StartCoroutine(destroyText());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator destroyText()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject parent = transform.parent.gameObject;
        Destroy(parent);

    }
}
