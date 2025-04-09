using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(GrassData))]
public class GrassDataEditor : Editor   
{
    Button regenButton;

    public override VisualElement CreateInspectorGUI()
    {
        // Get properties
        var grassProperty = serializedObject.FindProperty("grassBlade");
        var amountProperty = serializedObject.FindProperty("amount");
        var edgeOffsetProperty = serializedObject.FindProperty("edgeOffset");

        // Set up visual elements
        var container = new VisualElement();

        var grassHeader = new Label("Grass");
        grassHeader.style.fontSize = 18;

        var pointsHeader = new Label("Points");
        pointsHeader.style.fontSize = 18;

        regenButton = new Button(RegeneratePositions);
        regenButton.text = "Generate New Positions";

        container.Add(grassHeader);
        container.Add(new PropertyField(grassProperty));
        container.Add(pointsHeader);
        container.Add(new PropertyField(amountProperty));
        container.Add(new PropertyField(edgeOffsetProperty));
        container.Add(regenButton);

        // Value tracking
        container.TrackPropertyValue(grassProperty);
        container.TrackPropertyValue(amountProperty, x => regenButton.visible = true);
        container.TrackPropertyValue(edgeOffsetProperty, x => regenButton.visible = true);

        return container;
    }

    private void RegeneratePositions()
    {
        ((GrassData)target).RegeneratePositions();
        regenButton.visible = false;
    }
}
