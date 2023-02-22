using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviourManager : MonoBehaviour
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

    public bool swarmMode = false;
    private bool screamCooldown = false;

    public bool chasePlayer = false;
    public Vector3 targetPos;
    public Transform player;

    public int enemyType = 0;

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
        if(swarmMode)
        {
            targetPos = player.position;
        }

        if (chasePlayer)
        {
            agent.speed = speedFactor + 3;

            //only update the target pos if it is out of the attack range
            if (Vector3.Distance(transform.position, targetPos) > 1f)
            {
                agent.SetDestination(targetPos);
            }
            animator.SetFloat(VelocityHash, agent.velocity.magnitude);
            AttackPlayer();
        }
        else
        {
            agent.speed = speedFactor;
            Patrolling();
        }

    }

    public void Scream()
    {
        //if swarm mode is not on then scream to alert nearby zombies
        if (!swarmMode & !screamCooldown)
        {
            //stop the agent from moving 
            agent.isStopped = true;

            //execute animation and sound
            StartCoroutine(ScreamingAlert());

        }
       
    }

    private IEnumerator ScreamingAlert()
    {
        animator.SetTrigger("Scream");
        screamCooldown = true;
        yield return new WaitForSeconds(3);
        swarmMode = true;
        screamCooldown = false;
        agent.isStopped = false;
    }


    //TODO apply damage to player 

    private void AttackPlayer()
    {
        if (!attackCooldown)
        {
            if (Vector3.Distance(transform.position, targetPos) < 2f)
            {
                if(Vector3.Distance(player.position, targetPos) < 2f)
                {
                    StartCoroutine(Attack());
                    attackCooldown = true;
                }   
            }
        }
    }

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
