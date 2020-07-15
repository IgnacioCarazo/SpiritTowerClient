using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    SocketConnection socketConnection;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Greivin")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            socketConnection = GameObject.FindObjectOfType(typeof(SocketConnection)) as SocketConnection;
            socketConnection.destroyThread();
        }
    }
}
