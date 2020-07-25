using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreivinHitDetector : MonoBehaviour
{   

    SocketConnection socketConnection;
    Greivin greivinScript;
    // Start is called before the first frame update
    void Start()
    {
        socketConnection = GameObject.FindObjectOfType(typeof(SocketConnection)) as SocketConnection;
        greivinScript = GameObject.FindObjectOfType(typeof(Greivin)) as Greivin;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("canAttack")) {
            collision.GetComponent<EspectroMovement>().onDeath();
            socketConnection.SendData("SpectreKilled");
            
        } if (collision.CompareTag("canAttack2")) {
            collision.GetComponent<EnemyTest>().onDeath();
            socketConnection.SendData("SpectreKilled");

        } if (collision.CompareTag("Jarron")) {
            if (greivinScript.healthBar.numOfHearts < 5) {
                greivinScript.healthBar.numOfHearts += 1;
            }
            collision.GetComponent<JarronesScript>().onHit();
        } if (collision.CompareTag("EnemigoSimple")) {
            collision.GetComponent<SimpleEnemyDeath>().onDeath();
            socketConnection.SendData("SimpleEnemyKilled");

        } if (collision.CompareTag("Arrow")) {
            Debug.Log("HOLAAAAAAAAAAAAAAAAAAA");
        }
    }

    
}
