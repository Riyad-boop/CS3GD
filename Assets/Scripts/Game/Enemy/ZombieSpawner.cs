using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ZombieSpawner : MonoBehaviour
{

    // numbre of zombies to spawn
    [SerializeField]
    private int spawnNumber;

    //number of zombies to spawn
    [SerializeField]
    private GameObject zombiePrefab;

    [SerializeField]
    private GameObject[] waypointLists;

    // list of zombie objects
    public List<Zombie> zombies;

    [SerializeField]
    // player ref
    private Transform playerPos;

    [SerializeField]
    private LayerMask targetMask;

    // Start is called before the first frame update
    void Start()
    {
        // if(waypointLists.Length >= 1)
        // {
        //     Transform[] waypoints = getWaypointLocations(waypointLists[0]);
        //
        //     foreach (Transform g in waypoints)
        //     {
        //         Debug.Log(g.name);
        //     }
        // }

        // if(waypointLists.Length >= 1)
        // {
        //     for (int i = 0; i < spawnNumber; i++)
        //     {
        //         zombies[i] = new Zombie(getWaypointLocations(waypointLists[0]), player, 0);
        //     }
        // }

        //var zombie = GameObject.Instantiate(zombiePrefab);
        // zombie.AddComponent<Zombie>();

        if (waypointLists.Length >= 1 && spawnNumber > 0)
        {
            {
                for (int i = 0; i < spawnNumber; i++)
                {
                    Transform[] waypoints = getWaypointLocations(waypointLists[0]);

                    //generate random number to spawn enemy
                    Transform spawnPoint = waypoints[Random.Range(0, waypoints.Length)];

                    var zombie = GameObject.Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);

                    //TODO create two zombie types
                    zombie.AddComponent<Zombie>().Init(this, waypoints, playerPos, targetMask, _zombieType: 0, _hitboxRadius: 1f, _agentSpeed: 0.5f,fov_angle: 100f,fov_radius: 7f);
                    zombies.Add(zombie.GetComponent<Zombie>());
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// Extract all the child waypoints from the parent
    /// </summary>
    /// <param name="parentWaypoint"></param>
    /// <returns></returns>
    private Transform[] getWaypointLocations(GameObject parentWaypoint)
    {
        var waypoints = new List<Transform>();

        //bool to always skip the parent waypoint
        bool skip = true;

        foreach (Transform t in parentWaypoint.GetComponentsInChildren<Transform>())
        {
            if (skip)
            {
                skip= false;
                continue;
            }
            waypoints.Add(t);
        }

        return waypoints.ToArray();
    }


    public void removeZombieFromList(Zombie zombie)
    {
        zombies.Remove(zombie);
    }

    public void Victory()
    {
        foreach(Zombie zombie in zombies) { 
            zombie.Victory();
        }
    }
}
