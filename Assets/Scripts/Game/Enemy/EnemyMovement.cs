using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private Transform[] waypoints;
    private int VelocityHash;
    private int waypointIndex;
    private Vector3 target;
   
    public EnemyMovement Initialiser(NavMeshAgent _agent,Animator _anim, Transform[] _waypoints)
    {
        agent = _agent;
        animator = _anim;
        waypoints= _waypoints;
        VelocityHash = Animator.StringToHash("Velocity");

        return this;
    }

    public void ChaseTarget(Vector3 targetPos)
    {
        //only update the target pos if it is out of the attack range
        if (Vector3.Distance(transform.position, targetPos) > 1f)
        {
            agent.SetDestination(targetPos);
        }
        animator.SetFloat(VelocityHash, agent.velocity.magnitude);
    }

    public void Patrolling()
    {
        updatePatrolDestination();

        // check if ai is at the waypoint and iterate to the next one if current waypoint was met
        if (Vector3.Distance(transform.position, target) < 2f)
        {
            IterateThroughWaypoints();
        }

        animator.SetFloat(VelocityHash, agent.velocity.magnitude);

    }


    public void updatePatrolDestination()
    {
        target = waypoints[waypointIndex].position;
        agent.SetDestination(target);
    }


    public void IterateThroughWaypoints()
    {
        waypointIndex++;

        // reset waypoint index
        if (waypointIndex >= waypoints.Length)
        {
            waypointIndex = 0;
        }
    }
}
