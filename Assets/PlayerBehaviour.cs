using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    public int health = 100;

    public void damagePlayer(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Debug.Log("Player death");
        }
    }
}
