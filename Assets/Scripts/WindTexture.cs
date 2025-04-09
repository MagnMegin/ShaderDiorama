using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[ExecuteAlways]
public class WindTexture : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    float diffusion = 0f;
    [SerializeField]
    [Range(0f, 0.1f)]
    float decay = 0f;
    [SerializeField]
    [Range(0f, 1f)]
    float persitence;


    Texture2D tex;
    Array2D<Vector2> paintBuffer;

    
    // Runtime
    private void Start()
    {
        ResetTexture();
        InitializeBuffer();
    }

    private void FixedUpdate()
    {
        for (int x = 0; x < tex.width; x++)
        {
            for (int y = 0; y < tex.height; y++)
            {
                var sampleVector =
                    Decode01Vector(tex.GetPixel(x, y)) * (1-diffusion) +
                    Decode01Vector(tex.GetPixel(Mathf.Clamp(x + 1, 0, tex.width - 1), y)) * diffusion / 4 +
                    Decode01Vector(tex.GetPixel(Mathf.Clamp(x - 1, 0, tex.width - 1), y)) * diffusion / 4 +
                    Decode01Vector(tex.GetPixel(x, Mathf.Clamp(y + 1, 0, tex.width - 1))) * diffusion / 4 +
                    Decode01Vector(tex.GetPixel(x, Mathf.Clamp(y - 1, 0, tex.width - 1))) * diffusion / 4;
                Vector2 vec = Vector2.ClampMagnitude(paintBuffer[x, y] + sampleVector * (1f - decay), 1f);
                paintBuffer[x, y] = vec;
            }
        }

        for (int x = 0; x < tex.width; x++)
        {
            for (int y = 0; y < tex.height; y++)
            {
                tex.SetPixel(x, y, Encode01Vector(paintBuffer[x, y]));
                paintBuffer[x, y] = paintBuffer[x, y] * persitence;
            }
        }
        tex.Apply();
    }


    // Helper methods
    private void ResetTexture()
    {
        tex = new Texture2D(16, 16, TextureFormat.RGBAFloat, mipChain: false);
        tex.wrapMode = TextureWrapMode.Clamp;
        Color[] colors = new Color[16 * 16];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.gray;
        }
        tex.SetPixels(colors);
        tex.Apply();
        Shader.SetGlobalTexture("_WindTexture", tex);
    }

    private void InitializeBuffer()
    {
        paintBuffer = new Array2D<Vector2>(tex.width, tex.height);
    }

    private Color Encode01Vector(Vector2 vec)
    {
        vec = (vec + Vector2.one) * 0.5f;
        return new Color(vec.x, vec.y, 0.5f, 0.5f);
    }

    private Vector2 Decode01Vector(Color color)
    {
        Vector2 vec = new Vector2(color.r, color.g);
        return vec * 2f - Vector2.one;
    }


    // Public interface
    public void PaintVector(Vector2 uv, Vector2 vector)
    {
        Brush brush = Brush.Small();
        vector = Vector2.ClampMagnitude(vector, 1f);

        for (int i = 0; i < brush.positions.Length; i++)
        {
            Vector2Int pos = new(
                Mathf.RoundToInt(uv.x * (tex.width - 1)),
                Mathf.RoundToInt(uv.y * (tex.height - 1)));
            pos += brush.positions[i];

            bool posOutOfBounds =
                pos.x < 0 || 15 < pos.x ||
                pos.y < 0 || 15 < pos.y;
            if (posOutOfBounds) continue;

            paintBuffer[pos.x, pos.y] = Vector2.Lerp(Vector2.zero, vector, brush.values[i]);
        }
    }
}
