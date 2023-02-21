using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    public Transform[] waypoints;
    private int waypointIndex;
    private Vector3 target;

    [SerializeField]
    private float speedFactor = 0.5f;
    private int VelocityHash;

    [SerializeField]
    private bool attackCooldown = false;

    [SerializeField]
    private bool chasePlayer = false;
    [SerializeField]
    private Transform playerPos;


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
        //if 
        if (chasePlayer)
        {
            agent.speed = speedFactor + 3;
            agent.SetDestination(playerPos.position);
            animator.SetFloat(VelocityHash, agent.velocity.magnitude);
            AttackPlayer();
        }
        else
        {
            agent.speed = speedFactor;
            Patrolling();
        }

        

    }


    private void AttackPlayer()
    {
        if (!attackCooldown)
        {
            if (Vector3.Distance(transform.position, playerPos.position) < 2f)
            {
                StartCoroutine(Attack());
                attackCooldown = true;
            }
        }
       // else
       // {
       //    StartCoroutine(Attack());       
       // }     
    }

  // private void Attacks()
  // {
  //
  // }

    private IEnumerator Attack()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(3);
        attackCooldown= false;
    }


    private void Patrolling()
    {
        updatePatrolDestination();

        // check if ai is at the waypoint and iterate to the next one if current waypoint was met
        if (Vector3.Distance(transform.position, target) < 2f)
        {
            IterateThroughWaypoints();
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
