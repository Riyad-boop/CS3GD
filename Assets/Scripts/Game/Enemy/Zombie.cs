using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [SerializeField]
    private LayerMask entityMask; // this entity's layermask

    [Header("Constructor varaibles")]
    private ZombieSpawner spawner;
    public int wayPointListIndex;
    private Transform[] waypoints;
    public Vector3 target;
    public int zombieType;   //used for save/load
    public int zombieSkin;   //used for save/load
    private Transform player;
    private LayerMask targetMask;
    
    private float hitboxRadius;

    private Animator animator;
    private float speedFactor;
    private NavMeshAgent agent;

    private EnemyCombat combat;
    private EnemyMovement movement;
    //private FieldOfView fov;
    public EntityHealth health;

    //behaviour states
    private bool isAlive = true;
    public bool isVictorious;
    public bool swarmMode;   //used for save/load
    public bool chaseTarget = false;
    private bool screamCooldown = false;

    private ZombieAudio zombieAudio;

    /// <summary>
    /// Constructor function for Zombies
    /// </summary>
    /// <param name="spawner"></param>
    /// <param name="_wayPointListIndex"></param>
    /// <param name="_waypoints"></param>
    /// <param name="_swarmMode"></param>
    /// <param name="_player"></param>
    /// <param name="_targetMask"></param>
    /// <param name="_zombieType"></param>
    /// <param name="_hitboxRadius"></param>
    /// <param name="_agentSpeed"></param>
    /// <param name="fov_angle"></param>
    /// <param name="fov_radius"></param>
    /// <returns></returns>
    public Zombie Init(ZombieSpawner spawner,int _wayPointListIndex,Transform[] _waypoints,bool _swarmMode, Transform _player, LayerMask _targetMask ,int _zombieType, int _zombieSkin, float _hitboxRadius, float _agentSpeed, float fov_angle, float fov_radius) {
     
        this.spawner = spawner;
        this.wayPointListIndex = _wayPointListIndex;
        this.waypoints = _waypoints;
        this.swarmMode= _swarmMode;
        this.player = _player;
        this.zombieType = _zombieType;
        this.zombieSkin = _zombieSkin;
        this.targetMask= _targetMask;
        this.hitboxRadius = _hitboxRadius;
        this.speedFactor = _agentSpeed;

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        combat = gameObject.AddComponent<EnemyCombat>().Init(animator, targetMask, hitboxRadius, this);
        movement = gameObject.AddComponent<EnemyMovement>().Init(agent, animator, waypoints);

        //intialise fov
        GetComponentInChildren<FieldOfView>().Init(this, fov_angle, fov_radius, targetMask);
        health = GetComponentInChildren<EntityHealth>();

        entityMask = LayerMask.GetMask("Enemy");

        zombieAudio = GetComponent<ZombieAudio>();
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
                chaseTarget = true;
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
        zombieAudio.PlayGrowlSound();

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

        zombieAudio.PlayDeathSound();
        StartCoroutine(Despawn());
        isAlive = false;
        spawner.removeZombieFromList(this);
        //remove zombie from spawner list  
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(15);
        Destroy(gameObject);
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
        AlertNearbyZombies();
        swarmMode = true;
        //chaseTarget = true;
        screamCooldown = false;
        agent.isStopped = false;
    }

    // private void OnDrawGizmos()
    // {
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(transform.position, hitboxRadius * 15);
    //        
    // }

    private void AlertNearbyZombies()
    {
        //check that the target entity is in range of this entity
        Collider[] targetsinRange = Physics.OverlapSphere(transform.position, hitboxRadius * 15, entityMask);

        if (targetsinRange.Length > 0)
        {
            foreach (Collider col in targetsinRange)
            {
                Zombie target = col.GetComponent<Zombie>();
                if (target != null)
                {
                    target.swarmMode = true;
                    //target.chaseTarget= true;
                }
            }

        }

    }
}
