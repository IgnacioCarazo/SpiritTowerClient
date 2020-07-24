using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;

public class EspectroMovement : MonoBehaviour
{
    public Greivin greivinScript;

    public Unit AStar;
    
    public bool dead;
    public DemonEye eyeScript;
    public List<Transform> waypoints = new List<Transform>();
    private Transform targetWaypoint;
    private int targetWaypointIndex = 0;
    private float minDistance = 0.1f;
    private int lastWaypointIndex;
    private float movementSpeed = 5f;
    private float runningSpeed = 10f;
    private int greivinVisto = 0;
    public bool teleport = false;
    public Transform DemonEye;

    private Animator animator;
    TcpClient EspectroRojoClient;
    Vector2 pos;
    SocketConnection socketConnection;

    #region breadcrumb variables

    private enum EnemyState
    {
        PATROLLING,
        FOLLOWING_PLAYER,
        FOLLOWING_BREADCRUMBS
    };

    EnemyState state = EnemyState.PATROLLING;
    public Transform player;
    float minCrumbDistance = 3.0f;
    private Transform lastKnownWaypoint;
    private float inRange = 5.0f;
    private float escapeDistance = 6.0f;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        dead = false;
        animator = GetComponent<Animator>();
        socketConnection = GameObject.FindObjectOfType(typeof(SocketConnection)) as SocketConnection;
        lastWaypointIndex = waypoints.Count - 1;
        targetWaypoint = waypoints[targetWaypointIndex];
        InvokeRepeating("spectrePosition",  0.5f,  2f);

        
        AStar.enabled = false;

    }

    void Update()
    {
        if (!eyeScript.greivinVisto)
        {
            ControllEnemyState();
            UpdateTransform();
        }
        else if (eyeScript.greivinVisto)
        {
            if (this.gameObject.name == "BlueSpectre" && !teleport)
            {
                this.gameObject.transform.position = DemonEye.transform.position;
                teleport = true;
            }
            AStar.enabled = true;

        }
    }

    void spectrePosition(){
        if (dead == true){
            socketConnection.SendData(name + transform.position);
        }
        
    }

    void ControllEnemyState()
    {
        CheckDistanceToPlayer();
        switch (state)
        {
            case EnemyState.PATROLLING:
                float distance = Vector2.Distance(transform.position, targetWaypoint.position);
                CheckDistanceToWayPoint(distance);
                // uses the correct animation comparing targetWaypoint position vs "Espectro" position
                animator.SetFloat("Horizontal", targetWaypoint.position.x - transform.position.x);
                animator.SetFloat("Vertical", targetWaypoint.position.y - transform.position.y);
                break;
            case EnemyState.FOLLOWING_PLAYER:
            AStar.enabled = true;
            
                ReturnToStartingPoint();
                if (greivinVisto > 0)
                {
                    if (greivinScript.crumbs.Count >= 1)
                    {
                        if (greivinScript.ShouldPlaceCrumb())
                        {
                            greivinScript.DropBreadcrumb();
                        }
                    }
                    else
                    {
                        greivinScript.DropBreadcrumb();
                    }
                }

                // uses the correct animation comparing greivin position vs "Espectro" position
                animator.SetFloat("Horizontal", player.position.x - transform.position.x);
                animator.SetFloat("Vertical", player.position.y - transform.position.y);
                break;
            case EnemyState.FOLLOWING_BREADCRUMBS:
                break;
        }
    }

    void UpdateTransform()
    {
        if (greivinVisto > 0)
        {
            float movementStep = runningSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, movementStep);
        }
        else
        {
            float movementStep = movementSpeed * Time.deltaTime;
            if (dead == false){
                transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, movementStep);
            }
            
        }
    }

    void CheckDistanceToPlayer()
    {
        switch (state)
        {
            case EnemyState.PATROLLING:
                if (Vector2.Distance(transform.position, player.position) < 4.0f)
                {
                    lastKnownWaypoint = targetWaypoint;
                    targetWaypoint = greivinScript.crumb.transform;
                    greivinVisto += 1;
                    state = EnemyState.FOLLOWING_PLAYER;
                }

                break;
            case EnemyState.FOLLOWING_PLAYER:
                if (Vector2.Distance(transform.position, player.position) > escapeDistance
                )
                {
                    if (greivinVisto > 0)
                    {
                        greivinScript.putcrumb(true);
                    }
                }


                break;
            case EnemyState.FOLLOWING_BREADCRUMBS:
                if (greivinVisto > 0)
                {
                    int w = 0;
                    Transform lastCrumb = greivinScript.crumbs[w];
                    if (greivinVisto>0)
                    {
                        greivinScript.putcrumb(true);

                    }
                    targetWaypoint = lastCrumb;
                    w++;
                    state = EnemyState.FOLLOWING_PLAYER;
                }

                break;
        }
    }

    void CheckDistanceToWayPoint(float currentDistance)
    {
        if (currentDistance <= minDistance)
        {
            targetWaypointIndex += 1;
            UpdateTargetWayPoint();
        }
    }

    void UpdateTargetWayPoint()
    {
        if (targetWaypointIndex > lastWaypointIndex)
        {
            targetWaypointIndex = 0;
        }

        targetWaypoint = waypoints[targetWaypointIndex];
    }

    void ReturnToStartingPoint()
    {
        if (greivinScript.crumbs.Count >= 1) //There are still crumbs left to follow...
        {
            int i = 0;
            Transform lastCrumb = greivinScript.crumbs[i];
            targetWaypoint = lastCrumb;
            i++;
            if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.2f)
            {
                int currentCrumb = 0;
                greivinScript.crumbs.Remove(greivinScript.crumbs[currentCrumb]);
                // Destroy(greivinScript.crumbs[currentCrumb].gameObject);
                currentCrumb++;
            }
            /* if (Vector2.Distance(transform.position, lastKnownWaypoint.position) <
                 Vector2.Distance(transform.position, targetWaypoint.position))
             {
                 targetWaypoint = lastKnownWaypoint;
                 state = EnemyState.PATROLLING;
                 foreach (Transform breadcrumb in greivinScript.crumbs)
                 {
                     Destroy(breadcrumb.gameObject);
                 }
 
                 greivinScript.crumbs.Clear();
             }*/

            if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.3f) //reached the breadcrumb
            {
                greivinScript.crumbs.Remove(lastCrumb.transform);
                Destroy(lastCrumb.gameObject);
                ReturnToStartingPoint();
            }
        }
        else
        {
            //no crumbs left
            //state = EnemyState.PATROLLING;
            if (greivinVisto > 0)
            {
                greivinScript.DropBreadcrumb();
                targetWaypoint = greivinScript.crumbs[greivinScript.crumbs.Count - 1];
            }
        }
    }


    public void onDeath()
    {   
        dead = true;
        animator.SetBool("isDead", true);
        StartCoroutine(destroyEn());
        foreach (Transform breadcrumb in greivinScript.crumbs)
        {
            Destroy(breadcrumb.gameObject);
        }
        greivinScript.crumbs.Clear();
        greivinVisto = 0;
    }

    IEnumerator destroyEn()
    {
        yield return new WaitForSeconds(.3f);
        this.gameObject.SetActive(false);
        foreach (Transform breadcrumb in greivinScript.crumbs)
        {
            Destroy(breadcrumb.gameObject);
        }
        greivinScript.crumbs.Clear();
        greivinVisto = 0;
    }
}