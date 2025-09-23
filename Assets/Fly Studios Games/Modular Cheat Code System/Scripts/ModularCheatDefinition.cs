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
                    ExecuteSpawnObject(activator, spawnCalculator);
                    ExecuteTriggerEvent();
                    break;
            }
        }

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

        private void ExecuteTriggerEvent()
        {
            if (string.IsNullOrEmpty(eventIdentifier))
            {
                Debug.LogWarning($"Cheat '{name}': TriggerEvent action was executed, but the 'eventIdentifier' is empty.");
                return;
            }

            CheatEventManager.RaiseCheatEvent(eventIdentifier);
        }

        /// <summary>
        /// Checks if the cheat definition is configured correctly.
        /// </summary>
        /// <param name="errorMessage">An output message describing the validation error.</param>
        /// <returns>True if the configuration is valid, otherwise false.</returns>
        public bool IsValid(out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(cheatCode))
            {
                errorMessage = $"The cheat code is empty.";
                return false;
            }

            switch (actionType)
            {
                case CheatActionType.SpawnObject:
                    if (prefabToSpawn == null)
                    {
                        errorMessage = $"Action is 'SpawnObject' but no prefab is assigned.";
                        return false;
                    }
                    break;

                case CheatActionType.TriggerEvent:
                    if (string.IsNullOrWhiteSpace(eventIdentifier))
                    {
                        errorMessage = $"Action is 'TriggerEvent' but the identifier is empty.";
                        return false;
                    }
                    break;

                case CheatActionType.Both:
                    if (prefabToSpawn == null)
                    {
                        errorMessage = $"Action is 'Both' but no prefab is assigned.";
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(eventIdentifier))
                    {
                        errorMessage = $"Action is 'Both' but the identifier is empty.";
                        return false;
                    }
                    break;
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}

