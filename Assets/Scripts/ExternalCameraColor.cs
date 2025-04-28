using System.Runtime.InteropServices;
using UnityEngine;

public class ExternalCameraColor : MonoBehaviour
{
    private void Awake()
    {
        string backgroundColor = WebBackgroundColor();
        Debug.Log("Setting camera color to be " + backgroundColor);
        if (TryParseWebColor(backgroundColor, out Color32 color))
        {
            Camera camera = GetComponent<Camera>();
            camera.backgroundColor = color;
        }
    }

    private bool TryParseWebColor(string htmlColor, out Color32 color)
    {
        color = Color.black;

        htmlColor = htmlColor.Trim('r', 'g', 'b', 'a', '(', ')');

        string[] stringValues = htmlColor.Split(',');

        int[] values = new int[stringValues.Length];
        for (int i = 0; i < stringValues.Length; i++)
        {
            if (!int.TryParse(stringValues[i], out values[i])) return false;
        }

        string debug = string.Empty;
        foreach (int value in values) { debug += value + " "; }

        if (values.Length == 3 )
        {
            color = new((byte)values[0], (byte)values[1], (byte)values[2], 255);
            return true;
        }
        else if (values.Length == 4 )
        {
            color = new((byte)values[0], (byte)values[1], (byte)values[2], (byte)values[3]);
            return true;
        }
        else
        {
            return false;
        }

    }

    [DllImport("__Internal")]
    private static extern string GetWebBackgroundColor();


    private static string WebBackgroundColor()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer) return GetWebBackgroundColor();
        return string.Empty;
    }
}
