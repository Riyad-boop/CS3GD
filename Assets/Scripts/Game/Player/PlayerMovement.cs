using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    private Vector3 moveDirection;
    bool isMovementPressed;

    [SerializeField]
    // variables for animation 
    private float acceleration = 6f;
    private float deceleration = 100f;
    private float velocity = 0;
    private int velocityHash;


    //variables for rotation
    [SerializeField]
    private Transform cam;
    [SerializeField]
    private float rotationSpeed = 5f;
    private float rotationSmoothTime = 0.1f;

    public PlayerMovement Init(PlayerInput _playerInput, float _acceleration, float _deceleration, float _rotationSpeed)
    {
        this.playerInput = _playerInput;
        this.animator= GetComponent<Animator>();
        this.characterController = GetComponent<CharacterController>();
        this.acceleration= _acceleration;
        this.deceleration= _deceleration;
        this.rotationSpeed= _rotationSpeed;

        velocityHash = Animator.StringToHash("Velocity");

        // handling inputs for keydown
        playerInput.Gameplay.Move.started += onMovementInput;
        // handling inputs for keyup
        playerInput.Gameplay.Move.canceled += onMovementInput;
        // handling inputs for controller
        playerInput.Gameplay.Move.performed += onMovementInput;

        return this;
    }

    private void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        isMovementPressed = currentMovement.x != 0 || currentMovement.z != 0;
    }

    //private void handleRotation()
    //{
    //    Vector3 lookAtPos;
    //    lookAtPos.x = currentMovement.x;
    //    lookAtPos.y = 0f;
    //    lookAtPos.z = currentMovement.z;
    //
    //    Quaternion currentRotation = transform.rotation;
    //
    //    if (isMovementPressed)
    //    {
    //        Quaternion targetRotation = Quaternion.LookRotation(lookAtPos);
    //        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
    //    }
    // 
    //}

    private void handleRotation()
    {

        float targetAngle = Mathf.Atan2(currentMovement.x, currentMovement.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, rotationSmoothTime);

        if (isMovementPressed)
        {
            transform.rotation = Quaternion.Euler(0, angle, 0);
            moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
        }
        else
        {
            moveDirection = Vector3.zero;
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

        animator.SetFloat(velocityHash, velocity);
    }

    private void Update()
    {
        handleAnimation();
        handleRotation();
        characterController.Move(moveDirection * velocity * Time.deltaTime);
    }

}
