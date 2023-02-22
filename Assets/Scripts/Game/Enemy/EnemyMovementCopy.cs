using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementCopy : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    public Transform[] waypoints;
    int waypointIndex;
    private Vector3 target;
    [SerializeField]
    private float speedFactor = 0.5f;
    private int VelocityHash;


    // Start is called before the first frame update
    void Start()
    {
        VelocityHash = Animator.StringToHash("Velocity");
        animator = GetComponent<Animator>();
        agent  = GetComponent<NavMeshAgent>();
        updatePatrolDestination();

        //modify the speed of all instances of this agent
        agent.speed = speedFactor;
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

  //private void moveToDestination(Vector3 target)
  //{
  //    Vector3 destination = transform.TransformDirection(target);
  //
  //    float distance = Vector3.Distance(destination, transform.position);
  //
  //    if (distance > 0.1f)
  //    {
  //        float  
  //    }
  //
  //}

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
