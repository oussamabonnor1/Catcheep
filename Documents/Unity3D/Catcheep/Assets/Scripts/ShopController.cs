using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class ShopController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }

    public void ShipOne()
    {
        PlayerPrefs.SetInt("ship", 1);
    }
    public void ShipTwo()
    {
        PlayerPrefs.SetInt("ship", 2);
    }
}
