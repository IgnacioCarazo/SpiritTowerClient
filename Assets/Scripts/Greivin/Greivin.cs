using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;
using UnityEngine.SceneManagement;

public class Greivin : MonoBehaviour
{
    
    public float movementSpeed = 5f;
 
    public Rigidbody2D rb;
    public Animator animator;
    public HealthBar healthBar;
    public List<Transform> crumbs = new List<Transform>();
    public Transform Enemy;
    public GameObject crumb;
    float minCrumbDistance = 3.0f;
    public Vector2 movement;
    private int score=0;
    private Interactable interactable;
    public bool isSafe;
    public bool isDead;
    SocketConnection socketConnection;

    // Start is called before the first frame update
    void Start()
    {
        socketConnection = GameObject.FindObjectOfType(typeof(SocketConnection)) as SocketConnection;
        healthBar = GameObject.FindObjectOfType(typeof(HealthBar)) as HealthBar;
        InvokeRepeating("greivinPosition",  0.2f,  5f);
        InvokeRepeating("greivinLives",  0.4f,  5f);
    }

    // Update is called once per frame
    void Update()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (healthBar.numOfHearts<=0)
        {
            if (sceneName =="ForestLvl1")
            {
                SceneManager.LoadScene("ForestLvl1");    
            }else if (sceneName =="DungeonLvl2")
            {
                SceneManager.LoadScene("DungeonLvl2");
            }else if (sceneName =="DungeonLvl3")
            {
                 SceneManager.LoadScene("DungeonLvl3");
            }else if (sceneName =="DungeonLvl4")
            {
                SceneManager.LoadScene("DungeonLvl4");
            }else if (sceneName == "DungeonBossLevel")
            {
                SceneManager.LoadScene("DungeonBossLevel");
            }
            
        }
        
       movement.x = Input.GetAxisRaw("Horizontal");
       movement.y = Input.GetAxisRaw("Vertical");

        if (movement != Vector2.zero){
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }

        animator.SetFloat("Speed", movement.sqrMagnitude);
        playerAttack();
        playerShield();
    }

    
    void FixedUpdate() {
        rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);


        // Sends the input to the server
        if (Input.GetAxisRaw("Horizontal") == -1) {
            //socketConnection.SendData("GreivinLeft");
        } if (Input.GetAxisRaw("Horizontal") == 1) {
            //socketConnection.SendData("GreivinRight");
        } if (Input.GetAxisRaw("Vertical") == -1) {
            //socketConnection.SendData("GreivinDown");
        } if (Input.GetAxisRaw("Vertical") == 1) {
            //socketConnection.SendData("GreivinUp");
        }
    }

    public void greivinPosition() {
        socketConnection.SendData("Greivin Position: ");
    
    }
    public void greivinLives() {
        socketConnection.SendData("Greivin Lives: " + healthBar.numOfHearts);        
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name== "RedSpectre")
        {
            healthBar.numOfHearts -= 5;
        }
        if (other.gameObject.name== "RedSpectre1")
        {
            healthBar.numOfHearts -= 5;
        }
        if (other.gameObject.name== "RedSpectre2")
        {
            healthBar.numOfHearts -= 5;
        }
        if (other.gameObject.name== "RedSpectre3")
        {
            healthBar.numOfHearts -= 5;
        }
        if (other.gameObject.name== "RedSpectre4")
        {
            healthBar.numOfHearts -= 5;
        }
        if (other.gameObject.name== "GreySpectre")
        {
            healthBar.numOfHearts -= 5;
        }
        if (other.gameObject.name== "BlueSpectre")
        {
            healthBar.numOfHearts -= 5;
        }

        if (other.gameObject.tag== "coin")
        {
            score += 10;
            Debug.Log("Score : "+score);
            other.gameObject.SetActive(false);
        }
    }



     /// <summary> 	
	/// Checks if the player uses the shield 	
	/// </summary> 	
    void playerShield() {
        if (Input.GetMouseButton(0)){
            //socketConnection.SendData("GreivinShield");
            animator.SetBool("Shield", true);
            movement.y = 0; 
            movement.x = 0; 
        } else {
            animator.SetBool("Shield", false);
        }
    }


    /// <summary> 	
	/// Checks if the player uses the attack 	
	/// </summary> 	
    void playerAttack() {
        if (Input.GetMouseButtonDown(1)){
            FindObjectOfType<AudioManager>().Play("Sword");
           // socketConnection.SendData("GreivinAttack");
            animator.SetBool("Attack", true);
            movement.y = 0; 
            movement.x = 0; 
        } if (Input.GetMouseButtonUp(1)) {
            animator.SetBool("Attack", false);
        }
    }

    

    
   
    

    //Variables para breadcrumbing
    /*public enum PlayerState
    {
        PLAYING,
        ENEMY_FOLLOWING,
    };

   // PlayerState state = PlayerState.ENEMY_FOLLOWING;
   

    /*
    void ControllPlayerState()
    {
        switch (state)
        {
            case PlayerState.ENEMY_FOLLOWING:
                if (crumbs.Count >= 1)
                {
                    if (ShouldPlaceCrumb())
                    {
                        DropBreadcrumb();
                    }
                }
                else
                {
                    DropBreadcrumb();
                }

                break;
            case PlayerState.PLAYING:
                break;
        }
    }
    */
    public void putcrumb(bool booleano)
    {
        if (booleano)
        {
            if (crumbs.Count >= 1)
            {
                if (ShouldPlaceCrumb())
                {
                    DropBreadcrumb();
                }
            }
            else
            {
                DropBreadcrumb();
            }

        }
    }


    public void DropBreadcrumb()
    {
        GameObject droppedCrumb = Instantiate(crumb, transform.position, Quaternion.identity, null);
        crumbs.Add(droppedCrumb.transform);
    }

    public bool ShouldPlaceCrumb()
    {
        if (Vector2.Distance(transform.position, crumbs[crumbs.Count - 1].transform.position) > minCrumbDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    

    
}
