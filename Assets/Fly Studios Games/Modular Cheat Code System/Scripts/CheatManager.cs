using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ModularCheatCodeSystem
{
    /// <summary>
    /// The core component that listens for player input and activates cheats.
    /// This should be placed on a persistent object in your scene, like the player or a game manager.
    /// </summary>
    [RequireComponent(typeof(SmartSpawnCalculator))]
    public class CheatManager : MonoBehaviour
    {
        // --- Public Static Event ---
        // Any other script can subscribe to this event without needing a direct reference to the CheatManager.
        // Primarily used for triggering general UI feedback, like a notification panel.
        public static event Action OnCheatActivated;

        [Header("Configuration")]
        [Tooltip("The list of all cheat codes available in the game.")]
        public List<ModularCheatDefinition> availableCheats;

        [Tooltip("The time in seconds before the input buffer is automatically cleared.")]
        public float inputTimeout = 1.5f;

        // --- Internal References ---
        private readonly StringBuilder _inputBuffer = new StringBuilder();
        private float _timer;
        private SmartSpawnCalculator _spawnCalculator;

        private void Start()
        {
            _spawnCalculator = GetComponent<SmartSpawnCalculator>();
            ValidateAllCheatsOnStart();
        }

        private void Update()
        {
            // 1. Collect keyboard input
            if (!string.IsNullOrEmpty(Input.inputString))
            {
                // Append only alphanumeric characters to avoid issues with control keys.
                foreach (char c in Input.inputString)
                {
                    if (char.IsLetterOrDigit(c))
                    {
                        _inputBuffer.Append(char.ToUpper(c));
                        _timer = inputTimeout; // Reset the timer on new input
                    }
                }
            }

            // 2. Handle input timeout
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0)
                {
                    _inputBuffer.Clear();
                }
            }

            // 3. Check if the current buffer matches any cheat codes.
            CheckBufferForCheats();
        }

        /// <summary>
        /// Iterates through the available cheats and checks if the current input buffer ends with a valid cheat code.
        /// </summary>
        private void CheckBufferForCheats()
        {
            if (_inputBuffer.Length == 0) return;

            string currentInput = _inputBuffer.ToString();

            foreach (ModularCheatDefinition cheat in availableCheats)
            {
                // --- VALIDATION 1: Skip any null entries in the list ---
                if (cheat == null || string.IsNullOrEmpty(cheat.cheatCode))
                {
                    continue;
                }

                if (currentInput.EndsWith(cheat.cheatCode.ToUpper()))
                {
                    // --- VALIDATION 2: Check if the cheat is correctly configured before executing ---
                    string validationError;
                    if (cheat.IsValid(out validationError))
                    {
                        // A valid cheat was found, execute its action.
                        cheat.ExecuteAction(gameObject, _spawnCalculator);
                        OnCheatActivated?.Invoke();
                    }
                    else
                    {
                        // Log a warning if the cheat is typed but invalid, for easier debugging.
                        Debug.LogWarning($"Cheat '{cheat.cheatCode}' was activated but is not configured correctly: {validationError}");
                    }

                    // Clear the buffer and stop the timer to prevent re-activation.
                    _inputBuffer.Clear();
                    _timer = 0;
                    break; // Exit the loop since we found a match.
                }
            }
        }

        /// <summary>
        /// Checks all configured cheats at the start of the game and logs warnings for any invalid ones.
        /// </summary>
        private void ValidateAllCheatsOnStart()
        {
            Debug.Log("Validating all available cheat configurations...");
            for (int i = 0; i < availableCheats.Count; i++)
            {
                var cheat = availableCheats[i];
                if (cheat == null)
                {
                    Debug.LogWarning($"Found a null (empty) entry in the 'Available Cheats' list at index {i}.");
                    continue;
                }

                string validationError;
                if (!cheat.IsValid(out validationError))
                {
                    Debug.LogWarning($"Configuration issue in '{cheat.name}': {validationError}");
                }
            }
        }
    }
}

