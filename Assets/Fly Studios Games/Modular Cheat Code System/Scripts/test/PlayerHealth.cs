using UnityEngine;

namespace ModularCheatCodeSystem
{
    public class PlayerHealth : MonoBehaviour
    {
        private void OnEnable()
        {
            // Se abonează la sistemul de cheat-uri.
            CheatEventManager.OnCheatTriggered += HandleCheatInput;
        }

        private void OnDisable()
        {
            // Se dezabonează pentru a preveni erorile.
            CheatEventManager.OnCheatTriggered -= HandleCheatInput;
        }

        private void HandleCheatInput(string eventID)
        {
            // Verifică dacă evenimentul primit este cel relevant pentru acest script.
            if (eventID == "healt")
            {
                ExecuteAndShowLog("healt");
            }
            else if (eventID == "invincible")
            {
                ExecuteAndShowLog("invincible");
            }
            else if (eventID == "coins")
            {
                ExecuteAndShowLog("coins");
            }
        }

        public void ExecuteAndShowLog(string name)
        {
            Debug.Log($"Event Trigered : {name}");
        }
    }
}