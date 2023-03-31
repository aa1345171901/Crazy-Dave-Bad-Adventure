using UnityEditor;
using UnityEngine;

namespace TopDownPlate
{
    [CustomPropertyDrawer(typeof(ConditionHiden))]
    public class ConditionHidenDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ConditionHiden conditionHiden = (ConditionHiden)attribute;
            bool enabled = GetConditionAttributeResult(conditionHiden, property);
            bool previouslyEnabled = GUI.enabled;
            GUI.enabled = enabled;
            if (!conditionHiden.Hidden || enabled)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
            GUI.enabled = previouslyEnabled;
        }

        private bool GetConditionAttributeResult(ConditionHiden conditionHiden, SerializedProperty property)
        {
            bool enabled = true;
            string propertyPath = property.propertyPath;
            string conditionPath = propertyPath.Replace(property.name, conditionHiden.ConditionString);
            SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

            if (sourcePropertyValue != null)
            {
                enabled = sourcePropertyValue.boolValue;
            }
            else
            {
                Debug.LogWarning("No matching found for ConditionHiden Attribute in object: " + conditionHiden.ConditionString);
            }

            return enabled;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ConditionHiden conditionHiden = (ConditionHiden)attribute;
            bool enabled = GetConditionAttributeResult(conditionHiden, property);

            if (!conditionHiden.Hidden || enabled)
            {
                return EditorGUI.GetPropertyHeight(property, label);
            }
            else
            {
                return -EditorGUIUtility.standardVerticalSpacing;
            }
        }
    }
}
