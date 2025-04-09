using System;
using UnityEngine;

[Serializable]
public struct Array2D<T>
{
    [SerializeField]
    [HideInInspector]
    Vector2Int size;
    [SerializeField]
    [HideInInspector]
    T[] values;

    public readonly T[] InternalArray => values;

    public Array2D(int width, int height)
    {
        if (width < 0 || height < 0)
        {
            Debug.LogWarning("Cannot create Array2D with negative size.");
            width = 0;
            height = 0;
        }

        size = new Vector2Int(width, height);
        values = new T[width * height];
    }

    public Array2D(Vector2Int size)
    {
        if (size.x < 0 || size.y < 0)
        {
            Debug.LogWarning("Cannot create Array2D with negative size.");
            size = Vector2Int.zero;
        }

        this.size = size;
        values = new T[size.x * size.y];
    }

    public T this[int x, int y]
    {
        get => values[x + y * size.x];
        set => values[x + y * size.x] = value;
    }

    public T this[Vector2Int coord]
    {
        get => values[coord.x + coord.y * size.x];
        set => values[coord.x + coord.y * size.x] = value;
    }

    public readonly void Clear()
    {
        Array.Clear(values, 0, values.Length);
    }
}
