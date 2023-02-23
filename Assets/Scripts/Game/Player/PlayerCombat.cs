using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerCombat : MonoBehaviour
{

    private PlayerInput playerInput;

    public PlayerCombat Initialiser(PlayerInput _playerInput)
    {
        playerInput = _playerInput;
        return this;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput != null)
        {
            // handling inputs for keydown
            bool attackKeyHeld = playerInput.Gameplay.Attack.ReadValue<float>() > 0.1f;

            if (attackKeyHeld)
            {
                print("Attack");
            }
        }

    }

}
