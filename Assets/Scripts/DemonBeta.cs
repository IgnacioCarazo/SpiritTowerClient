using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBeta : MonoBehaviour
{
    public float Speed;
    public Animator animator;
    public Rigidbody2D rb;
    public Transform[] moveSpots;
    private int randomSpot;
    private float waitTime;
    public float startWaitTime;

    void Start(){
        waitTime = startWaitTime ;
        randomSpot = Random.Range(0,moveSpots.Length);

    }
    // Update is called once per frame
    void Update()
        {
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, Speed *Time.deltaTime);
        animator.SetFloat("Horizontal", transform.position.normalized.x);
        animator.SetFloat("Vertical", transform.position.normalized.y);
        if(Vector2.Distance(transform.position, moveSpots[randomSpot].position)<0.2f){
            if(waitTime<=0){
                randomSpot = Random.Range(0,moveSpots.Length);
                waitTime = startWaitTime;
            }else{
                waitTime -= Time.deltaTime;
            }
        }
    }
}
