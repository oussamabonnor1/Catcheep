using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSpinner : MonoBehaviour
{

    public GameObject wheelOfLuck;

	// Use this for initialization
	void Start ()
	{
	    StartCoroutine(wheelTurned());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator wheelTurned()
    {
        print("whw");
        int i = 0;
        do
        {
            Quaternion a = wheelOfLuck.GetComponent<RectTransform>().rotation;
            wheelOfLuck.GetComponent<RectTransform>().transform.Rotate(wheelOfLuck.transform.localPosition,i);//RotateAround(wheelOfLuck.transform.position,Vector3.back,10);
            i++;
            yield return new WaitForSeconds(0.01f);
        } while (i < 10);
    }
}
