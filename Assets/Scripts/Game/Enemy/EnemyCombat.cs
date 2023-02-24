using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    private float hitboxRadius;
    private bool attackCooldown = false;
    private Animator animator;
    private LayerMask targetMask;
    private Zombie parent;

    public EnemyCombat Init(Animator _anim, LayerMask _targetMask, float _hitboxRadius, Zombie _parent)
    {
        animator = _anim;
        targetMask = _targetMask;
        hitboxRadius = _hitboxRadius;
        parent = _parent;
        return this;
    }

    public void AttackPlayer(Vector3 currentTargetPos, Vector3 playerPos)
    {
        if (!attackCooldown)
        {
            // check if this gameobject is close to the target and that the player is clsoe to the target before attacking
            if ((Vector3.Distance(transform.position, currentTargetPos) < 2f) && (Vector3.Distance(playerPos, currentTargetPos) < 2f))
            {
               StartCoroutine(Attack());
               attackCooldown = true;
                
            }
        }
    }

    public IEnumerator Attack()
    {
        animator.SetTrigger("Attack");
        //wait one second for animation to play then call the damage player function 
        yield return new WaitForSeconds(1);
        DamageEntity(5);
        yield return new WaitForSeconds(2);
        attackCooldown = false;
    }


    public void DamageEntity(int damage)
    {
        //check that the target entity is in range of this entity
        Collider[] targetsinRange = Physics.OverlapSphere(transform.position, hitboxRadius, targetMask);

        if (targetsinRange.Length > 0)
        {
            EntityHealth target = targetsinRange[0].GetComponentInChildren<EntityHealth>();
            if (target != null)
            {
                //change state if player is dead
                if (target.damageEntity(damage))
                {
                    parent.Victory();
                }
            }        
        }
    }

    // private void OnDrawGizmos()
    // {
    //     //draw hitbox
    //     Gizmos.DrawWireSphere(transform.position, hitboxRadius);
    // }
}
