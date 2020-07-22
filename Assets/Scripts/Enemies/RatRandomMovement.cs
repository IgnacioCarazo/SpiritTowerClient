using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatRandomMovement : MonoBehaviour
{

    public float movementSpeed;

    private Rigidbody2D rb;

    public bool isWalking;

    public float walkTime;
    private float walkCounter;
    public float waitTime;
    public float waitCounter;


    private int walkDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        waitCounter = waitTime;

        walkCounter = walkTime;

        chooseDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking) {

            walkCounter -= Time.deltaTime;

        

            switch (walkDirection)
            {
                
                case 0:
                rb.velocity = new Vector2(0, movementSpeed);
                break;

                case 1:
                rb.velocity = new Vector2(movementSpeed, 0);
                break;

                case 2:
                rb.velocity = new Vector2(0, -movementSpeed);
                break;

                case 3:
                rb.velocity = new Vector2(-movementSpeed, 0);
                break;

            }

            if (walkCounter < 0) {
                isWalking = false;
                waitCounter = waitTime;
            }

        } else {

            waitCounter -= Time.deltaTime;
            rb.velocity = Vector2.zero;
            if (waitCounter < 0) {
                chooseDirection();
            }
        }
    }


    public void chooseDirection() {
        walkDirection = Random.Range(0,4);
        isWalking = true;
        walkCounter = walkTime;

    }
}
