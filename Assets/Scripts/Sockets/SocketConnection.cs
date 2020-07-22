using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;
using UnityEngine.SceneManagement;


public class SocketConnection : MonoBehaviour
{
    Greivin greivinScript;
    private Thread clientReceiveThread; 
    TcpClient tcpClient;

    // Start is called before the first frame update
    void Start()
    {
        greivinScript = GameObject.FindObjectOfType(typeof(Greivin)) as Greivin;

        tcpClient = new TcpClient();
        tcpClient.ConnectAsync("127.0.0.1", 5050);
        clientReceiveThread = new Thread (new ThreadStart(ListenForData)); 			
		clientReceiveThread.IsBackground = true; 			
		clientReceiveThread.Start(); 
    }

    /// <summary> 	
	/// Send message to server using socket connection. 	
	/// </summary> 	
	public void SendData(string message) {         
		if (tcpClient == null) {             
			return;         
		}  		
		try { 			
			// Get a stream object for writing. 			
			NetworkStream stream = tcpClient.GetStream(); 			
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
    public void ListenForData() { 
        string serverMessage;		
		try { 			
			 			
			Byte[] bytes = new Byte[1024];             
			while (true) { 				
				// Get a stream object for reading 				
				using (NetworkStream receivingstream = tcpClient.GetStream()) { 					
					int length; 					
					// Read incomming stream into byte arrary. 					
					while ((length = receivingstream.Read(bytes, 0, bytes.Length)) != 0) { 						
						var incommingData = new byte[length]; 						
						Array.Copy(bytes, 0, incommingData, 0, length); 						
						// Convert byte array to string message. 						
						serverMessage = Encoding.ASCII.GetString(incommingData); 
                         // The player movement responds according to the message received from the server
                        if (serverMessage == "GreivinRight"){
                            greivinScript.movement.x = 1;
                            greivinScript.movement.y = 0;                            
                        }  if (serverMessage == "GreivinLeft"){
                            greivinScript.movement.x = -1;
                            greivinScript.movement.y = 0;
                        }  if (serverMessage == "GreivinUp"){
                            greivinScript.movement.x = 0;
                            greivinScript.movement.y = 1;
                        }  if (serverMessage == "GreivinDown"){
                            greivinScript.movement.x = 0;
                            greivinScript.movement.y = -1;
                        } 
                        Debug.Log(serverMessage);
					
					} 				
				} 			
			}         
		}         
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		} 
            
	}  
    
    public void destroyThread() {
        clientReceiveThread.Abort();
    }

    void OnApplicationQuit()
    {
		
        clientReceiveThread.Abort();
        tcpClient.Close();
    }
}
