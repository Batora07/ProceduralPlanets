using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// One of the faces of a planet, contains mesh construction of this plane / face
/// </summary>
public class TerrainFace {

    private Mesh mesh;
    private int resolution;
    private Vector3 localUp;

    private Vector3 axisA;
    private Vector3 axisB;

    public TerrainFace(Mesh _mesh, int _resolution, Vector3 _localUp)
    {
        this.mesh = _mesh;
        this.resolution = _resolution;
        this.localUp = _localUp;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }

    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[resolution * resolution];
        //-> 6 = number of triangles to make a square * number of vertices to make a triangle (2*3)
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int i = 0;
        int triIndex = 0;

        for(int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                // how close to complete each of these loops is
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
                // transform the current planar cube as a spherical/round plane
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                vertices[i] = pointOnUnitSphere;

                // make a square for this mesh
                if (x != resolution - 1 && y != resolution - 1)
                {
                    // first triangle
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;
                    // second triangle
                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + resolution + 1;
                     // we added 6 vertices, update the index count
                    triIndex += 6;
                } 
                ++i;
            }
        }
        // clear all the data from the mesh to prevent 
        // adding to the previous lower resolution mesh
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
