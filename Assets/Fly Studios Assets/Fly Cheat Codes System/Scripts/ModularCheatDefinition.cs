using UnityEngine;
using System; // Necesar pentru evenimente (Action)

/// <summary>
/// Definește tipurile de acțiuni pe care le poate executa un cheat.
/// </summary>
public enum CheatActionType
{
    SpawnObject,
    TriggerEvent // Acțiune generică pentru a apela funcții custom
    // Puteți adăuga tipuri mai specifice dacă doriți o configurare mai detaliată
}

[CreateAssetMenu(fileName = "Cheat_", menuName = "Fly Cheats/Modular Cheat Definition")]
public class ModularCheatDefinition : ScriptableObject
{
    [Header("Cheat Configuration")]
    [Tooltip("Codul exact care trebuie tastat de jucător.")]
    public string cheatCode;

    [Tooltip("Tipul de acțiune pe care o va executa acest cheat.")]
    public CheatActionType actionType;

    [Header("Data Payload")]
    [Tooltip("Nume de identificare pentru acțiunile de tip TriggerEvent (ex: 'AddHealth', 'ToggleFlyMode').")]
    public string eventIdentifier; // Un ID unic pentru evenimentul generic

    [Tooltip("Valoare numerică opțională de trimis împreună cu evenimentul (ex: 100 viață).")]
    public int integerValue;

    [Tooltip("Prefab de spawnat (folosit doar dacă Action Type este SpawnObject).")]
    public GameObject prefabToSpawn;

    /// <summary>
    /// Execută acțiunea aleasă.
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
        }
    }

    // --- Implementarea Acțiunii de Spawnare ---
    private void ExecuteSpawnObject(GameObject activator, SmartSpawnCalculator spawnCalculator)
    {
        if (prefabToSpawn == null)
        {
            Debug.LogError($"Cheat '{name}': Nu a fost setat niciun prefab pentru spawnare!");
            return;
        }
        Vector3 safeSpawnPosition = spawnCalculator.GetSafeSpawnPosition();
        Instantiate(prefabToSpawn, safeSpawnPosition, activator.transform.rotation);
    }

    // --- Implementarea Acțiunii de Eveniment Decuplat ---
    private void ExecuteTriggerEvent()
    {
        // Anunțăm sistemul de evenimente că acest cheat specific a fost activat.
        // Trimitem ID-ul evenimentului și valoarea asociată.
        CheatEventManager.RaiseCheatEvent(eventIdentifier, integerValue);
    }
}