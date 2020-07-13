using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chefcitoBreseham : MonoBehaviour
{
    // Start is called before the first frame update
    public Greivin greivinScript;
    public Rigidbody2D rb;
    public Transform player;
    private Vector2 moveTo;
    private bool ready = false;
    private bool ready2 = true;
    private static float moveCoorX;
    private static float moveCoorY;
    private static float newCoorx=0; 
    private static float newCoory=0;
  
    Vector2 wayofchefcito()
    {
        if (ready2)
        {
           
           float greivincoordinatex = GameObject.Find("Greivin").transform.position.x;
           float greivincoordinatey = GameObject.Find("Greivin").transform.position.y;
           float coordinatex= GameObject.Find("Chefcito").transform.position.x;
           float coordinatey= GameObject.Find("Chefcito").transform.position.y;
           rutabresenham(coordinatex,coordinatey,greivincoordinatex,greivincoordinatey);
           //Debug.Log(moveCoorX);
           //Debug.Log(moveCoorY);
           // Debug.Log(coordinatex);
           //Debug.Log(coordinatey);
           newCoorx = (transform.position.x + moveCoorX);
           newCoory = (transform.position.y + moveCoorY);
           moveCoorX = 0;
           moveCoorY = 0;
           ready2 = false;
            
        }

        if (transform.position.x == newCoorx & transform.position.y == newCoory)
        {
            ready2 = true;
            ready = false;
        }
        moveTo = new Vector2(newCoorx,newCoory);
        return moveTo;
    }
    void moveChefcito()
    {
        float movementSpeed = 5.0f;
        float movementStep = movementSpeed *Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, wayofchefcito(), movementStep);
    }
    // Update is called once per frame
    void Update()
    {
      // checkDistancetoPlayer();
     moveChefcito();
    }

    void rutabresenham(float coorx, float coory, float coorx2, float coory2)
    {
        float width = coorx2 - coorx;
        float height = coory2 - coory;
        float difx=0f, dify=0f, difx2=0f, dify2=0f;
        if (width < 0)
        {
            difx = -1;
        }else if(width>0)

        {
            difx = 1;
        }

        if (height < 0)
        {
            dify = -1;
        }else if (height>0)
        {
            dify = 1;
        }

        if (width<0)
        {
            difx2=-1;
        }else if (width > 0)
        {
            difx2 = 1;
        }

        float longest = Math.Abs(width);
        float shortest = Math.Abs(height);
        if (!(longest>shortest))
        {
            longest = Math.Abs(height);
            shortest = Math.Abs(width);
            if (height < 0)
            {
                dify2 = -1;
            }else if (height >0)
            {
                dify2 = 1;
                difx2 = 0;
            }
        }

        float numerator = (int)Math.Round(longest) >> 1;
        if (!ready)
        {
            for (int i = 0; i <= longest; i++)
            {
                numerator += shortest;
                if (!(numerator <longest))
                {
                    numerator -= longest;
                    moveCoorX += difx;
                    moveCoorY += dify;
                }
                else
                {
                    moveCoorX += difx2;
                    moveCoorY += dify2;
                }
            }

            ready = true;
        }
        Debug.Log(moveCoorX);
        Debug.Log(moveCoorY);
    }
}
