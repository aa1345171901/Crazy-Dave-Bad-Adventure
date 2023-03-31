using UnityEditor;
using UnityEngine;

namespace TopDownPlate
{
    [CustomEditor(typeof(LevelManager))]
    [InitializeOnLoad]
    public class LevelManagerEditor : Editor
    {
        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
        static void DrawGameObjectName(LevelManager levelManager, GizmoType gizmoType)
        {
            GUIStyle style = new GUIStyle();
            Vector3 v3FrontTopLeft;

            if (levelManager.LevelBounds.size != Vector3.zero)
            {
                style.normal.textColor = Color.yellow;
                v3FrontTopLeft = new Vector3(levelManager.LevelBounds.center.x - levelManager.LevelBounds.extents.x, levelManager.LevelBounds.center.y + levelManager.LevelBounds.extents.y + 1, levelManager.LevelBounds.center.z - levelManager.LevelBounds.extents.z);  // Front top left corner
                Handles.Label(v3FrontTopLeft, "Level Bounds", style);
                DrawHandlesBounds(levelManager.LevelBounds, Color.yellow);
            }

            if (levelManager.CameraBounds.size != Vector3.zero)
            {
                style.normal.textColor = Color.red;
                v3FrontTopLeft = new Vector3(levelManager.CameraBounds.center.x - levelManager.CameraBounds.extents.x, levelManager.CameraBounds.center.y + levelManager.CameraBounds.extents.y + 1, levelManager.CameraBounds.center.z - levelManager.CameraBounds.extents.z);  // Front top left corner
                Handles.Label(v3FrontTopLeft, "Camera Bounds", style);
                DrawHandlesBounds(levelManager.CameraBounds, Color.red);
            }
        }

        public static void DrawHandlesBounds(Bounds bounds, Color color)
        {
#if UNITY_EDITOR
            Vector3 boundsCenter = bounds.center;
            Vector3 boundsExtents = bounds.extents;

            Vector3 v3FrontTopLeft = new Vector3(boundsCenter.x - boundsExtents.x, boundsCenter.y + boundsExtents.y, boundsCenter.z - boundsExtents.z);  // Front top left corner
            Vector3 v3FrontTopRight = new Vector3(boundsCenter.x + boundsExtents.x, boundsCenter.y + boundsExtents.y, boundsCenter.z - boundsExtents.z);  // Front top right corner
            Vector3 v3FrontBottomLeft = new Vector3(boundsCenter.x - boundsExtents.x, boundsCenter.y - boundsExtents.y, boundsCenter.z - boundsExtents.z);  // Front bottom left corner
            Vector3 v3FrontBottomRight = new Vector3(boundsCenter.x + boundsExtents.x, boundsCenter.y - boundsExtents.y, boundsCenter.z - boundsExtents.z);  // Front bottom right corner
            Vector3 v3BackTopLeft = new Vector3(boundsCenter.x - boundsExtents.x, boundsCenter.y + boundsExtents.y, boundsCenter.z + boundsExtents.z);  // Back top left corner
            Vector3 v3BackTopRight = new Vector3(boundsCenter.x + boundsExtents.x, boundsCenter.y + boundsExtents.y, boundsCenter.z + boundsExtents.z);  // Back top right corner
            Vector3 v3BackBottomLeft = new Vector3(boundsCenter.x - boundsExtents.x, boundsCenter.y - boundsExtents.y, boundsCenter.z + boundsExtents.z);  // Back bottom left corner
            Vector3 v3BackBottomRight = new Vector3(boundsCenter.x + boundsExtents.x, boundsCenter.y - boundsExtents.y, boundsCenter.z + boundsExtents.z);  // Back bottom right corner


            Handles.color = color;

            Handles.DrawLine(v3FrontTopLeft, v3FrontTopRight);
            Handles.DrawLine(v3FrontTopRight, v3FrontBottomRight);
            Handles.DrawLine(v3FrontBottomRight, v3FrontBottomLeft);
            Handles.DrawLine(v3FrontBottomLeft, v3FrontTopLeft);

            Handles.DrawLine(v3BackTopLeft, v3BackTopRight);
            Handles.DrawLine(v3BackTopRight, v3BackBottomRight);
            Handles.DrawLine(v3BackBottomRight, v3BackBottomLeft);
            Handles.DrawLine(v3BackBottomLeft, v3BackTopLeft);

            Handles.DrawLine(v3FrontTopLeft, v3BackTopLeft);
            Handles.DrawLine(v3FrontTopRight, v3BackTopRight);
            Handles.DrawLine(v3FrontBottomRight, v3BackBottomRight);
            Handles.DrawLine(v3FrontBottomLeft, v3BackBottomLeft);
#endif
        }
    }
}