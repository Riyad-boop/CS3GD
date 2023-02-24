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

    public GameObject targetRef;

    public LayerMask targetMask;
    public LayerMask obstructionsMask;

    public bool playerVisibility;

    public Material VisionConeMaterial;
    private Mesh VisionConeMesh;
    private MeshFilter VisionConeMeshFilter;

    private Zombie parent;
    private bool chaseMode = false;


    public FieldOfView Init(Zombie _parent, float _angle,float _radius, LayerMask _targetMask)
    {
        this.parent = _parent;
        this.angle = _angle;
        this.radius = _radius;
        this.targetMask = _targetMask;
        this.transform.AddComponent<MeshRenderer>().material = VisionConeMaterial;
        this.VisionConeMeshFilter = transform.AddComponent<MeshFilter>();
        this.VisionConeMesh = new Mesh();

        return this;
    }

  // // Start is called before the first frame update
  // void Start()
  // {
  //     //playerRef = GameObject.FindGameObjectsWithTag()
  //     enemyBehaviour = GetComponentInParent<Zombie>();
  //     transform.AddComponent<MeshRenderer>().material = VisionConeMaterial;
  //     VisionConeMeshFilter = transform.AddComponent<MeshFilter>();
  //     VisionConeMesh = new Mesh();
  //   
  // }

    private void FOV()
    {
        // Overlap sphere around the enemy to detect the playermask in the view radius
        Collider[] playerinRange = Physics.OverlapSphere(transform.position, radius, targetMask);


        if (playerinRange.Length > 0 )
        {
            Transform player = playerinRange[0].transform;
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            //check the direction to player is within the fov angle
            if (Vector3.Angle(transform.forward, directionToPlayer) < angle / 2)
            {
                //  Distance of the enemy and the player
                float dstToPlayer = Vector3.Distance(transform.position, player.position);

                //check that the player is not behind any obstructions
                if (!Physics.Raycast(transform.position, directionToPlayer, dstToPlayer, obstructionsMask))
                {
                    // check that the player target is within the view radius
                    if (Vector3.Distance(transform.position, player.position) < radius)
                    {
                        playerVisibility = true; //  The player has been seeing by the enemy and then the enemy starts to chasing the player

                        parent.target = player.position;
                        parent.chaseTarget = true;
                        setVisionConeColour(new Color(0.9f, 0, 0, 0.4f));

                        if(parent.zombieType == 0)
                        {
                           chaseMode = true;
                        }
                        else if(parent.zombieType == 1)
                        {
                            parent.Scream();
                        }
                        
                        //StopCoroutine(stopChaseMode());
                        StopAllCoroutines();
                    }
                }
                else
                {
                    stopChasing();
                }
            }
            else
            {
                stopChasing();
            }
        }
        else if (playerVisibility)
        {  
            stopChasing();
        }
        
    }

    private void stopChasing()
    {
        playerVisibility = false;

        // if the entity is currently in chase target mode
        if (chaseMode)
        {
            //check if player is nearby (70% of radius).
            Collider[] playersinRange = Physics.OverlapSphere(transform.position, radius * 0.7f, targetMask);

            // if player is still nearby then continue the chase by getting the player's position, rather than relying on visibility.
            if (playersinRange.Length > 0)
            {
                Transform player = playersinRange[0].transform;
                parent.target = player.position;
                parent.chaseTarget = true;
                setVisionConeColour(new Color(0.9f, 0, 0, 0.2f));
            }
            //else stop chase if player is far enough away
            else
            {
                setVisionConeColour(new Color(0.7f, 0.7f, 0.7f, 0.2f));
                StartCoroutine(stopChaseMode());
            }
        }
    }

    private IEnumerator stopChaseMode()
    {
        chaseMode= false;

        //chase only for 5 seconds after visibility is lost
        yield return new WaitForSeconds(5);

        //Then go back to patrol mode (if swarm mode is not on)
        if (!playerVisibility)
        {
            parent.target = Vector3.zero;
            parent.chaseTarget = false;
           
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


    // Update is called once per frame
    void Update()
    {
        //if parent has not been set then return 
        //if (!parent) { return; }

        // if swarm is disabled then show fov
        if (!parent.swarmMode)
        {
            FOV();
            DrawVisionCone();
        }
        //in swarm mode disable mesh renderer
        else
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
       
    }

  // private void OnDrawGizmos()
  // {
  //     //ShowFOV();
  // }
  // 
  // 
  // private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
  // {
  //     angleInDegrees += eulerY;
  // 
  //     return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
  // }
  // private void ShowFOV()
  // {
  //     // Gizmos.color = Color.black;
  //     // Gizmos.DrawWireSphere(transform.position, radius);
  // 
  //     Gizmos.color = Color.red;
  // 
  //     Vector3 viewAngleLeft = DirectionFromAngle(transform.eulerAngles.y, -angle / 2);
  //     Vector3 viewAngleRight = DirectionFromAngle(transform.eulerAngles.y, angle / 2);
  // 
  //     Gizmos.DrawLine(transform.position, transform.position + viewAngleLeft * radius);
  //     Gizmos.DrawLine(transform.position, transform.position + viewAngleRight * radius);
  // 
  //     //draw a line to player if player is seen
  //     if (playerVisibility)
  //     {
  //         Gizmos.color = Color.green;
  //         Gizmos.DrawLine(transform.position, targetRef.transform.position);
  //     }
  // }
}
