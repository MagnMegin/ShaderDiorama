using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshFilter))]
public class Surface : MonoBehaviour
{
    [SerializeField]
    int resolution = 20;
    [SerializeField]
    float heightOffset = 0f;
    [SerializeField]
    List<SurfaceFunction> surfaceFunctions;

    public HeightMap HeightMap => heightMap;
    public Mesh Mesh => mesh;

    [SerializeField]
    [HideInInspector]
    HeightMap heightMap;
    [SerializeField]
    [HideInInspector]
    Mesh mesh;
    [SerializeField]
    [HideInInspector]
    MeshFilter filter;

    bool valuesChanged;
    private Vector2Int VertexResolution => Vector2Int.one * (resolution + 1);

    #region Unity Messages
    private void Reset()
    {
        heightMap = new HeightMap(VertexResolution, HeightFunction);
        mesh = new Mesh();
        filter = GetComponent<MeshFilter>();
        filter.sharedMesh = mesh;
    }

    private void OnValidate()
    {
        valuesChanged = true;
        if (resolution < 1) resolution = 1;
    }

    private void Update()
    {
        if (!valuesChanged) return;
        if (mesh == null)
        {
            mesh = new Mesh();
            filter = GetComponent<MeshFilter>();
            filter.sharedMesh = mesh;
        }

        heightMap.Regenerate(VertexResolution, HeightFunction);
        GenerateMesh();
    }
    #endregion

    #region Helper Functions
    private float HeightFunction(float x, float y)
    {
        float sum = heightOffset;
        foreach (var modifier in surfaceFunctions)
        {
            sum += modifier.Evaluate(x, y);
        }

        return sum;
    }

    private void GenerateMesh()
    {
        Vector2Int resolution = heightMap.Resolution;
        int maxSideLength = Mathf.Max(resolution.x, resolution.y);

        Array2D<Vector3> vertices = new(resolution);
        for (int z = 0; z < resolution.y; z++)
        {
            for (int x = 0; x < resolution.x; x++)
            {
                float y = heightMap.HeightFromPixel(x, z);
                float x_coord = x / (float)(maxSideLength - 1);
                float z_coord = z / (float)(maxSideLength - 1);

                vertices[x, z] = new Vector3(x_coord, y, z_coord);
            }
        }

        int[] triangles = new int[6 * (resolution.x - 1) * (resolution.y - 1)];
        for (int vertex = 0, index = 0, z = 0; z < resolution.y - 1; z++)
        {
            for (int x = 0; x < resolution.x - 1; x++)
            {
                triangles[vertex + 0] = index + 0;
                triangles[vertex + 1] = index + (resolution.x);
                triangles[vertex + 2] = index + 1;
                triangles[vertex + 3] = index + 1;
                triangles[vertex + 4] = index + (resolution.x);
                triangles[vertex + 5] = index + (resolution.x) + 1;
                vertex += 6;
                index++;
            }
            index++;
        }

        mesh.Clear();
        mesh.vertices = vertices.InternalArray;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.RecalculateBounds();
        mesh.name = "Generated_Surface";
    }
    #endregion
}
