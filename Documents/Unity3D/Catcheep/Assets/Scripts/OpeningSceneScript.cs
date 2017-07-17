using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningSceneScript : MonoBehaviour
{
    private int i;

	// Use this for initialization
	void Start ()
	{
	    GetComponentInChildren<TextMeshProUGUI>().fontSharedMaterial.SetFloat("Face/Softness",1f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        i++;
        if (i > 5) StartCoroutine(opening());
    }

    IEnumerator opening()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }

    IEnumerator textBlur()
    {
        for (int j = 0; j < 9; j++)
        {
            yield return new WaitForSeconds(0.1f);
            GetComponentInChildren<TextMeshProUGUI>().GetComponent<Material>().SetFloat("Softness",1f);
        }
    }
}
