using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    public Transform player;
    [SerializeField]
    private float offset;

    private Camera mainCam;

    [SerializeField]
    private LayerMask ObstructionLayer;


    private void Start()
    {
        mainCam= GetComponent<Camera>();
    }

    private void Update()
    {
        Vector2 cutoutPos = mainCam.WorldToViewportPoint(player.position);
        cutoutPos.y /= (Screen.width / Screen.height);

        Vector3 viewOffset = player.position - transform.position;
        RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, viewOffset, viewOffset.magnitude, ObstructionLayer);

        for (int i = 0; i < hitObjects.Length; i++)
        {
            Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;

            for (int m = 0; m < materials.Length; m++)
            {
                materials[m].SetVector("_CutoutPos", cutoutPos);
                materials[m].SetFloat("_CutoutSize", 0.1f);
                materials[m].SetFloat("_FalloffSize", 0.05f);
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(player.position.x, transform.position.y, player.position.z - offset);
    }
}
