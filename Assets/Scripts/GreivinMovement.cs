using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreivinMovement : MonoBehaviour
{

    public float movementSpeed = 5f;

    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement != Vector2.zero){
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }

        animator.SetFloat("Speed", movement.sqrMagnitude);

    }

    void FixedUpdate() {

        rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);

    }
}
