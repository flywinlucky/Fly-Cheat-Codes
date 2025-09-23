using UnityEditor;
using UnityEngine;

namespace ModularCheatCodeSystem.Editor
{
    /// <summary>
    /// Custom editor for the ModularCheatDefinition class.
    /// It dynamically shows or hides fields in the Inspector based on the selected CheatActionType
    /// and provides helpful tips and warnings to the user.
    /// </summary>
    [CustomEditor(typeof(ModularCheatDefinition))]
    public class ModularCheatDefinitionEditor : UnityEditor.Editor
    {
        // --- Serialized Properties ---
        private SerializedProperty cheatCodeProp;
        private SerializedProperty actionTypeProp;
        private SerializedProperty eventIdentifierProp;
        private SerializedProperty prefabToSpawnProp;

        private void OnEnable()
        {
            // Link the serialized properties to the fields in ModularCheatDefinition.
            cheatCodeProp = serializedObject.FindProperty("cheatCode");
            actionTypeProp = serializedObject.FindProperty("actionType");
            eventIdentifierProp = serializedObject.FindProperty("eventIdentifier");
            prefabToSpawnProp = serializedObject.FindProperty("prefabToSpawn");
        }

        public override void OnInspectorGUI()
        {
            // Always update the serialized object at the beginning of the frame.
            serializedObject.Update();

            // --- Draw the cheat code field with help text and warnings ---
            EditorGUILayout.HelpBox("Define the exact code the player needs to type. It is not case-sensitive.", MessageType.Info);
            EditorGUILayout.PropertyField(cheatCodeProp, new GUIContent("Cheat Code"));
            if (string.IsNullOrEmpty(cheatCodeProp.stringValue))
            {
                EditorGUILayout.HelpBox("The Cheat Code cannot be empty!", MessageType.Warning);
            }

            EditorGUILayout.Space(10); // Add some vertical space

            // --- Draw the action type field ---
            EditorGUILayout.PropertyField(actionTypeProp, new GUIContent("Action Type"));

            CheatActionType selectedAction = (CheatActionType)actionTypeProp.enumValueIndex;

            EditorGUILayout.Space();

            // --- Draw conditional fields with their own warnings ---
            switch (selectedAction)
            {
                case CheatActionType.SpawnObject:
                    EditorGUILayout.LabelField("Spawn Action Settings", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(prefabToSpawnProp);
                    if (prefabToSpawnProp.objectReferenceValue == null)
                    {
                        EditorGUILayout.HelpBox("The 'Prefab to Spawn' has not been assigned. This action will fail.", MessageType.Warning);
                    }
                    break;

                case CheatActionType.TriggerEvent:
                    EditorGUILayout.LabelField("Event Action Settings", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(eventIdentifierProp);
                    if (string.IsNullOrEmpty(eventIdentifierProp.stringValue))
                    {
                        EditorGUILayout.HelpBox("The 'Event Identifier' is empty. Listening scripts may not be able to identify this event.", MessageType.Warning);
                    }
                    break;

                case CheatActionType.Both:
                    EditorGUILayout.LabelField("Combined Action Settings", EditorStyles.boldLabel);

                    // Spawn Object Section
                    EditorGUILayout.PropertyField(prefabToSpawnProp);
                    if (prefabToSpawnProp.objectReferenceValue == null)
                    {
                        EditorGUILayout.HelpBox("The 'Prefab to Spawn' has not been assigned.", MessageType.Warning);
                    }

                    EditorGUILayout.Space();

                    // Trigger Event Section
                    EditorGUILayout.PropertyField(eventIdentifierProp);
                    if (string.IsNullOrEmpty(eventIdentifierProp.stringValue))
                    {
                        EditorGUILayout.HelpBox("The 'Event Identifier' is empty.", MessageType.Warning);
                    }
                    break;
            }

            // Always apply the changes at the end of the frame.
            serializedObject.ApplyModifiedProperties();
        }
    }
}

