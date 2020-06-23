using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreivinAttack : MonoBehaviour
{
   public Animator animator;



    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Attack", false);
        playerAttack();
        
    }

     void playerAttack() {
        if (Input.GetMouseButtonDown(1)){
            Debug.Log("click");
            animator.SetBool("Attack", true);

        } if (Input.GetMouseButtonUp(1)) {
            animator.SetBool("Attack", false);
        }
    }
}

