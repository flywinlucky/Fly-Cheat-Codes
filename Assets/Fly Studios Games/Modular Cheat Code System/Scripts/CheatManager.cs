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
        }

        private void Update()
        {
            // 1. Collect keyboard input
            // Appends any typed characters to the buffer and resets the timeout timer.
            if (!string.IsNullOrEmpty(Input.inputString))
            {
                _inputBuffer.Append(Input.inputString.ToUpper());
                _timer = inputTimeout;
            }

            // 2. Handle input timeout
            // If the timer is running, count down. If it reaches zero, clear the buffer.
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
                if (currentInput.EndsWith(cheat.cheatCode.ToUpper()))
                {
                    // A valid cheat was found, execute its action.
                    cheat.ExecuteAction(gameObject, _spawnCalculator);

                    // --- Firing the Event ---
                    // Notify the rest of the game that a cheat has been activated.
                    // If no scripts are subscribed (e.g., NotificationController is missing), this does nothing.
                    OnCheatActivated?.Invoke();

                    // Clear the buffer and stop the timer to prevent re-activation.
                    _inputBuffer.Clear();
                    _timer = 0;
                    break; // Exit the loop since we found a match.
                }
            }
        }
    }
}
