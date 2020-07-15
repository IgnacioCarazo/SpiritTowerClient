  
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void onDeath(){
        animator.SetBool("isDead", true);
        StartCoroutine(destroyEn());
    }

    IEnumerator destroyEn(){
    yield return new WaitForSeconds(.3f);
       this.gameObject.SetActive(false);
    }
}
