using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //varialbes saved to file
    public int level;

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
    public GameManager gameManager;

    public Player Init(int _level,LayerMask _targetMask,float _hitboxRadius , GameManager _gameManager)
    {
        this.level = _level;
        this.targetMask = _targetMask;
        this.hitboxRadius = _hitboxRadius;
        this.gameManager = _gameManager;
        this.playerInput = new PlayerInput();
        this.combat = gameObject.AddComponent<PlayerCombat>().Init(playerInput,targetMask,hitboxRadius);
        this.movement = gameObject.AddComponent<PlayerMovement>().Init(playerInput,acceleration,deceleration,rotationSpeed);
        this.health = GetComponentInChildren<EntityHealth>();

        playerInput.Gameplay.Enable();

        return this;
    }

    public void SaveGame()
    {
        gameManager.SaveGame();
    }


   // private void OnEnable()
   // {
   //     playerInput.Gameplay.Enable();
   // }

    private void OnDisable()
    {
        playerInput.Gameplay.Disable();
    }

}
