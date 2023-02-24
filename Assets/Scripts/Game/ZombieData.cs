using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZombieData 
{
 
    //data to save
    public int zombieType;
    public bool swarmMode;
    public float health;
    public int wayPointListIndex;
    public float[] position = new float[3];


    public ZombieData(Zombie zombie)
    {
        this.zombieType = zombie.zombieType;
        this.swarmMode= zombie.swarmMode;
        this.health = zombie.health.currentHealth;
        this.wayPointListIndex = zombie.wayPointListIndex;
        
        //the transform position of the zombie
        this.position[0] = zombie.transform.position.x;
        this.position[1] = zombie.transform.position.y;
        this.position[2] = zombie.transform.position.z;

    }
}
