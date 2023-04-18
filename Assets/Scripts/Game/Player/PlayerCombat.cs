using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerCombat : MonoBehaviour
{
    private LayerMask targetMask;
    private float hitboxRadius;
    private PlayerInput playerInput;
    private Animator animator;
    private bool attackCooldown = false;

    //playerData variables
    public int killCount;

    public TMP_Text killCountText;

    public PlayerCombat Init(PlayerInput _playerInput, LayerMask _targetMask ,float _hitboxRadius)
    {
        this.playerInput = _playerInput;
        this.animator = GetComponent<Animator>();
        this.targetMask = _targetMask;
        this.hitboxRadius = _hitboxRadius;
        setKillCountText();
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
        DamageEntity(20);
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
                Zombie target = col.GetComponent<Zombie>();
                if (target != null)
                {
                    if(!target.swarmMode && !target.chaseTarget)
                    {
                        //apply damage multipyer if the enemy is hit while unaware
                        damage *= 5;
                    }
                }

                EntityHealth targetHealth = col.GetComponentInChildren<EntityHealth>();

                if (targetHealth != null)
                {
                    //change state if player is dead
                    if (targetHealth.damageEntity(damage))
                    {
                        killCount++;
                        setKillCountText();
                    }
                }
            }
        
        }

    }

    public void setKillCountText()
    {
        killCountText.text = killCount.ToString();
    }

}
