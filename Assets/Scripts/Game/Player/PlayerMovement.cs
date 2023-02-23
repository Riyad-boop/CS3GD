using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // reference variables 
    private CharacterController characterController;
    private Animator animator;
    private PlayerInput playerInput;

    // variables to store player input for movement
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    bool isMovementPressed;

    [SerializeField]
    // variables for animation 
    private float acceleration = 6f;
    private float deceleration = 100f;
    private float velocity = 0;
    private int VelocityHash;

    private PlayerCombat combat;

    //variables for rotation
    [SerializeField]
    private float rotationFactor = 5f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerInput = new PlayerInput();
        VelocityHash = Animator.StringToHash("Velocity");

        combat = GetComponent<PlayerCombat>().Initialiser(playerInput,animator);


        // handling inputs for keydown
        playerInput.Gameplay.Move.started += onMovementInput;
        // handling inputs for keyup
        playerInput.Gameplay.Move.canceled += onMovementInput;
        // handling inputs for controller
        playerInput.Gameplay.Move.performed += onMovementInput;

    }


    private void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        isMovementPressed = currentMovement.x != 0 || currentMovement.z != 0;
    }

    private void handleRotation()
    {
        Vector3 lookAtPos;
        lookAtPos.x = currentMovement.x;
        lookAtPos.y = 0f;
        lookAtPos.z = currentMovement.z;

        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookAtPos);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactor * Time.deltaTime);
        }
     
    }

    private void handleAnimation()
    {

        if(isMovementPressed)
        {
            if(velocity < 10)
            {
                velocity += Time.deltaTime * acceleration;
            }
            else
            {
                velocity = 10f;
            }      
        }
        else
        {
            if(velocity > 0)
            {
                velocity -= Time.deltaTime * deceleration;
            }
            else
            {
                velocity= 0f;
            }
        }

        animator.SetFloat(VelocityHash, velocity);
    }

    private void Update()
    {
        handleAnimation();
        handleRotation();
        characterController.Move(currentMovement * velocity * Time.deltaTime);
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
