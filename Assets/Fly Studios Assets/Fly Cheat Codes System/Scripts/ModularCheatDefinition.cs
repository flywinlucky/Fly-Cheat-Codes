using UnityEngine;

// Am eliminat enum-ul CheatActionType pentru simplitate maximă.
// Fiecare asset creat cu acest script va avea doar acțiunea de spawnare.

[CreateAssetMenu(fileName = "Cheat_", menuName = "Fly Cheats/Spawn Cheat Definition")]
public class ModularCheatDefinition : ScriptableObject
{
    [Header("Cheat Configuration")]
    [Tooltip("Codul exact care trebuie tastat de jucător (nu este sensibil la majuscule).")]
    public string cheatCode;

    [Header("Spawn Settings")]
    [Tooltip("Prefab-ul care va fi instanțiat la activarea codului.")]
    public GameObject prefabToSpawn;

    [Tooltip("Distanța față de jucător unde va apărea obiectul.")]
    public float spawnOffsetForward = 3f;

    /// <summary>
    /// Execută acțiunea de spawnare.
    /// </summary>
    /// <param name="activator">Obiectul jucătorului care a activat cheat-ul.</param>
    public void ExecuteAction(GameObject activator)
    {
        if (prefabToSpawn == null)
        {
            Debug.LogError($"Cheat '{name}': Nu a fost setat niciun prefab pentru spawnare!");
            return;
        }

        // Calculează poziția de spawn în fața jucătorului.
        Vector3 spawnPosition = activator.transform.position + activator.transform.forward * spawnOffsetForward;

        Instantiate(prefabToSpawn, spawnPosition, activator.transform.rotation);
        Debug.Log($"Cheat activat: {prefabToSpawn.name} a fost spawnat.");
    }
}