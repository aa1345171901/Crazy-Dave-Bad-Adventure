using UnityEditor;
using UnityEngine;

namespace TopDownPlate
{
    [CustomEditor(typeof(Trigger2D), true)]
    [InitializeOnLoad]
    public class TriggerEnter2DEditor : Editor
    {
        public Trigger2D TriggerEnter2DTarget
        {
            get
            {
                return (Trigger2D)target;
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (TriggerEnter2DTarget.triggerType == Trigger2D.TriggerType.Tag)
            {
                Editor.DrawPropertiesExcluding(serializedObject, new string[] { "layerMasks" });
            }
            else if (TriggerEnter2DTarget.triggerType == Trigger2D.TriggerType.Layer)
            {
                Editor.DrawPropertiesExcluding(serializedObject, new string[] { "tags" });
            }
            else
            {
                DrawDefaultInspector();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}