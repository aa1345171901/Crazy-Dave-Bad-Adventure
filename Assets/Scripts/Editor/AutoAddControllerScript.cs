using UnityEditor;
using UnityEngine;

namespace TopDownPlate
{
    [CustomEditor(typeof(Character))]
    [CanEditMultipleObjects]
    public class AutoAddControllerScript : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Autobuild", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("添加所需的依赖脚本如rigidbody,collider,controller,CharacterAbility等", MessageType.Warning, true);
            if (GUILayout.Button("AutoBuild Character"))
            {
                GenerateCharacter();
            }

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void GenerateCharacter()
        {
            Character character = (Character)target;

            Debug.LogFormat(character.name + " : Character Autobuild Start");

            // Adds the rigidbody2D
            Rigidbody2D rigidbody2D = (character.GetComponent<Rigidbody2D>() == null) ? character.gameObject.AddComponent<Rigidbody2D>() : character.GetComponent<Rigidbody2D>();
            rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rigidbody2D.useAutoMass = false;
            rigidbody2D.mass = 1;
            rigidbody2D.drag = 0;
            rigidbody2D.angularDrag = 0.05f;
            rigidbody2D.gravityScale = 1;

            // Adds the boxcollider 2D
            BoxCollider2D boxcollider2D = (character.GetComponent<BoxCollider2D>() == null) ? character.gameObject.AddComponent<BoxCollider2D>() : character.GetComponent<BoxCollider2D>();

            // Adds the Controller
            CharacterController controller = (character.GetComponent<CharacterController>() == null) ? character.gameObject.AddComponent<CharacterController>() : character.GetComponent<CharacterController>();

            if (character.CharacterType == CharacterTypes.Player)
            {
                if (character.GetComponent<CharacterMovement>() == null) { character.gameObject.AddComponent<CharacterMovement>(); }
            }
            else
            {
                if (character.GetComponent<AIMove>() == null) { character.gameObject.AddComponent<AIMove>(); }
            }
            if (character.GetComponent<Health>() == null) { character.gameObject.AddComponent<Health>(); }

            Debug.LogFormat(character.name + " : Character Autobuild Complete");
        }
    }
}
