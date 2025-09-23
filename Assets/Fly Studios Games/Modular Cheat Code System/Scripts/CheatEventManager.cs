using System;
using UnityEngine;

namespace ModularCheatCodeSystem
{
    /// <summary>
    /// A static class that manages cheat-related events.
    /// This allows for a decoupled system where different components (e.g., PlayerStats, MissionManager)
    /// can react to cheats without needing a direct reference to the CheatManager.
    /// </summary>
    public static class CheatEventManager
    {
        /// <summary>
        /// The main event that user scripts can subscribe to.
        /// It passes a string ID to identify which specific action was triggered.
        /// </summary>
        public static event Action<string> OnCheatTriggered;

        /// <summary>
        /// Method called by ModularCheatDefinition to trigger the event for all listeners.
        /// </summary>
        public static void RaiseCheatEvent(string eventID)
        {
            Debug.Log($"Cheat event triggered: ID='{eventID}'");
            OnCheatTriggered?.Invoke(eventID);
        }
    }
}
