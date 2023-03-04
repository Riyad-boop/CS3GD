using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonMovement : MonoBehaviour
{
    // reference variables 
    private CharacterController characterController;
    private PlayerInput playerInput;

    // variables to store player input for movement
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 moveDirection;
    bool isMovementPressed;

    [SerializeField]
    // variables for animation 
    private float acceleration = 6f;
    private float deceleration = 100f;
    private float rotationSmoothTime = 0.1f;
    private float rotationSpeed = 5f;
    private float velocity = 3f;
    private int velocityHash;

    public Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        playerInput= new PlayerInput();
        characterController = GetComponent<CharacterController>();

        playerInput.Gameplay.Enable();

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

        float targetAngle = Mathf.Atan2(currentMovement.x, currentMovement.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y,targetAngle,ref rotationSpeed, rotationSmoothTime);

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


    private void Update()
    {
        handleRotation();
        characterController.Move(moveDirection * velocity * Time.deltaTime);
    }

    private void OnDisable()
    {
        playerInput.Gameplay.Disable();
    }
}
