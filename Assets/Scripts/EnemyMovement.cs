using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private Animator animator;
    NavMeshAgent agent;
    public Transform[] waypoints;
    int waypointIndex;
    public Vector3 target;
    private int VelocityHash;

    // Start is called before the first frame update
    void Start()
    {
        VelocityHash = Animator.StringToHash("Velocity");
        animator = GetComponent<Animator>();
        agent  = GetComponent<NavMeshAgent>();
        updatePatrolDestination();
    }

    // Update is called once per frame
    void Update()
    {
        // check if ai is at the waypoint
        if(Vector3.Distance(transform.position, target) < 2f)
        {
            IterateThroughWaypoints();
            updatePatrolDestination();
        }

        animator.SetFloat(VelocityHash, agent.velocity.magnitude);

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
