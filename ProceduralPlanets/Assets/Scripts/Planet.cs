using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

    [Range(2,256)]
    public int resolution = 10;

    /// <summary>
    /// the amount of maximum planes/faces a planet can have
    /// </summary>
    private const int MAX_FACES = 6;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    private void OnValidate()
    {
        Initialize();
        GenerateMesh();
    }

    /// <summary>
    /// Initialize the planet mesh
    /// </summary>
    private void Initialize()
    {
        if(meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[MAX_FACES];
        }
      
        terrainFaces = new TerrainFace[MAX_FACES];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for(int i = 0; i < MAX_FACES; ++i)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObject = new GameObject("mesh");
                meshObject.transform.parent = transform;

                meshObject.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = meshObject.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
            terrainFaces[i] = new TerrainFace(meshFilters[i].sharedMesh, resolution, directions[i]);
        }
    }

    /// <summary>
    /// Generate the mesh for the planet
    /// </summary>
    private void GenerateMesh()
    {
        foreach(TerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }
    }
}
