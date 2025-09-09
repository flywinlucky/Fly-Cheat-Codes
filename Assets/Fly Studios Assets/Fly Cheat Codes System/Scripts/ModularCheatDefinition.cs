using UnityEngine;

[CreateAssetMenu(fileName = "Cheat_", menuName = "Fly Cheats/Spawn Cheat Definition")]
public class ModularCheatDefinition : ScriptableObject
{
    [Header("Cheat Configuration")]
    [Tooltip("Codul exact care trebuie tastat de jucător.")]
    public string cheatCode;

    [Header("Spawn Settings")]
    [Tooltip("Prefab-ul care va fi instanțiat la activarea codului.")]
    public GameObject prefabToSpawn;

    // Am eliminat 'spawnOffsetForward' de aici, deoarece logica este acum în SmartSpawnCalculator.

    /// <summary>
    /// Execută acțiunea de spawnare folosind calculatorul inteligent.
    /// </summary>
    /// <param name="activator">Obiectul jucătorului care a activat cheat-ul.</param>
    /// <param name="spawnCalculator">Referința la calculatorul de spawn de pe jucător.</param>
    public void ExecuteAction(GameObject activator, SmartSpawnCalculator spawnCalculator)
    {
        if (prefabToSpawn == null)
        {
            Debug.LogError($"Cheat '{name}': Nu a fost setat niciun prefab pentru spawnare!");
            return;
        }

        if (spawnCalculator == null)
        {
            Debug.LogError($"Cheat '{name}': Nu s-a găsit SmartSpawnCalculator pe jucător!");
            // Fallback la spawnare simplă dacă lipsește calculatorul
            Instantiate(prefabToSpawn, activator.transform.position + activator.transform.forward * 2f, activator.transform.rotation);
            return;
        }

        // Obținem poziția sigură de la noul script
        Vector3 safeSpawnPosition = spawnCalculator.GetSafeSpawnPosition();

        Instantiate(prefabToSpawn, safeSpawnPosition, activator.transform.rotation);
        Debug.Log($"Cheat activat: {prefabToSpawn.name} a fost spawnat la poziția sigură.");
    }
}