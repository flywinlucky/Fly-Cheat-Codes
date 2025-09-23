using System.Collections;
using UnityEngine;

namespace ModularCheatCodeSystem
{
    /// <summary>
    /// A simple UI controller that shows a panel for a few seconds when a cheat is activated.
    /// Listens to the CheatManager's OnCheatActivated event.
    /// </summary>
    public class NotificationController : MonoBehaviour
    {
        [Header("UI References")]
        [Tooltip("The main notification panel GameObject that will be enabled/disabled.")]
        public GameObject notificationPanel;

        [Header("Settings")]
        [Tooltip("The duration in seconds that the notification remains visible.")]
        public float displayDuration = 2.5f;

        private void OnEnable()
        {
            // Subscribes to the cheat manager's event when this object becomes active.
            CheatManager.OnCheatActivated += HandleCheatActivation;
        }

        private void OnDisable()
        {
            // Unsubscribes from the event when the object is destroyed or disabled to prevent errors.
            CheatManager.OnCheatActivated -= HandleCheatActivation;
        }

        private void Start()
        {
            // Ensure the notification panel is hidden at the start of the game.
            if (notificationPanel != null)
            {
                notificationPanel.SetActive(false);
            }
        }

        /// <summary>
        /// This function is called automatically by the event from the CheatManager.
        /// </summary>
        private void HandleCheatActivation()
        {
            // Stop any previous notification coroutines to avoid them overlapping or cutting short.
            StopAllCoroutines();
            StartCoroutine(ShowNotificationCoroutine());
        }

        /// <summary>
        /// A coroutine that activates the notification panel and deactivates it after a delay.
        /// </summary>
        private IEnumerator ShowNotificationCoroutine()
        {
            if (notificationPanel != null)
            {
                notificationPanel.SetActive(true);
            }

            yield return new WaitForSeconds(displayDuration);

            if (notificationPanel != null)
            {
                notificationPanel.SetActive(false);
            }
        }
    }
}
