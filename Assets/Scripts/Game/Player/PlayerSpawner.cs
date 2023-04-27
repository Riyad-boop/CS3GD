using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    private LayerMask targetMask;
    [SerializeField]
    private float hitboxRadius;

    [SerializeField]
    public ZombieSpawner zombieSpawner;

    public FollowPlayer MainCam;
    public FollowPlayer MinimapCam;

    [SerializeField]
    private GameObject playerPrefab;

    public Player player;

    private Player SpawnPlayer(int level, int kills, GameManager gameManager)
    {
        //spawn player 
        GameObject playerGameObject = Instantiate(playerPrefab, transform);
        player = playerGameObject.GetComponent<Player>();
        player.Init(_level: level, kills, targetMask, hitboxRadius, gameManager);

        //add player ref to cameras
        MainCam.player = playerGameObject.transform;
        MinimapCam.player = playerGameObject.transform;

        //add player ref to zombie spawner
        zombieSpawner.playerPos = playerGameObject.transform;


        return player;
    }

    public void SpawnNewPlayer(int level, int kills, GameManager gameManager)
    {
        player = SpawnPlayer(level,kills ,gameManager);
    }

    public void SavePlayer()
    {
        SaveLoadSystem.SavePlayer(player);
    }

    public void LoadPlayer(GameManager gameManager)
    {
        PlayerData data = SaveLoadSystem.LoadPlayer();

        player = SpawnPlayer(data.level, data.killCount, gameManager);

        // set the current health and update the healthbar
        player.health.currentHealth = data.health;
        player.health.setHealhBar();

        // load the player position
        Vector3 loadPosition;
        loadPosition.x = data.position[0];
        loadPosition.y = data.position[1];
        loadPosition.z = data.position[2];

        //the character controller prevents teleportation so we can just temporaily disable then re-enable after load 
        var controller = player.GetComponent<CharacterController>();
        controller.enabled = false;
        this.transform.position = loadPosition;
        controller.enabled = true;
    }
}
