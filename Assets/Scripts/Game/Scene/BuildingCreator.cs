using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildingCreator : MonoBehaviour
{
    public int minBlocks = 3;
    public int maxBlocks = 8;
    public Transform[] leftSpawnPoints;
    public Transform[] rightSpawnPoints;

    public GameObject[] baseBlocks;
    public GameObject[] middleBlocks;
    public GameObject[] roofBlocks;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < leftSpawnPoints.Length; i++)
        {
            Build(leftSpawnPoints[i]);
        }

        //for buildings on the right we need to flip on the y axis
        for (int i = 0; i < rightSpawnPoints.Length; i++)
        {
            Transform spawnpoint = rightSpawnPoints[i];
            spawnpoint.rotation = new Quaternion(0, 180, 0, 0);
            Build(rightSpawnPoints[i]);
            //then we can reset the position
            spawnpoint.rotation = new Quaternion(0, 0, 0, 0);
        }

    }

    void Build(Transform spawnPoint)
    {
        int targetBlocks = Random.Range(minBlocks, maxBlocks);
        float heightOffset = 0;
        float scaleOffset = 5;
        //spawning the base layer
        heightOffset += SpawnPieceLayer(baseBlocks, spawnPoint, heightOffset, scaleOffset, true);


        //spawn middle layers
        for (int i = 2; i < targetBlocks; i++)
        {
            heightOffset += SpawnPieceLayer(middleBlocks, spawnPoint, heightOffset, scaleOffset, false);
        }

        //spawn roof layer
        SpawnPieceLayer(roofBlocks, spawnPoint, heightOffset, scaleOffset, false);
    }

    float SpawnPieceLayer(GameObject[] pieceArray, Transform spawnPoint, float inputHeight, float scaleOffset, bool baselayer)
    {
        Transform randomTransform = pieceArray[Random.Range(0, pieceArray.Length)].transform;
        randomTransform.localScale = new Vector3(scaleOffset, scaleOffset, scaleOffset);
        GameObject clone = Instantiate(randomTransform.gameObject, spawnPoint.position + new Vector3(0, inputHeight, 0), transform.rotation);

        //set the layer to show on the minimap
        clone.gameObject.layer = 7; //LayerMask.GetMask("Obstruction");

        Mesh cloneMesh = clone.GetComponentInChildren<MeshFilter>().mesh;
        Bounds bounds = cloneMesh.bounds;
        float heightOffset = bounds.size.y * scaleOffset;

        clone.transform.SetParent(spawnPoint);

        //add box colider for base layer only
        if (baselayer)
        {
            GameObject baseObject = clone.transform.GetChild(0).gameObject;
            baseObject.AddComponent<BoxCollider>();
            baseObject.AddComponent<NavMeshObstacle>().carving= true;
            
        }
        

        return heightOffset;
    }


}
