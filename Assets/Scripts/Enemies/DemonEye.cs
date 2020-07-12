using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonEye : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();

    private Transform targetWaypoint;

    private int targetWaypointIndex = 0;

    private float minDistance = 0.1f;

    private int lastWaypointIndex;

    private float movementSpeed = 5f;
    private Animator animator;

    #region chasing variables

    private enum EnemyState
    {
        PATROLLING,
        FOLLOWING_PLAYER
    };

    private EnemyState state = EnemyState.PATROLLING;
    public Transform player;
    private float inRange = 2.0f;
    private float escapeDistance = 6.0f;
    private Transform lastKnownWaypoint;
    public bool greivinVisto = false;

    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        lastWaypointIndex = waypoints.Count - 1;
        targetWaypoint = waypoints[targetWaypointIndex];
    }

    // Update is called once per frame
    void Update()
    {
        ControllEnemyState();
        UpdateTransform();

    }

    void UpdateTransform()
    {
        float movementStep = movementSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, movementStep);
    }

    void CheckDistanceToWayPoint(float currentDistance)
    {
        if (currentDistance <= minDistance)
        {
            targetWaypointIndex += 1;
            UpdateTargetWayPoint();
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
                break;
            case EnemyState.FOLLOWING_PLAYER :
                break;
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

    void CheckDistanceToPlayer()
    {
        switch (state)
        {
            case EnemyState.PATROLLING:
                if (Vector2.Distance(transform.position, player.position)< 4.0f)
                {
                    lastKnownWaypoint = targetWaypoint;
                    targetWaypoint = player.transform;
                    greivinVisto = true;
                    state = EnemyState.FOLLOWING_PLAYER;
                }

                break;
            case EnemyState.FOLLOWING_PLAYER:
                if (greivinVisto)
                {
                    lastKnownWaypoint = targetWaypoint;
                    targetWaypoint = player.transform;
                }
                break;
        }
    }
}
