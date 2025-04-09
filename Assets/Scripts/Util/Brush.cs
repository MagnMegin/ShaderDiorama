using UnityEngine;

public readonly struct Brush
{
    public readonly Vector2Int[] positions;
    public readonly float[] values;


    public Brush(Vector2Int[] positions, float[] values)
    {
        this.positions = positions;
        this.values = values;
    }

    public static Brush Small()
    {
        Vector2Int[] positions = new Vector2Int[9]
        {
            Vector2Int.up + Vector2Int.left,
            Vector2Int.up,
            Vector2Int.up + Vector2Int.right,
            Vector2Int.left,
            Vector2Int.zero,
            Vector2Int.right,
            Vector2Int.down + Vector2Int.left,
            Vector2Int.down,
            Vector2Int.down + Vector2Int.right,
        };

        float[] values = new float[9]
        {
            0.25f,
            0.5f,
            0.25f,
            0.5f,
            1f,
            0.5f,
            0.25f,
            0.5f,
            0.25f,
        };

        return new Brush(positions, values);
    }
}
