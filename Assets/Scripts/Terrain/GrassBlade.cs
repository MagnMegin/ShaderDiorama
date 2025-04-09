using System;
using UnityEngine;

[Serializable]
public struct GrassBlade
{
    [SerializeField]
    float width;
    [SerializeField]
    float height;

    public GrassBlade(float width, float height)
    {
        this.width = width;
        this.height = height;
    }

    public readonly Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[5]
        {
            new Vector3(-0.5f * width, 0f, 0f),
            new Vector3(0.5f * width, 0f, 0f),
            new Vector3(-0.5f * width, 0.3f * height, 0f),
            new Vector3(0.5f * width, 0.3f * height, 0f),
            new Vector3(0f, height, 0f),
        };
        

        Vector3[] normals = new Vector3[5]
        {
            Vector3.back,
            Vector3.back,
            Vector3.back,
            Vector3.back,
            Vector3.back,
        };

        int[] triangles = new int[9] { 0, 2, 1, 2, 3, 1, 2, 4, 3 };

        Vector2[] uvs = new Vector2[5]
        {
            new Vector2(-0.5f * width, 0f),
            new Vector2(0.5f * width, 0f),
            new Vector2(-0.5f * width, 0.3f * height),
            new Vector2(0.5f * width, 0.3f * height),
            new Vector2(0f, height),
        };

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateBounds();
        mesh.name = "Generated grassblade";

        return mesh;
    }
}
