using System;
using UnityEngine;


[Serializable]
public struct SurfaceFunction
{
    enum Type
    {
        Perlin,
        Logistic,
    }

    [SerializeField]
    Type type;
    [SerializeField]
    Vector2 offset;
    [SerializeField]
    float scale;
    [SerializeField]
    float strength;

    public readonly float Evaluate(float x, float y)
    {
        if (type == Type.Logistic)
        {
            x -= 0.5f;
            y -= 0.5f;
        }

        x = (x + offset.x) * scale;
        y = (y + offset.y) * scale;

        return type switch
        {
            Type.Perlin => Mathf.PerlinNoise(x, y) * strength,
            Type.Logistic => Mathf.Exp(-(x*x + y*y)) * strength,
            _ => 0f,
        };
    }
}


