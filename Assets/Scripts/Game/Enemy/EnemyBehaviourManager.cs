using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyBehaviourManager : MonoBehaviour
{
    /// <summary>
    /// movement variables
    /// </summary>
    private Animator animator;
    private NavMeshAgent agent;
    public Transform[] waypoints;
    public Vector3 target;
    private float speedFactor = 0.5f;
    private EnemyMovement movement;

    /// <summary>
    /// attack variables
    /// </summary>
    [SerializeField]
    private float hitboxRadius = 1f;
    [SerializeField]
    public LayerMask playerMask;
    private EnemyCombat combat;


    public bool swarmMode = false;
    private bool screamCooldown = false;

    public bool chasePlayer = false;
    public Transform player;

    public int enemyType = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent  = GetComponent<NavMeshAgent>();

        combat = GetComponent<EnemyCombat>().Initialiser(animator, playerMask, 1f);
        movement = GetComponent<EnemyMovement>().Initialiser(agent, animator, waypoints);
        movement.updatePatrolDestination();

        //modify the speed of all instances of this agent
        agent.speed = speedFactor;
    }

    // Update is called once per frame
    void Update()
    {
        if(swarmMode)
        {
            target = player.position;
        }

        if (chasePlayer)
        {
            agent.speed = speedFactor + 3;

            ////only update the target pos if it is out of the attack range
            //if (Vector3.Distance(transform.position, target) > 1f)
            //{
            //    agent.SetDestination(target);
            //}
            //animator.SetFloat(VelocityHash, agent.velocity.magnitude);
            movement.ChaseTarget(target);
            combat.AttackPlayer(target, player.position);
      
        }
        else
        {
            agent.speed = speedFactor;
            movement.Patrolling();
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

   // private void Patrolling()
   // {
   //     updatePatrolDestination();
   //
   //     // check if ai is at the waypoint and iterate to the next one if current waypoint was met
   //     if (Vector3.Distance(transform.position, target) < 2f)
   //     {
   //         IterateThroughWaypoints();
   //     }
   //
   //     animator.SetFloat(VelocityHash, agent.velocity.magnitude);
   //
   // }
   //
   //
   // private void updatePatrolDestination()
   // {
   //     target = waypoints[waypointIndex].position;
   //     agent.SetDestination(target);
   // }
   //
   //
   // private void IterateThroughWaypoints()
   // {
   //     waypointIndex++;
   //
   //     // reset waypoint index
   //     if(waypointIndex >= waypoints.Length)
   //     {
   //         waypointIndex = 0;
   //     }
   // }
}
