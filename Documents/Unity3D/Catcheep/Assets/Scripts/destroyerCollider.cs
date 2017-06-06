using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyerCollider : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "group")
        {
            Destroy(other.gameObject);
        }
        else
        {
            other.gameObject.GetComponent<sheepDestroyer>().Destruction();
        }
    }
}
