using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    Greivin greivinScript;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Greivin")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            greivinScript = GameObject.FindObjectOfType(typeof(Greivin)) as Greivin;
            greivinScript.destroyThread();
        }
    }
}
