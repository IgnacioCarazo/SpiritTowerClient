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
    private Thread clientReceiveThread; 
    public Rigidbody2D rb;
    public Animator animator;
    public HealthBar healthBar;
    public List<Transform> crumbs = new List<Transform>();
    public Transform Enemy;
    public GameObject crumb;
    float minCrumbDistance = 3.0f;
    Vector2 movement;
    private int puntaje;
    private Interactable interactable;
    public bool isSafe;

    TcpClient greivinClient;
    // Start is called before the first frame update
    void Start()
    {
        healthBar = GameObject.FindObjectOfType(typeof(HealthBar)) as HealthBar;
        greivinClient = new TcpClient();
        greivinClient.ConnectAsync("127.0.0.1", 5050);
        clientReceiveThread = new Thread (new ThreadStart(ListenForData)); 			
		clientReceiveThread.IsBackground = true; 			
		clientReceiveThread.Start();  
    }

    // Update is called once per frame
    void Update()
    {
        if (healthBar.numOfHearts<=0)
        {
            SceneManager.LoadScene("ForestLvl1");
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
            SendData("GreivinLeft");
        } if (Input.GetAxisRaw("Horizontal") == 1) {
            SendData("GreivinRight");
        } if (Input.GetAxisRaw("Vertical") == -1) {
            SendData("GreivinDown");
        } if (Input.GetAxisRaw("Vertical") == 1) {
            SendData("GreivinUp");
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name== "RedSpectre")
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
    }



     /// <summary> 	
	/// Checks if the player uses the shield 	
	/// </summary> 	
    void playerShield() {
        if (Input.GetMouseButton(0)){
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
            animator.SetBool("Attack", true);
            movement.y = 0; 
            movement.x = 0; 
        } if (Input.GetMouseButtonUp(1)) {
            animator.SetBool("Attack", false);
        }
    }

    

    
    /// <summary> 	
	/// Send message to server using socket connection. 	
	/// </summary> 	
	private void SendData(string message) {         
		if (greivinClient == null) {             
			return;         
		}  		
		try { 			
			// Get a stream object for writing. 			
			NetworkStream stream = greivinClient.GetStream(); 			
			if (stream.CanWrite) {                 
								
				// Convert string message to byte array.                 
				byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(message); 				
				// Write byte array to socketConnection stream.                 
				stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);                 
				//Debug.Log("Client sent his message - should be received by server");             
			}         
		} 		
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		}     
	} 

    /// <summary> 	
	/// Listens message from the server using socket connection. 	
	/// </summary> 	
    private void ListenForData() { 		
		try { 			
			 			
			Byte[] bytes = new Byte[1024];             
			while (true) { 				
				// Get a stream object for reading 				
				using (NetworkStream receivingstream = greivinClient.GetStream()) { 					
					int length; 					
					// Read incomming stream into byte arrary. 					
					while ((length = receivingstream.Read(bytes, 0, bytes.Length)) != 0) { 						
						var incommingData = new byte[length]; 						
						Array.Copy(bytes, 0, incommingData, 0, length); 						
						// Convert byte array to string message. 						
						string serverMessage = Encoding.ASCII.GetString(incommingData); 


                        // The player movement responds according to the message received from the server
                        if (serverMessage == "GreivinRight"){
                            movement.x = 1;
                            movement.y = 0;                            
                        }  if (serverMessage == "GreivinLeft"){
                            movement.x = -1;
                            movement.y = 0;
                        }  if (serverMessage == "GreivinUp"){
                            movement.x = 0;
                            movement.y = 1;
                        }  if (serverMessage == "GreivinDown"){
                            movement.x = 0;
                            movement.y = -1;
                        } 
						//Debug.Log("EL SERVER RECIBIO: " + serverMessage); 					
					} 				
				} 			
			}         
		}         
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
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

    public void destroyThread() {
        clientReceiveThread.Abort();
    }

    void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
    }
}
