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

        Vector3[] vertices = new Vector3[9]
        {
            new Vector3(-0.5f * width, 0f, 0f),
            new Vector3(0.5f * width, 0f, 0f),
            new Vector3(-0.5f * width, 0.25f * height, 0f),
            new Vector3(0.5f * width, 0.25f * height, 0f),
            new Vector3(-0.3f * width, 0.5f * height, 0f),
            new Vector3(0.3f * width, 0.5f * height, 0f),
            new Vector3(-0.25f * width, 0.75f * height, 0f),
            new Vector3(0.25f * width, 0.75f * height, 0f),
            new Vector3(0f, height, 0f),
        };
        

        Vector3[] normals = new Vector3[9]
        {
            Vector3.back,
            Vector3.back,
            Vector3.back,
            Vector3.back,
            Vector3.back,
            Vector3.back,
            Vector3.back,
            Vector3.back,
            Vector3.back,
        };

        int[] triangles = new int[21] { 0, 2, 1, 2, 3, 1, 2, 4, 3, 3, 4 ,5, 4, 6, 5, 5, 6, 7, 6, 8, 7 };

        Vector2[] uvs = new Vector2[9]
        {
            new Vector2(-0.5f * width, 0f),
            new Vector2(0.5f * width, 0f),
            new Vector2(-0.5f * width, 0.25f * height),
            new Vector2(0.5f * width, 0.25f * height),
            new Vector2(-0.3f * width, 0.5f * height),
            new Vector2(0.3f * width, 0.5f * height),
            new Vector2(-0.25f * width, 0.75f * height),
            new Vector2(0.25f * width, 0.75f * height),
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
