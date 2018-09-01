using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

    /* PUBLIC FIELDS */
    [Range(2,256)]
    public int resolution = 10;
    public ShapeSettings shapeSettings;
    public ColourSettings colourSettings;
    [HideInInspector]
    public bool shapeSettingsFoldOut;[
    HideInInspector]
    public bool colourSettingsFoldOut;

    /* PRIVATE FIELDS */
    private ShapeGenerator shapeGenerator;
        /// <summary>
    /// the amount of maximum planes/faces a planet can have
    /// </summary>
    private const int MAX_FACES = 6;
    [SerializeField, HideInInspector]
    private  MeshFilter[] meshFilters;
    private TerrainFace[] terrainFaces;

    /// <summary>
    /// Use this to generate a new planet
    /// </summary>
    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColours();
    }

    /// <summary>
    /// Use this in case only the colour of a planet changed
    /// </summary>
    public void OnColourSettingsUpdated()
    {
        Initialize();
        GenerateColours();
    }

    /// <summary>
    /// Use this in case we want to change the shape of an existing planet
    /// </summary>
    public void OnShapeSettingsUpdated()
    {
        Initialize();
        GenerateMesh();
    }

    private void OnValidate()
    {
        Initialize();
        GeneratePlanet();
    }

    /// <summary>
    /// Initialize the planet mesh
    /// </summary>
    private void Initialize()
    {
        shapeGenerator = new ShapeGenerator(shapeSettings);

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
            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
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

    /// <summary>
    /// Loop throught the meshes and set the material colour to the ones from the colour settings
    /// </summary>
    private void GenerateColours()
    {
        foreach(MeshFilter m in meshFilters)
        {
            m.GetComponent<MeshRenderer>().sharedMaterial.color = colourSettings.planetColour;
        }
    }
}
