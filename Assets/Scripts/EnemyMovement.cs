using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform[] waypoints;
    int waypointIndex;
    Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        agent  = GetComponent<NavMeshAgent>();
        updatePatrolDestination();
    }

    // Update is called once per frame
    void Update()
    {
        // check if ai is at the waypoint
        if(Vector3.Distance(transform.position, target) < 1)
        {
            IterateThroughWaypoints();
            updatePatrolDestination();
        }   

    }

    private void updatePatrolDestination()
    {
        target = waypoints[waypointIndex].position;
        agent.SetDestination(target);
    }

    private void IterateThroughWaypoints()
    {
        waypointIndex++;

        // reset waypoint index
        if(waypointIndex >= waypoints.Length)
        {
            waypointIndex = 0;
        }
    }
}
