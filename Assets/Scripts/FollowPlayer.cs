using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    public Transform player;
    [SerializeField]
    private float offset;


    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(player.position.x, transform.position.y, player.position.z - offset);
    }
}
