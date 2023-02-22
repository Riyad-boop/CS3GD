using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public Material VisionConeMaterial;
    private Mesh VisionConeMesh;
    private MeshFilter VisionConeMeshFilter;

    private EnemyMovement enemyBehaviour;


    // Start is called before the first frame update
    void Start()
    {
        //playerRef = GameObject.FindGameObjectsWithTag()
        enemyBehaviour = GetComponentInParent<EnemyMovement>();
        transform.AddComponent<MeshRenderer>().material = VisionConeMaterial;
        VisionConeMeshFilter = transform.AddComponent<MeshFilter>();
        VisionConeMesh = new Mesh();
      
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
                        setVisionConeColour(new Color(0.9f, 0, 0, 0.4f));
                        //chasePlayer = false;                 //  Change the state to chasing the player
                    }
                    else
                    {
                        playerVisibility = false;
                        enemyBehaviour.playerPos = Vector3.zero;
                        enemyBehaviour.chasePlayer = false;
                        setVisionConeColour(new Color(0.7f, 0.7f, 0.7f, 0.4f));
                    }
                }
                else
                {
                    playerVisibility = false;
                    enemyBehaviour.playerPos = Vector3.zero;
                    enemyBehaviour.chasePlayer = false;
                    setVisionConeColour(new Color(0.7f, 0.7f, 0.7f, 0.4f));
                }

                // if the player is outside the view radius then the player can not be seen
                if (Vector3.Distance(transform.position, player.position) > radius)
                {
                    playerVisibility = false;
                    enemyBehaviour.playerPos = Vector3.zero;
                    enemyBehaviour.chasePlayer = false;
                    setVisionConeColour(new Color(0.7f, 0.7f, 0.7f, 0.4f));
                }
            }
        }
        else if (playerVisibility)
        {
            enemyBehaviour.playerPos = Vector3.zero;
            enemyBehaviour.chasePlayer = false;
            playerVisibility = false;
            setVisionConeColour(new Color(0.7f, 0.7f, 0.7f, 0.4f));
        }
        
    }

    private void setVisionConeColour(Color colour)
    {
        var coneRenderer = GetComponent<MeshRenderer>();
        coneRenderer.material.SetColor("_BaseColor", colour);
    }


    /// <summary>
    /// https://www.youtube.com/watch?v=luLrhoTZYD8
    /// </summary>
    void DrawVisionCone()//this method creates the vision cone mesh
    {
        int VisionConeResolution = 120;
        float VisionAngle = angle * Mathf.Deg2Rad;

        int[] triangles = new int[(VisionConeResolution - 1) * 3];
        Vector3[] Vertices = new Vector3[VisionConeResolution + 1];
        Vertices[0] = Vector3.zero;
        float Currentangle = -VisionAngle / 2;
        float angleIcrement = VisionAngle / (VisionConeResolution - 1);
        float Sine;
        float Cosine;

        for (int i = 0; i < VisionConeResolution; i++)
        {
            Sine = Mathf.Sin(Currentangle);
            Cosine = Mathf.Cos(Currentangle);
            Vector3 RaycastDirection = (transform.forward * Cosine) + (transform.right * Sine);
            Vector3 VertForward = (Vector3.forward * Cosine) + (Vector3.right * Sine);
            if (Physics.Raycast(transform.position, RaycastDirection, out RaycastHit hit, radius, obstructionsMask))
            {
                Vertices[i + 1] = VertForward * hit.distance;
            }
            else
            {
                Vertices[i + 1] = VertForward * radius;
            }


            Currentangle += angleIcrement;
        }
        for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j + 2;
        }
        VisionConeMesh.Clear();
        VisionConeMesh.vertices = Vertices;
        VisionConeMesh.triangles = triangles;
        VisionConeMeshFilter.mesh = VisionConeMesh;
    }


    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    // Update is called once per frame
    void Update()
    {
        FOV();
        DrawVisionCone();
    }

    private void OnDrawGizmos()
    {
        //ShowFOV();
    }

    private void ShowFOV()
    {
        // Gizmos.color = Color.black;
        // Gizmos.DrawWireSphere(transform.position, radius);

        Gizmos.color = Color.red;

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

}
