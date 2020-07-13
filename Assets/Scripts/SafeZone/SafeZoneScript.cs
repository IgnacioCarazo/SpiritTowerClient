using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZoneScript : MonoBehaviour
{
    Greivin greivinScript;
    // Start is called before the first frame update
    void Start()
    {
        greivinScript = GameObject.FindObjectOfType(typeof(Greivin)) as Greivin;
    }
    void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Greivin")) {
            greivinScript.isSafe = true;
            
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Greivin")) {
            greivinScript.isSafe = false;
            
        }
    }
}
