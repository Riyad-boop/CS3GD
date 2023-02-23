using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerCombat : MonoBehaviour
{
    public LayerMask targetMask;
    public float hitboxRadius;
    private PlayerInput playerInput;
    private Animator animator;
    private bool attackCooldown = false;

    public PlayerCombat Initialiser(PlayerInput _playerInput, Animator _anim)
    {
        this.playerInput = _playerInput;
        this.animator = _anim;
        return this;
    }


    // Update is called once per frame
    void Update()
    {
        if (playerInput != null)
        {
            // handling inputs for keydown
            bool attackKeyHeld = playerInput.Gameplay.Attack.ReadValue<float>() > 0.1f;

            if (attackKeyHeld && !attackCooldown)
            {
                StartCoroutine(Attack());
            }
        }

    }

    public IEnumerator Attack()
    {
        attackCooldown= true;
        animator.SetTrigger("Attack");
        //wait one second for animation to play then call the damage player function 
        yield return new WaitForSeconds(1);
        DamageEntity(15);
        yield return new WaitForSeconds(1);
        attackCooldown = false;
    }


    public void DamageEntity(int damage)
    {
        //check that the target entity is in range of this entity
        Collider[] targetsinRange = Physics.OverlapSphere(transform.position, hitboxRadius, targetMask);

        if (targetsinRange.Length > 0)
        {
            foreach(Collider col in targetsinRange)
            {
                EntityHealth target = col.GetComponentInChildren<EntityHealth>();
                if (target != null)
                {
                    //change state if player is dead
                    if (target.damageEntity(damage))
                    {
                        Debug.Log("enemy felled");
                    }
                }
            }
        
        }

    }

}
