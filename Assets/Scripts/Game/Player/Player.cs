using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private InputAction menu;
    private bool isPaused;
    private GameObject overlayCavnas;
    [SerializeField]
    private GameObject escapeMenu;
    [SerializeField]
    private GameObject gameOverMenu;

    // reference variables 
    private PlayerInput playerInput;

    public PlayerCombat combat;
    public EntityHealth health;
    public PlayerMovement movement;

    [Header("Components (set in script)")]
    public GameManager gameManager;
    public int level;     //variables saved to file


    [Header("Movement")]
    // variables for movement
    [SerializeField]
    private float acceleration = 3f;
    [SerializeField]
    private float deceleration = 100f;
    [SerializeField]
    private float rotationSpeed = 5f;

    [Header("Combat")]
    //varaibles for combat
    [SerializeField]
    private LayerMask targetMask;
    [SerializeField]
    private float hitboxRadius;

    private PlayerAudio playerAudio;


    public Player Init(int _level,int kills,LayerMask _targetMask,float _hitboxRadius , GameManager _gameManager)
    {
        this.level = _level;
        this.targetMask = _targetMask;
        this.hitboxRadius = _hitboxRadius;
        this.gameManager = _gameManager;
        this.playerInput = new PlayerInput();
        this.combat.Init(playerInput,targetMask,hitboxRadius);
        this.movement.Init(playerInput,acceleration,deceleration,rotationSpeed);

        overlayCavnas = health.gameObject;
        playerInput.Gameplay.Enable();
        
        menu = playerInput.Gameplay.GameMenu;
        menu.Enable();
        menu.performed += PauseGame;

        //set kill count
        this.combat.setKillCount(kills);

        playerAudio = GetComponent<PlayerAudio>();
        return this;
    }

    private void PauseGame(InputAction.CallbackContext callback)
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            escapeMenu.SetActive(true);
            overlayCavnas.SetActive(false);

            // pause time and audio
            Time.timeScale = 0f;
            AudioListener.pause = true;
        }

        else
        {
            escapeMenu.SetActive(false);
            overlayCavnas.SetActive(true);

            // resume time and audio
            if (Time.timeScale == 0f)
            {
                Time.timeScale = 1.0f;
                AudioListener.pause = false;
            }
        }
    }

    private void OnDisable()
    {
        playerInput.Gameplay.Disable();
        menu.Disable();
    }

    public void handleDeath()
    {
        movement.enabled= false;
        playerAudio.PlayDeathSound();
        playerInput.Gameplay.Disable();
        menu.Disable();
        gameOverMenu.SetActive(true);
    }

}
