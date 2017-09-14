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
	    StartCoroutine(textBlur());
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
        SceneManager.LoadScene(1);
    }

    IEnumerator textBlur()
    {
        GetComponentInChildren<TextMeshProUGUI>().fontSharedMaterial.SetFloat(ShaderUtilities.ID_OutlineSoftness, 1f);
        for (int j = 0; j < 8; j++)
        {
            yield return new WaitForSeconds(0.2f);
            GetComponentInChildren<TextMeshProUGUI>().fontSharedMaterial.SetFloat(ShaderUtilities.ID_OutlineSoftness, 
            GetComponentInChildren<TextMeshProUGUI>().fontSharedMaterial.GetFloat(ShaderUtilities.ID_OutlineSoftness) - 0.1f);
        }
    }
}
