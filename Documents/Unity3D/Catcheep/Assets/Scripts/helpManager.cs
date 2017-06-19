using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class helpManager : MonoBehaviour
{
    public GameObject[] helpTools;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void hayStack()
    {
        Instantiate(helpTools[0], transform.position, Quaternion.identity);
    }
}
