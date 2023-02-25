using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ZombieSpawner : MonoBehaviour
{

    // numbre of zombies to spawn
    [SerializeField]
    private int spawnNumber;

    //prefab tro spawn zombies
    [SerializeField]
    private GameObject zombiePrefab;

    [SerializeField]
    private GameObject[] waypointLists;

    // list of zombie objects
    public List<Zombie> zombies;

    // player ref - set by player spawner
    public Transform playerPos;

    [SerializeField]
    private LayerMask targetMask;

    // // Start is called before the first frame update
    // void Start()
    // {
    //     if (waypointLists.Length >= 1 && spawnNumber > 0)
    //     {
    //         {
    //             for (int i = 0; i < spawnNumber; i++)
    //             {
    //                 int wayPointListIndex = 0;
    //                 Transform[] waypoints = getWaypointLocations(waypointLists[wayPointListIndex]);
    //
    //                 //generate random number to spawn enemy
    //                 Transform spawnPoint = waypoints[Random.Range(0, waypoints.Length)];
    //
    //                 var zombie = GameObject.Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);
    //
    //                 //TODO create two zombie types
    //                 zombie.AddComponent<Zombie>().Init(this, wayPointListIndex , waypoints, _swarmMode: false, playerPos, targetMask, _zombieType: 0, _hitboxRadius: 1f, _agentSpeed: 0.5f,fov_angle: 100f,fov_radius: 7f);
    //                 addZombieToList(zombie.GetComponent<Zombie>());
    //             }
    //         }
    //     }
    // }
    //

    public void NewGameSpawn()
    {
        if (waypointLists.Length >= 1 && spawnNumber > 0)
        {
            {
                for (int i = 0; i < spawnNumber; i++)
                {
                    int wayPointListIndex = 0;
                    Transform[] waypoints = getWaypointLocations(waypointLists[wayPointListIndex]);

                    //generate random number to spawn enemy
                    Transform spawnPoint = waypoints[Random.Range(0, waypoints.Length)];

                    GameObject zombie = Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);

                    //TODO create two zombie types
                    zombie.AddComponent<Zombie>().Init(this, wayPointListIndex, waypoints, _swarmMode: false, playerPos, targetMask, _zombieType: 0, _hitboxRadius: 1f, _agentSpeed: 0.5f, fov_angle: 100f, fov_radius: 7f);
                    addZombieToList(zombie.GetComponent<Zombie>());
                }
            }
        }
    }

    public void SaveZombies()
    {
        SaveLoadSystem.SaveZombies();
    }

    public void LoadZombies()
    {
        List<ZombieData> zombieDataList = SaveLoadSystem.loadZombies();

        foreach(ZombieData data in zombieDataList)
        {
            int wayPointListIndex = data.wayPointListIndex;
            Transform[] waypoints = getWaypointLocations(waypointLists[wayPointListIndex]);

            //generate random number to spawn enemy
            Vector3 spawnPoint;
            spawnPoint.x = data.position[0];
            spawnPoint.y = data.position[1];
            spawnPoint.z = data.position[2];

            var zombie = GameObject.Instantiate(zombiePrefab, spawnPoint, Quaternion.identity);

            zombie.AddComponent<Zombie>().Init(this, wayPointListIndex, waypoints, _swarmMode: data.swarmMode, playerPos, targetMask, _zombieType: data.zombieType, _hitboxRadius: 1f, _agentSpeed: 0.5f, fov_angle: 100f, fov_radius: 7f);
            Zombie zombieComponent = zombie.GetComponent<Zombie>();
            zombieComponent.health.currentHealth = data.health;
            zombieComponent.health.setHealhBar();
            addZombieToList(zombieComponent);
        }
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

    //TODO clear zombie list?
    /// <summary>
    /// change the state of all zombies to victory
    /// </summary>
    public void Victory()
    {
        foreach (Zombie zombie in zombies)
        {
            zombie.Victory();
        }
    }

    /// <summary>
    /// adds the zombie to list and save system's version of the list
    /// </summary>
    /// <param name="zombie"></param>
    public void addZombieToList(Zombie zombie)
    {
        zombies.Add(zombie);
        SaveLoadSystem.zombies = zombies;
    }

    /// <summary>
    /// delete's the zombie to list and save system's version of the list
    /// </summary>
    /// <param name="zombie"></param>
    public void removeZombieFromList(Zombie zombie)
    {
        zombies.Remove(zombie);
        SaveLoadSystem.zombies = zombies;
    }

}
