using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    // The spawner variables
    private ZombieSpawner spawner;
    private Transform[] waypoints;
    public Vector3 target;
    public int zombieType;
    private Transform player;
    private LayerMask targetMask;
    private float hitboxRadius;

    private Animator animator;
    private float speedFactor;
    private NavMeshAgent agent;

    private EnemyCombat combat;
    private EnemyMovement movement;
    //private FieldOfView fov;

    //behaviour states
    private bool isAlive = true;
    public bool isVictorious;
    public bool swarmMode = false;
    public bool chaseTarget = false;
    private bool screamCooldown = false;

    /// <summary>
    /// Constructor function for Zombies
    /// </summary>
    /// <param name="_waypoints"></param>
    /// <param name="_player"></param>
    /// <param name="_zombieType"></param>
    public Zombie Init(ZombieSpawner spawner,Transform[] _waypoints, Transform _player, LayerMask _targetMask ,int _zombieType, float _hitboxRadius, float _agentSpeed, float fov_angle, float fov_radius) {
     
        this.spawner = spawner;
        this.waypoints = _waypoints;
        this.player = _player;
        this.zombieType = _zombieType;
        this.targetMask= _targetMask;
        this.hitboxRadius = _hitboxRadius;
        this.speedFactor = _agentSpeed;

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        combat = gameObject.AddComponent<EnemyCombat>().Init(animator, targetMask, hitboxRadius, this);
        movement = gameObject.AddComponent<EnemyMovement>().Init(agent, animator, waypoints);

        //intialise fov
        GetComponentInChildren<FieldOfView>().Init(this, fov_angle, fov_radius, targetMask);
     

        return this;
    }

    // Update is called once per frame
    void Update()
    {
        //only this this if the zombie is alive
        if(isAlive)
        {
            if (swarmMode)
            {
                target = player.position;
            }

            if (chaseTarget)
            {
                agent.speed = speedFactor + 3;
                movement.ChaseTarget(target);
                combat.AttackPlayer(target, player.position);
            }
            else
            {
                agent.speed = speedFactor;
                movement.Patrolling();
            }
        }
    }

    public void Victory()
    {
        chaseTarget = false;
        swarmMode = false;

        //stop the agent from moving 
        agent.isStopped = true;

        // if victory state is enabled then send state to global spawner
        if (isVictorious)
        {
            isVictorious = false;
            // when player is dead propagate the new state to the spawner so all enemies can stop moving
            spawner.Victory();
        }
        else
        {
            GetComponentInChildren<FieldOfView>().gameObject.SetActive(false);
        }
    }

    //TODO if player dies at same time as death. 
    public void Death()
    {
        try
        {
            //disable fov
            GetComponentInChildren<FieldOfView>().gameObject.SetActive(false);
        }
        catch
        {
            Debug.Log("Fov disabled already due to victory");
        }

        chaseTarget = false;
        swarmMode = false;

        //stop the agent from moving 
        agent.isStopped = true;

        // TODO start despawn?
        isAlive = false;
        spawner.removeZombieFromList(this);
        //remove zombie from spawner list  
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
}
