using System;
using UnityEngine;

public static class CheatEventManager
{
    /// <summary>
    /// Evenimentul principal la care se abonează scripturile utilizatorului.
    /// Trimite un ID de string (ce acțiune?) și o valoare int (cât?).
    /// </summary>
    public static event Action<string, int> OnCheatTriggered;

    /// <summary>
    /// Metodă chemată de ModularCheatDefinition pentru a declanșa evenimentul.
    /// </summary>
    public static void RaiseCheatEvent(string eventID, int value)
    {
        Debug.Log($"Eveniment de cheat declanșat: ID='{eventID}', Valoare={value}");
        OnCheatTriggered?.Invoke(eventID, value);
    }
}