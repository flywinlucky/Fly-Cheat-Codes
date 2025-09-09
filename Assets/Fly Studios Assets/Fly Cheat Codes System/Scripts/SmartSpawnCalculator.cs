using UnityEngine;

public class SmartSpawnCalculator : MonoBehaviour
{
    [Header("Spawn Collision Settings")]
    [Tooltip("Distanța maximă la care se încearcă spawnarea obiectului în fața jucătorului.")]
    public float maxSpawnDistance = 5f;

    [Tooltip("Raza obiectului care urmează a fi spawnat. Folosit pentru a preveni coliziunea cu pereții.")]
    public float objectSpawnRadius = 0.5f;

    [Tooltip("LayerMask-ul care definește obstacolele (ex: Pereți, Mediu). Obiectele de pe aceste layere vor fi evitate.")]
    public LayerMask collisionLayerMask;

    [Header("Randomization Settings (GTA Style)")]
    [Tooltip("Introduce o ușoară variație a unghiului de spawnare la stânga/dreapta.")]
    [Range(0f, 45f)]
    public float randomAngleVariance = 10f;

    // Variabile interne pentru Gizmos
    private Vector3 _lastCalculatedSpawnPoint;
    private bool _didHitObstacle;

    /// <summary>
    /// Calculează o poziție de spawn sigură în fața jucătorului, evitând obstacolele.
    /// </summary>
    /// <returns>Poziția sigură calculată în spațiul lumii (world space).</returns>
    public Vector3 GetSafeSpawnPosition()
    {
        // 1. Aplicăm randomizarea stil GTA
        float randomAngle = Random.Range(-randomAngleVariance, randomAngleVariance);
        Quaternion rotationOffset = Quaternion.Euler(0, randomAngle, 0);
        Vector3 spawnDirection = rotationOffset * transform.forward;

        // 2. Folosim SphereCast pentru a detecta coliziunile
        // Începem verificarea puțin deasupra picioarelor jucătorului pentru a evita coliziunea cu solul imediat.
        Vector3 castStartPosition = transform.position + Vector3.up * 0.5f;

        RaycastHit hitInfo;
        if (Physics.SphereCast(castStartPosition, objectSpawnRadius, spawnDirection, out hitInfo, maxSpawnDistance, collisionLayerMask, QueryTriggerInteraction.Ignore))
        {
            // Coliziune detectată. Mutăm punctul de spawn înapoi de la punctul de impact.
            // Scădem raza sferei din distanța loviturii pentru a ne asigura că obiectul încape.
            float safeDistance = hitInfo.distance - objectSpawnRadius * 0.1f; // Mic buffer suplimentar
            _lastCalculatedSpawnPoint = castStartPosition + spawnDirection * Mathf.Max(0, safeDistance);
            _didHitObstacle = true;
        }
        else
        {
            // Nicio coliziune. Spawnăm la distanța maximă dorită.
            _lastCalculatedSpawnPoint = castStartPosition + spawnDirection * maxSpawnDistance;
            _didHitObstacle = false;
        }

        return _lastCalculatedSpawnPoint;
    }

    /// <summary>
    /// Desenează Gizmos în Editor pentru vizualizarea razei de spawn și a punctului final.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // Simulează calculul pentru vizualizare (fără randomizare în Gizmos pentru stabilitate vizuală)
        Vector3 startPoint = transform.position + Vector3.up * 0.5f;
        Vector3 direction = transform.forward;

        RaycastHit hitInfo;
        bool hit = Physics.SphereCast(startPoint, objectSpawnRadius, direction, out hitInfo, maxSpawnDistance, collisionLayerMask, QueryTriggerInteraction.Ignore);

        if (hit)
        {
            // Desenează linia de coliziune (roșu)
            Gizmos.color = Color.red;
            Gizmos.DrawLine(startPoint, hitInfo.point);
            Gizmos.DrawWireSphere(startPoint + direction * hitInfo.distance, objectSpawnRadius);
        }
        else
        {
            // Desenează linia liberă (verde)
            Gizmos.color = Color.green;
            Vector3 endPoint = startPoint + direction * maxSpawnDistance;
            Gizmos.DrawLine(startPoint, endPoint);
            Gizmos.DrawWireSphere(endPoint, objectSpawnRadius);
        }
    }
}