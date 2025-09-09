using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth = 100;

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

    private void HandleCheatInput(string eventID, int value)
    {
        // Verifică dacă evenimentul primit este cel relevant pentru acest script.
        if (eventID == "AddHealth")
        {
            AddHealth(value);
        }
        else if (eventID == "SetInvincible")
        {
            Debug.Log($"Set Invicible:");
        }
    }

    public void AddHealth(int amount)
    {
        currentHealth += amount;
        Debug.Log($"Viață adăugată! Viața curentă: {currentHealth}");
    }
}