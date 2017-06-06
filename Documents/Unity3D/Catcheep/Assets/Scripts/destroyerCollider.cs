using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyerCollider : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    { 
        other.gameObject.GetComponent<sheepDestroyer>().Destruction();
    }
}
