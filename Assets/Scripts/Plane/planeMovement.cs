using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;

public class planeMovement : MonoBehaviour
{
  
    private PlayerInput playerInput;
    private Rigidbody rb;
    
    // variables to store player input for movement
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    bool isMovementPressed;
    
    [SerializeField]
    // variables for animation 
    private float acceleration = 1f;
    private float velocity;
    [SerializeField]
    private float maxThrottle = 1f;

    //variables for rotation
    [SerializeField]
    private float rotationSpeed = 0.5f;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        this.playerInput = new PlayerInput();
        this.rb = GetComponent<Rigidbody>();

        playerInput.Gameplay.Enable();

        // handling inputs for keydown
        playerInput.Gameplay.Move.started += onMovementInput;
        // handling inputs for keyup
        playerInput.Gameplay.Move.canceled += onMovementInput;
        // handling inputs for controller
        playerInput.Gameplay.Move.performed += onMovementInput;

       
        // tweaking rotationspeed
        //rotationSpeed = (rb.mass/10f) * rotationSpeed;
    }
    
    private void onMovementInput(InputAction.CallbackContext context)
    {
        // roll and pitch control
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;

    }

    private void handleInputs()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (velocity < 10)
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
            if (velocity > 0)
            {
                velocity -= Time.deltaTime * acceleration;
            }
            else
            {
                velocity = 0f;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        handleInputs();
    }

    private void FixedUpdate()
    {
        //throttle
        rb.AddForce(transform.forward * velocity * maxThrottle, ForceMode.Impulse);

        //pitch
        rb.AddTorque(transform.right * currentMovement.z * rotationSpeed,ForceMode.Impulse);

        //yaw
        rb.AddTorque(transform.up * currentMovement.x * rotationSpeed, ForceMode.Impulse);


    }

    private void OnDisable()
    {
        playerInput.Gameplay.Disable();
    }
}
