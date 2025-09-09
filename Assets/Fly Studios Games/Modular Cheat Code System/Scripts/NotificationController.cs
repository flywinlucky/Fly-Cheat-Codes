using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Dacă folosești Text, Image etc.

namespace ModularCheatCodeSystem
{
    public class NotificationController : MonoBehaviour
    {
        [Header("UI References")]
        [Tooltip("Obiectul principal al notificării care va fi activat/dezactivat.")]
        public GameObject notificationPanel;

        [Tooltip("Timpul în secunde cât notificarea rămâne vizibilă.")]
        public float displayDuration = 2.5f;

        // Optional: Text pentru a afișa ce cheat a fost activat
        // public Text notificationText; 

        private void OnEnable()
        {
            // Se abonează la evenimentul managerului de cheat-uri când acest obiect devine activ.
            CheatManager.OnCheatActivated += HandleCheatActivation;
        }

        private void OnDisable()
        {
            // Se dezabonează de la eveniment când obiectul este distrus sau dezactivat.
            CheatManager.OnCheatActivated -= HandleCheatActivation;
        }

        private void Start()
        {
            // Asigură-te că notificarea este ascunsă la începutul jocului.
            if (notificationPanel != null)
            {
                notificationPanel.SetActive(false);
            }
        }

        /// <summary>
        /// Această funcție este chemată automat de evenimentul din CheatManager.
        /// </summary>
        private void HandleCheatActivation()
        {
            // Oprește orice corutină de notificare anterioară pentru a evita suprapunerile.
            StopAllCoroutines();
            StartCoroutine(ShowNotificationCoroutine());
        }

        private IEnumerator ShowNotificationCoroutine()
        {
            Debug.Log("Notification UI: Cheat detected, showing notification.");

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