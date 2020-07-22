using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreivinHitDetector : MonoBehaviour
{   


    Greivin greivinScript;
    // Start is called before the first frame update
    void Start()
    {
        greivinScript = GameObject.FindObjectOfType(typeof(Greivin)) as Greivin;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("canAttack")) {
            collision.GetComponent<EspectroMovement>().onDeath();
            
        } if (collision.CompareTag("canAttack2")) {
            collision.GetComponent<EnemyTest>().onDeath();
        } if (collision.CompareTag("Jarron")) {
            if (greivinScript.healthBar.numOfHearts < 5) {
                greivinScript.healthBar.numOfHearts += 1;
            }
            collision.GetComponent<JarronesScript>().onHit();
        } if (collision.CompareTag("EnemigoSimple")) {
            collision.GetComponent<SimpleEnemyDeath>().onDeath();
        }
    }

    
}
