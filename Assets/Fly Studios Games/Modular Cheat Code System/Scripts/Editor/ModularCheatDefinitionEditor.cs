using UnityEditor;
using UnityEngine;

namespace ModularCheatCodeSystem.Editor
{
    /// <summary>
    /// Custom editor for the ModularCheatDefinition class.
    /// It dynamically shows or hides fields in the Inspector based on the selected CheatActionType.
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

            // --- Draw the default fields that are always visible ---
            EditorGUILayout.PropertyField(cheatCodeProp);
            EditorGUILayout.PropertyField(actionTypeProp);

            // Cast the enum value to our CheatActionType for the switch statement.
            CheatActionType selectedAction = (CheatActionType)actionTypeProp.enumValueIndex;

            // --- Draw conditional fields based on the selected action type ---
            switch (selectedAction)
            {
                case CheatActionType.SpawnObject:
                    EditorGUILayout.PropertyField(prefabToSpawnProp);
                    break;

                case CheatActionType.TriggerEvent:
                    EditorGUILayout.PropertyField(eventIdentifierProp);
                    break;

                case CheatActionType.Both:
                    // Show both fields if 'Both' is selected.
                    EditorGUILayout.PropertyField(prefabToSpawnProp);

                    // Add some spacing for better readability in the inspector.
                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(eventIdentifierProp);
                    break;
            }

            // Always apply the changes at the end of the frame.
            serializedObject.ApplyModifiedProperties();
        }
    }
}
