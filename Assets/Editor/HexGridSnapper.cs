using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class HexGridSnapper
{
    private static bool isSnappingEnabled = true; // Snapping is enabled by default

    static HexGridSnapper()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    // Toggle snapping via menu item
    [MenuItem("Tools/Hex Grid Snapping/Toggle Snapping %#h")] // Ctrl + Shift + H
    public static void ToggleSnapping()
    {
        isSnappingEnabled = !isSnappingEnabled;
        Debug.Log("Hex Grid Snapping " + (isSnappingEnabled ? "Enabled" : "Disabled"));
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        if (!isSnappingEnabled)
            return;

        Event e = Event.current;

        // Check if an object is being moved
        if (e.type == EventType.MouseUp && e.button == 0 && Selection.activeTransform != null)
        {
            foreach (var transform in Selection.transforms)
            {
                SnapTransformToHexGrid(transform);
            }
        }
    }

    private static void SnapTransformToHexGrid(Transform transform)
    {
        // Replace '1f' with your hexagon side length if different
        float a = 6.5f; // Side length of your hexagon
        float size = a;

        // Calculate hex dimensions
        float width = 2f * size;
        float height = Mathf.Sqrt(3f) * size;

        Vector3 position = transform.position;

        // Convert world position to hex grid coordinates
        float q = (2f / 3f * position.x) / size;
        float r = (-1f / 3f * position.x + Mathf.Sqrt(3f) / 3f * position.z) / size;

        // Round to nearest hex grid coordinate
        int qInt = Mathf.RoundToInt(q);
        int rInt = Mathf.RoundToInt(r);

        // Convert hex grid coordinates back to world position
        Vector3 snappedPosition = HexToWorldPosition(qInt, rInt, size);

        // Record the change for undo functionality
        Undo.RecordObject(transform, "Snap to Hex Grid");
        transform.position = snappedPosition;
    }

    private static Vector3 HexToWorldPosition(int q, int r, float size)
    {
        float x = size * (3f / 2f) * q;
        float z = size * Mathf.Sqrt(3f) * (r + q / 2f);
        return new Vector3(x, 0f, z);
    }
}
