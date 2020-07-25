using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public float speed;

    public bool attack;

    private Transform target;

    private Transform defaultPos;

    private float timeBtwShots;
    public float startTimeBtwShots;

    public GameObject fireball;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Greivin").GetComponent<Transform>();

        defaultPos = transform;

        timeBtwShots = startTimeBtwShots;
    }

    // Update is called once per frame
    void Update()
    {
        if (target.position.y > -10){

            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);  

            if (timeBtwShots <= 0) {

            Instantiate(fireball, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;

            } else {
                timeBtwShots -= Time.deltaTime;
            }   
        } else {
             transform.position = Vector2.MoveTowards(transform.position, defaultPos.position, speed * Time.deltaTime);
        }

        
    }
}
