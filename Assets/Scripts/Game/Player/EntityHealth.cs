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

    private void Start()
    {
        currentHealth = maxHealth;
        cam = Camera.main;
        setHealhBar();
    }

    public void damageEntity(int damage)
    {
        currentHealth -= damage;

        setHealhBar();

        if (currentHealth <= 0)
        {
            Debug.Log("Death");
        }
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
