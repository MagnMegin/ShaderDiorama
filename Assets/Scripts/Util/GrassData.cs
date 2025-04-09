using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/GrassData")]
public class GrassData : ScriptableObject
{
    [SerializeField]
    private uint amount = 0;
    [SerializeField]
    [Range(0f, 0.5f)]
    private float edgeOffset = 0f;
    [SerializeField]
    private GrassBlade grassBlade = new (0.2f, 1f);

    [SerializeField]
    [HideInInspector]
    private Vector2[] positions;

    public Vector2[] Positions => positions;
    public GrassBlade GrassBlade => grassBlade;

    private void Awake()
    {
        RegeneratePositions();
    }

    public void RegeneratePositions()
    { 
        positions = new Vector2[amount];
        for (int i = 0; i < amount; i++)
        {
            positions[i] = new Vector2(
                x: Random.Range(edgeOffset, 1f - edgeOffset),
                y: Random.Range(edgeOffset, 1f - edgeOffset));
        }
    }
}
