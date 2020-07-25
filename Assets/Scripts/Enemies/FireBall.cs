using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{

    public float speed;

    Greivin greivinScript;

    private Transform greivin;

    private Vector2 target;

    // Start is called before the first frame update
    void Start()
    {
        greivin = GameObject.FindGameObjectWithTag("Greivin").GetComponent<Transform>();
        greivinScript = GameObject.FindObjectOfType(typeof(Greivin)) as Greivin;
        target = new Vector2(greivin.position.x, greivin.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (transform.position.x == target.x && transform.position.y == target.y) {
            destroyFireball();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Greivin")) {
            destroyFireball();
            greivinScript.healthBar.numOfHearts -= 1;
        }
    }

    void destroyFireball() {
        Destroy(gameObject);
    }
}
