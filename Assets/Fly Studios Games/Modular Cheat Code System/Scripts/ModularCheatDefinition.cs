using UnityEngine;

namespace ModularCheatCodeSystem
{
    /// <summary>
    /// Defines the types of actions a cheat can perform.
    /// </summary>
    public enum CheatActionType
    {
        SpawnObject,
        TriggerEvent,
        Both // Executes both SpawnObject and TriggerEvent actions.
    }

    /// <summary>
    /// A ScriptableObject that defines a single cheat code and its corresponding action.
    /// Create new cheats by right-clicking in the Project window -> Create -> Modular Cheat System -> Cheat Definition.
    /// </summary>
    [CreateAssetMenu(fileName = "NewCheat", menuName = "Modular Cheat System/Cheat Definition")]
    public class ModularCheatDefinition : ScriptableObject
    {
        [Header("Cheat Configuration")]
        [Tooltip("The exact code the player needs to type.")]
        public string cheatCode;

        [Tooltip("The type of action this cheat will perform.")]
        public CheatActionType actionType;

        // --- Data Payloads ---
        // Note: The visibility of these fields in the Inspector is controlled by ModularCheatDefinitionEditor.cs

        [Tooltip("An identifier for TriggerEvent actions (e.g., 'AddHealth', 'ToggleFlyMode'). This ID is sent to all listeners.")]
        public string eventIdentifier;

        [Tooltip("The prefab to spawn (only used if Action Type is SpawnObject).")]
        public GameObject prefabToSpawn;

        /// <summary>
        /// Executes the chosen action(s) for this cheat.
        /// </summary>
        public void ExecuteAction(GameObject activator, SmartSpawnCalculator spawnCalculator)
        {
            switch (actionType)
            {
                case CheatActionType.SpawnObject:
                    ExecuteSpawnObject(activator, spawnCalculator);
                    break;

                case CheatActionType.TriggerEvent:
                    ExecuteTriggerEvent();
                    break;

                case CheatActionType.Both:
                    // Execute both actions when 'Both' is selected.
                    ExecuteSpawnObject(activator, spawnCalculator);
                    ExecuteTriggerEvent();
                    break;
            }
        }

        // --- SpawnObject Action Implementation ---
        private void ExecuteSpawnObject(GameObject activator, SmartSpawnCalculator spawnCalculator)
        {
            if (prefabToSpawn == null)
            {
                Debug.LogError($"Cheat '{name}': No prefab has been assigned for spawning!");
                return;
            }

            if (spawnCalculator == null)
            {
                Debug.LogError($"Cheat '{name}': The activator is missing a SmartSpawnCalculator component!");
                return;
            }

            Vector3 safeSpawnPosition = spawnCalculator.GetSafeSpawnPosition();
            Instantiate(prefabToSpawn, safeSpawnPosition, activator.transform.rotation);
        }

        // --- TriggerEvent Action Implementation ---
        private void ExecuteTriggerEvent()
        {
            // Check if an identifier is provided before raising the event.
            if (string.IsNullOrEmpty(eventIdentifier))
            {
                Debug.LogWarning($"Cheat '{name}': TriggerEvent action was executed, but the 'eventIdentifier' is empty.");
                return;
            }

            // Notify the event system that this specific cheat has been activated.
            CheatEventManager.RaiseCheatEvent(eventIdentifier);
        }
    }
}

