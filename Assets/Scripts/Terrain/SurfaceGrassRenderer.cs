using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Surface))]
public class SurfaceGrassRenderer : MonoBehaviour
{
    // Exposed
    [Header("Grass")]
    [SerializeField]
    Material material;
    [SerializeField]
    GrassData grassData;


    // Serialized
    [SerializeField]
    [HideInInspector]
    Surface surface;
    [SerializeField]
    [HideInInspector]
    RenderParams renderParams;
    [SerializeField]
    [HideInInspector]
    int surfaceMatrixPropertyID;


    // Runtime
    Matrix4x4[] renderMatrices;
    Mesh grassBladeMesh;


    #region Editor
    private void Reset()
    {
        surface = GetComponent<Surface>();
        surfaceMatrixPropertyID = Shader.PropertyToID("_SurfaceWorldToLocal");
    }

    private void OnValidate()
    {
        renderParams = new RenderParams(material);
    }
    #endregion

    #region Runtime
    private void Start()
    {
        surface = GetComponent<Surface>();
        renderParams = new RenderParams(material);
        surfaceMatrixPropertyID = Shader.PropertyToID("_SurfaceWorldToLocal");
        renderMatrices = new Matrix4x4[grassData.Positions.Length];
        grassBladeMesh = grassData.GrassBlade.GenerateMesh();
        UpdateMaterial();
        UpdateRenderMatrices();
        transform.hasChanged = false;
    }

    private void LateUpdate()
    {
        if (grassData == null) return;
        if (material == null) return;
        if (grassData.Positions.Length < 1) return;

        if (renderMatrices == null || renderMatrices.Length != grassData.Positions.Length)
        {
            renderMatrices = new Matrix4x4[grassData.Positions.Length];
            UpdateMaterial();
            UpdateRenderMatrices();
            Debug.Log("Regenerated render matrices");
        }

        if (!Application.isPlaying)
        {
            grassBladeMesh = grassData.GrassBlade.GenerateMesh();
        }
        
        if (transform.hasChanged)
        {
            UpdateMaterial();
            UpdateRenderMatrices();
            transform.hasChanged = false;
        }
        Graphics.RenderMeshInstanced(renderParams, grassBladeMesh, 0, renderMatrices);
    }
    #endregion

    #region Helper Functions
    void UpdateMaterial()
    {
        material.SetMatrix(surfaceMatrixPropertyID, transform.worldToLocalMatrix);
    }

    void UpdateRenderMatrices()
    {
        for (int i = 0; i < grassData.Positions.Length; i++)
        {
            float x = grassData.Positions[i].x;
            float z = grassData.Positions[i].y;
            float y = surface.HeightMap.GetNearestHeight(x, z);
            renderMatrices[i] =
                transform.localToWorldMatrix
                * Matrix4x4.Translate(new Vector3(x, y, z));
        }
    }
    #endregion
}
