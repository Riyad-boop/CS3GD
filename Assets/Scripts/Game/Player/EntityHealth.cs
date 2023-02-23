using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;

    [SerializeField]
    private Image spriteHealthBar;
    private Camera cam;

    private Animator animator;
    private CapsuleCollider capsuleCollider;

    private void Start()
    {
        cam = Camera.main;
        setHealhBar();
        animator = GetComponentInParent<Animator>();
        capsuleCollider = GetComponentInParent<CapsuleCollider>();
    }

    public bool damageEntity(int damage)
    {
        currentHealth -= damage;

        setHealhBar();

        bool death = currentHealth <= 0;

        if (death)
        {
            Death();
        }

        return death;
    }

    public void Death()
    {
        animator.enabled= false;
        capsuleCollider.enabled= false;
    }

    private void setHealhBar()
    {
       spriteHealthBar.fillAmount = currentHealth / maxHealth;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }
}
