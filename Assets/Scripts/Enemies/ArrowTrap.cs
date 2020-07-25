using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    SocketConnection socketConnection;
    HealthBar healthBar;
    public GameObject arrowPrefab;

   void Start()
    {
        socketConnection = GameObject.FindObjectOfType(typeof(SocketConnection)) as SocketConnection;
        healthBar = GameObject.FindObjectOfType(typeof(HealthBar)) as HealthBar;
        InvokeRepeating("spawnArrow",  0.1f,  3f);
    }

    public void Update(){

    }

    public void spawnArrow(){
        GameObject arrow = Instantiate(arrowPrefab, new Vector3(19.7f, -36.37f, 0), Quaternion.identity);
        arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -5.0f);
        Destroy(arrow, 7);
    }

    void Oncollisionenter2D(){ 
        Debug.Log("COLLISION");
    }
}
