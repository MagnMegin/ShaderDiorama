using System;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class HeightMap
{
    public delegate float HeightFunc(float x, float z);

    [SerializeField]
    [HideInInspector]
    Vector2Int resolution;
    [SerializeField]
    [HideInInspector]
    Array2D<float> heightValues;

    public Vector2Int Resolution => resolution;

    public HeightMap(Vector2Int resolution, HeightFunc heightFunc)
    {
        this.resolution = resolution;
        
        int longestSide = Mathf.Max(resolution.x, resolution.y);

        heightValues = new Array2D<float>(resolution);
        for (int z = 0; z < resolution.y; z++)
        {
            for (int x = 0; x < resolution.x; x++)
            {
                float pixelHeight = heightFunc(x / ((float)longestSide - 1), z / ((float)longestSide - 1));
                heightValues[x,z] = pixelHeight;
            }
        }
    }

    public HeightMap(int width, int height, HeightFunc heightFunc)
    {
        resolution = new Vector2Int(width, height);
        
        int longestSide = Mathf.Max(width, height);

        heightValues = new Array2D<float>(resolution);
        for (int z = 0; z < resolution.y; z++)
        {
            for (int x = 0; x < resolution.x; x++)
            {
                float pixelHeight = heightFunc(x / ((float)longestSide - 1), z / ((float)longestSide - 1));
                heightValues[x, z] = pixelHeight;
            }
        }
    }

    public void Regenerate(Vector2Int resolution, HeightFunc heightFunc)
    {
        this.resolution = resolution;

        int longestSide = Mathf.Max(resolution.x, resolution.y);

        heightValues = new Array2D<float>(resolution);
        for (int z = 0; z < resolution.y; z++)
        {
            for (int x = 0; x < resolution.x; x++)
            {
                float pixelHeight = heightFunc(x / ((float)longestSide - 1), z / ((float)longestSide - 1));
                heightValues[x, z] = pixelHeight;
            }
        }
    }

    public void Regenerate(int width, int height, HeightFunc heightFunc)
    {
        resolution = new Vector2Int(width, height);

        int longestSide = Mathf.Max(width, height);

        heightValues = new Array2D<float>(resolution);
        for (int z = 0; z < resolution.y; z++)
        {
            for (int x = 0; x < resolution.x; x++)
            {
                float pixelHeight = heightFunc(x / ((float)longestSide - 1), z / ((float)longestSide - 1));
                heightValues[x, z] = pixelHeight;
            }
        }
    }

    public float HeightFromPixel(int x, int z)
    {
        return heightValues[x, z];
    }

    /// <summary>
    /// Coordinates are expected to be between 0 and 1.
    /// </summary>
    public float GetNearestHeight(float x, float z)
    {
        // Pixel space coordinates
        int xPixel = Mathf.RoundToInt((resolution.x - 1) * x);
        int zPixel = Mathf.RoundToInt((resolution.y - 1) * z);

        return heightValues[xPixel, zPixel];
    }
}
