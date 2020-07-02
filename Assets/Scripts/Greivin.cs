using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;

public class Greivin : MonoBehaviour
{   

    public float movementSpeed = 5f;
    private Thread clientReceiveThread; 
    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;

    TcpClient greivinClient;
    // Start is called before the first frame update
    void Start()
    {
        greivinClient = new TcpClient();
        greivinClient.ConnectAsync("127.0.0.1", 5050);
        clientReceiveThread = new Thread (new ThreadStart(ListenForData)); 			
		clientReceiveThread.IsBackground = true; 			
		clientReceiveThread.Start();  
    }

    // Update is called once per frame
    void Update()
    {
        
        //movement.x = Input.GetAxisRaw("Horizontal");
        //movement.y = Input.GetAxisRaw("Vertical");

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
        if (Input.GetAxisRaw("Horizontal") == -1) {
            SendData("GreivinLeft");
        } if (Input.GetAxisRaw("Horizontal") == 1) {
            SendData("GreivinRight");
        }
        if (Input.GetAxisRaw("Vertical") == -1) {
            SendData("GreivinDown");
        } if (Input.GetAxisRaw("Vertical") == 1) {
            SendData("GreivinUp");
        }
    }


    void playerShield() {
        if (Input.GetMouseButton(0)){
            animator.SetBool("Shield", true);
            movement.y = 0;
            movement.x = 0;
        } else {
            animator.SetBool("Shield", false);
        }
    }

    void playerAttack() {
        if (Input.GetMouseButtonDown(1)){
            animator.SetBool("Attack", true);
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
						Debug.Log("EL SERVER RECIBIO: " + serverMessage); 					
					} 				
				} 			
			}         
		}         
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		}     
	}  
}
