using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;

public class planeMovement : MonoBehaviour
{
    //escape button action
    private InputAction menu;

    [SerializeField]
    private float acceleration = 0.25f;
    private float currentVelocity = 0.5f;
    private float maxSpeed = 5f;

    private PlayerInput playerInput;

    [SerializeField]
    Vector3 currentMovement;

    [SerializeField]
    private GameObject propellerPrefab;
    [SerializeField]
    private GameObject damagedParticles;
    [SerializeField]
    private GameObject smokePrefab;
    [SerializeField]
    private GameObject canvasPrefab;
    [SerializeField]
    private GameObject characterPrefab;

    private AudioSource audioSource;

    public AudioClip explosionSound;
    public AudioClip bigExplosionSound;

    public bool isMoving = true;

    void Start()
    {
        this.audioSource = GetComponent<AudioSource>();
        this.playerInput = new PlayerInput();

        playerInput.Gameplay.Enable();
       
        // handling inputs for keydown
        playerInput.Gameplay.Move.started += onMovementInput;
        // handling inputs for keyup
        playerInput.Gameplay.Move.canceled += onMovementInput;
        // handling inputs for controller
        playerInput.Gameplay.Move.performed += onMovementInput;

        playerInput.Gameplay.Throttle.started += onThrottleInput;

        playerInput.Gameplay.Throttle.canceled+= onThrottleInput;

        playerInput.Gameplay.Throttle.performed+= onThrottleInput;

        //allow player to ship scene if escape key pressed
        menu = playerInput.Gameplay.GameMenu;
        menu.Enable();
        menu.performed += onSkipScene;


        StartCoroutine(Explosion());
       
    }

    public IEnumerator Explosion()
    {
        yield return new WaitForSeconds(30);
        audioSource.volume = 1;
        audioSource.pitch = 1;
        audioSource.PlayOneShot(explosionSound);
        audioSource.volume = 0.2f;
        smokePrefab.SetActive(false);
        damagedParticles.SetActive(true);
        yield return new WaitForSeconds(40);
        endScene();
    }

    private void endScene()
    {
        StopAllCoroutines();
        audioSource.loop = false;
        audioSource.volume = 0.8f;
        audioSource.pitch = 1;
        isMoving = false;
        playerInput.Gameplay.Disable();
        //characterPrefab.GetComponent<AudioSource>().Stop();
        audioSource.clip = bigExplosionSound;
        audioSource.Play();
        canvasPrefab.SetActive(true);
    }

    private void onSkipScene(InputAction.CallbackContext callback)
    {
        endScene();
    }

        private void onMovementInput(InputAction.CallbackContext context)
    {
        // roll and pitch control
        Vector2 currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.y = currentMovementInput.y;
    
    }

    private void onThrottleInput(InputAction.CallbackContext context)
    {
        // roll and pitch control
        currentMovement.z = context.ReadValue<float>();
    }

    private void Update()
    {
        if (isMoving)
        {
            if (currentMovement.z > 0)
            {
                currentVelocity += Time.deltaTime * acceleration;
            }
            else if (currentMovement.z < 0)
            {
                currentVelocity -= Time.deltaTime * acceleration * 3;
            }

            if (currentVelocity <= 1)
            {
                currentVelocity = 1;
            }
            else if (currentVelocity >= maxSpeed)
            {

                currentVelocity = maxSpeed;
            }

            SpinPropeller();
        }
       
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            Vector3 movement = new Vector3(currentMovement.x, currentMovement.y, currentVelocity / 2);

            Vector3 newPosition = transform.position + movement * maxSpeed * Time.deltaTime;

            // Clamp the position so the plane does not move off the map
            newPosition.x = Mathf.Clamp(newPosition.x, -50f, 50f);
            newPosition.y = Mathf.Clamp(newPosition.y, 20f, 80f);

            // Set the player's position to the clamped position
            transform.position = newPosition;
        }
      
    }
    private void OnDisable()
    {
        playerInput.Gameplay.Disable();
    }


    /// <summary>
    /// Spins the propeller and changes the spin according the velocity.
    /// </summary>
    private void SpinPropeller()
    {
        propellerPrefab.transform.Rotate(0f, currentVelocity * 5, 0f);
        audioSource.pitch = (currentVelocity / maxSpeed) + 1;
    }
}
