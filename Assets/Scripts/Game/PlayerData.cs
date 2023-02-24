using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public int killCount;
    public float health;
    public float[] position = new float[3];

    public PlayerData(Player player)
    {
        this.killCount = player.combat.killCount;
        this.health = player.health.currentHealth;

        //the transform position of the player
        this.position[0] = player.transform.position.x;
        this.position[1] = player.transform.position.y;
        this.position[2] = player.transform.position.z;

    }
}
