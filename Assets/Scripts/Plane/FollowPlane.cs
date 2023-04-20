using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// NOT USED ANYMORE
/// </summary>
public class FollowPlane : MonoBehaviour
{
    [SerializeField]
    public Transform player;
    [SerializeField]
    private float offset;


    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(player.position.x, player.position.y, player.position.z - offset);
    }
}
