using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class FieldOfView : MonoBehaviour
{
    public float radius;

    [Range(0,360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask playerMask;
    public LayerMask obstructionsMask;

    public bool playerVisibility;

    private EnemyMovement enemyBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        //playerRef = GameObject.FindGameObjectsWithTag()
        enemyBehaviour = GetComponentInParent<EnemyMovement>();

    }

    private void FOV()
    {
        // Overlap sphere around the enemy to detect the playermask in the view radius
        Collider[] playerinRange = Physics.OverlapSphere(transform.position, radius, playerMask);


        if (playerinRange.Length > 0 )
        {
            for (int i = 0; i < playerinRange.Length; i++)
            {

                Transform player = playerinRange[i].transform;
                Vector3 directionToPlayer = (player.position - transform.position).normalized;

                //check the direction to player is within the fov angle
                if (Vector3.Angle(transform.forward, directionToPlayer) < angle / 2)
                {
                    //  Distance of the enemy and the player
                    float dstToPlayer = Vector3.Distance(transform.position, player.position);

                    //check taht the player is not behind any obstructions
                    if (!Physics.Raycast(transform.position, directionToPlayer, dstToPlayer, obstructionsMask))
                    {
                        playerVisibility = true;             //  The player has been seeing by the enemy and then the enemy starts to chasing the player
                        enemyBehaviour.playerPos = player.position;
                        enemyBehaviour.chasePlayer = true;
                        //chasePlayer = false;                 //  Change the state to chasing the player
                    }
                    else
                    {
                        playerVisibility = false;
                        enemyBehaviour.playerPos = Vector3.zero;
                        enemyBehaviour.chasePlayer = false;
                    }
                }
                else
                {
                    playerVisibility = false;
                    enemyBehaviour.playerPos = Vector3.zero;
                    enemyBehaviour.chasePlayer = false;
                }

                // if the player is outside the view radius then the player can not be seen
                if (Vector3.Distance(transform.position, player.position) > radius)
                {
                    playerVisibility = false;              
                }
            }
        }
        else if (playerVisibility)
        {
          playerVisibility = false;
        }
        
    }


   private void ShowFOV()
   {
        Gizmos.color = Color.yellow;

        Vector3 viewAngleLeft = DirectionFromAngle(transform.eulerAngles.y, -angle / 2);
        Vector3 viewAngleRight = DirectionFromAngle(transform.eulerAngles.y, angle / 2);

        Gizmos.DrawLine(transform.position, transform.position + viewAngleLeft * radius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleRight * radius);
   
        //draw a line to player if player is seen
        if (playerVisibility)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, playerRef.transform.position);
        }
   }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void OnDrawGizmos()
    {
        ShowFOV();
    }

    // Update is called once per frame
    void Update()
    {
        FOV();
    }
}
