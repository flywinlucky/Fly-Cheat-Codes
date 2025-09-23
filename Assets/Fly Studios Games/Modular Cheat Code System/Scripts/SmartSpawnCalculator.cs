using UnityEngine;

namespace ModularCheatCodeSystem
{
    /// <summary>
    /// A helper component that calculates a safe position in front of an object (e.g., the player)
    /// to spawn another object, avoiding immediate collisions with the environment.
    /// </summary>
    public class SmartSpawnCalculator : MonoBehaviour
    {
        [Header("Spawn Collision Settings")]
        [Tooltip("The maximum distance in front of the player to attempt spawning the object.")]
        public float maxSpawnDistance = 5f;

        [Tooltip("The radius of the object to be spawned. Used for the collision check to prevent clipping into walls.")]
        public float objectSpawnRadius = 0.5f;

        [Tooltip("The LayerMask that defines obstacles (e.g., Walls, Environment). Objects on these layers will be avoided.")]
        public LayerMask collisionLayerMask;

        [Header("Randomization Settings (GTA Style)")]
        [Tooltip("Introduces a slight random variance to the spawn angle (left/right).")]
        [Range(0f, 45f)]
        public float randomAngleVariance = 10f;

        /// <summary>
        /// Calculates a safe spawn position in front of the player, avoiding obstacles.
        /// </summary>
        /// <returns>The calculated safe position in world space.</returns>
        public Vector3 GetSafeSpawnPosition()
        {
            // 1. Apply GTA-style randomization to the direction
            float randomAngle = Random.Range(-randomAngleVariance, randomAngleVariance);
            Quaternion rotationOffset = Quaternion.Euler(0, randomAngle, 0);
            Vector3 spawnDirection = rotationOffset * transform.forward;

            // 2. Use a SphereCast to detect collisions
            // We start the cast slightly above the object's origin to avoid an immediate collision with the ground.
            Vector3 castStartPosition = transform.position + Vector3.up * 0.5f;
            Vector3 finalSpawnPoint;

            RaycastHit hitInfo;
            if (Physics.SphereCast(castStartPosition, objectSpawnRadius, spawnDirection, out hitInfo, maxSpawnDistance, collisionLayerMask, QueryTriggerInteraction.Ignore))
            {
                // Collision detected. We move the spawn point back from the impact point.
                // We subtract a small buffer from the hit distance to ensure the object fits.
                float safeDistance = hitInfo.distance - objectSpawnRadius * 0.1f; // Small additional buffer
                finalSpawnPoint = castStartPosition + spawnDirection * Mathf.Max(0, safeDistance);
            }
            else
            {
                // No collision. We can spawn at the desired maximum distance.
                finalSpawnPoint = castStartPosition + spawnDirection * maxSpawnDistance;
            }

            return finalSpawnPoint;
        }

        /// <summary>
        /// Draws Gizmos in the Editor to visualize the spawn check and the resulting point.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            // Simulate the calculation for visualization (without randomization in Gizmos for visual stability)
            Vector3 startPoint = transform.position + Vector3.up * 0.5f;
            Vector3 direction = transform.forward;

            RaycastHit hitInfo;
            bool hit = Physics.SphereCast(startPoint, objectSpawnRadius, direction, out hitInfo, maxSpawnDistance, collisionLayerMask, QueryTriggerInteraction.Ignore);

            if (hit)
            {
                // Draw the collision line (red) and the final sphere position
                Gizmos.color = Color.red;
                Gizmos.DrawLine(startPoint, hitInfo.point);
                Gizmos.DrawWireSphere(startPoint + direction * hitInfo.distance, objectSpawnRadius);
            }
            else
            {
                // Draw the free path line (green) and the final sphere position
                Gizmos.color = Color.green;
                Vector3 endPoint = startPoint + direction * maxSpawnDistance;
                Gizmos.DrawLine(startPoint, endPoint);
                Gizmos.DrawWireSphere(endPoint, objectSpawnRadius);
            }
        }
    }
}
