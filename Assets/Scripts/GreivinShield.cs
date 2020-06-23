using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreivinShield : MonoBehaviour
{
     public Animator animator;
    

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Shield", false);

        playerShield();
    }


    void playerShield() {
        if (Input.GetMouseButton(0)){
            animator.SetBool("Shield", true);
            

        } else {
            animator.SetBool("Shield", false);
            

        }
    }
}
