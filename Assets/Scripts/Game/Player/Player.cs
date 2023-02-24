using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // reference variables 
    private PlayerInput playerInput;

    [Header("Combat")]
    //varaibles for combat
    [SerializeField]
    private LayerMask targetMask;
    [SerializeField]
    private float hitboxRadius;

    [Header("Movement")]
    // variables for movement
    [SerializeField]
    private float acceleration = 3f;
    [SerializeField]
    private float deceleration = 100f;
    [SerializeField]
    private float rotationSpeed = 5f;

    [Header("Components (set in script)")]
    public PlayerCombat combat;
    public PlayerMovement movement;
    public EntityHealth health;

    void Awake()
    {
        playerInput = new PlayerInput();
        combat = gameObject.AddComponent<PlayerCombat>().Init(playerInput,targetMask,hitboxRadius);
        movement = gameObject.AddComponent<PlayerMovement>().Init(playerInput,acceleration,deceleration,rotationSpeed);
        health = GetComponentInChildren<EntityHealth>();

        PlayerData data = SaveLoadSystem.LoadPlayer();
    }

    public void SavePlayer()
    {
        SaveLoadSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveLoadSystem.LoadPlayer();

        // set the current health and update the healthbar
        health.currentHealth = data.health;
        health.setHealhBar();

        //TODO update counter in UI
        combat.killCount = data.killCount;
        

        // load the player position
        Vector3 loadPosition;
        loadPosition.x = data.position[0];
        loadPosition.y = data.position[1];
        loadPosition.z = data.position[2];

        //the character controller prevents teleportation so we can just temporaily disable then re-enable after load 
        var controller = GetComponent<CharacterController>();
        controller.enabled = false;
        transform.position = loadPosition;
        controller.enabled = true;
    }


    private void OnEnable()
    {
        playerInput.Gameplay.Enable();
    }

    private void OnDisable()
    {
        playerInput.Gameplay.Disable();
    }

}
