using UnityEngine;
using UnityEngine.UI; // Required for interacting with UI elements like Text.

namespace ModularCheatCodeSystem
{
    /// <summary>
    /// Manages the player's core statistics such as health, armor, and coins.
    /// This component is responsible for modifying these values and updating the UI display.
    /// Designed to be a modular and easy-to-integrate system for any game.
    /// </summary>
    public class PlayerStats : MonoBehaviour
    {
        [Header("UI References")]
        [Tooltip("The Text element that displays the player's health.")]
        public Text healthText;
        [Tooltip("The Text element that displays the player's armor.")]
        public Text armorText;
        [Tooltip("The Text element that displays the player's coin count.")]
        public Text coinsText;

        [Header("Player Attributes")]
        [Tooltip("The current health of the player.")]
        public int health = 15;
        [Tooltip("The current armor of the player.")]
        public int armor = 10;
        [Tooltip("The current number of coins the player has.")]
        public int coins = 80;

        [Header("Cheat Settings")]
        [Tooltip("The amount of health to add when the health cheat is triggered.")]
        public int healthCheatAmount = 50;
        [Tooltip("The amount of armor to add when the armor cheat is triggered.")]
        public int armorCheatAmount = 25;
        [Tooltip("The number of coins to add when the coins cheat is triggered.")]
        public int coinsCheatAmount = 100;

        #region Unity Lifecycle Methods

        private void Start()
        {
            // Update the UI with the initial values when the game starts.
            UpdateStatsUI();
        }

        private void OnEnable()
        {
            // Subscribe to the cheat system's event manager to listen for cheat codes.
            CheatEventManager.OnCheatTriggered += HandleCheatInput;
        }

        private void OnDisable()
        {
            // Unsubscribe to prevent memory leaks and errors when the object is disabled or destroyed.
            CheatEventManager.OnCheatTriggered -= HandleCheatInput;
        }

        #endregion

        #region Cheat Handling

        /// <summary>
        /// This method is called by the CheatEventManager whenever a cheat code is successfully entered.
        /// </summary>
        /// <param name="eventID">The unique identifier for the triggered cheat.</param>
        private void HandleCheatInput(string eventID)
        {
            // Check which cheat was triggered and call the corresponding function.
            // Note: The 'eventID' string must exactly match the ID set in your Cheat Code Manager.
            if (eventID == "health")
            {
                AddHealth(healthCheatAmount);
                Debug.Log($"Cheat Activated: Added {healthCheatAmount} health.");
            }
            else if (eventID == "armor")
            {
                AddArmor(armorCheatAmount);
                Debug.Log($"Cheat Activated: Added {armorCheatAmount} armor.");
            }
            else if (eventID == "coins")
            {
                AddCoins(coinsCheatAmount);
                Debug.Log($"Cheat Activated: Added {coinsCheatAmount} coins.");
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Increases the player's health by a specified amount. There is no upper limit.
        /// </summary>
        /// <param name="amount">The amount of health to add.</param>
        public void AddHealth(int amount)
        {
            health += amount;
            UpdateStatsUI();
        }

        /// <summary>
        /// Increases the player's armor by a specified amount. There is no upper limit.
        /// </summary>
        /// <param name="amount">The amount of armor to add.</param>
        public void AddArmor(int amount)
        {
            armor += amount;
            UpdateStatsUI();
        }

        /// <summary>
        /// Increases the player's coin count by a specified amount.
        /// </summary>
        /// <param name="amount">The number of coins to add.</param>
        public void AddCoins(int amount)
        {
            coins += amount;
            UpdateStatsUI();
        }

        #endregion

        #region UI Management

        /// <summary>
        /// Updates all assigned UI Text elements with the current stat values.
        /// </summary>
        private void UpdateStatsUI()
        {
            // Check if the healthText reference is assigned before trying to update it.
            if (healthText != null)
            {
                // Display health with a prefix and a percentage sign.
                healthText.text = $"Health : {health}%";
            }

            // Check if the armorText reference is assigned.
            if (armorText != null)
            {
                // Display armor with a prefix and a percentage sign.
                armorText.text = $"Armor : {armor}%";
            }

            // Check if the coinsText reference is assigned.
            if (coinsText != null)
            {
                // Display coins with a prefix.
                coinsText.text = $"Coins : {coins}";
            }
        }

        #endregion
    }
}

