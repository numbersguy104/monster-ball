using System.Collections.Generic;
using UnityEngine;

public class NuclearBall : AbstractAbilityBall
{
    [Header("References")]
    [SerializeField] NuclearArea nuclearPrefab;
    [Header("Ball Settings")]
    [SerializeField] private Vector3 environmentRotation;
    [Tooltip("Spawned nuclear object count inside radius, every spawn will be in the same size")]
    [SerializeField] private int areaCount = 3;
    [Tooltip("Spawned skill object size ralated to ball")]
    [SerializeField] private float zoneScale = 1.0f;
    [Tooltip("Spawned skill object duration")]
    [SerializeField] private float zoneDuration = 5.0f;
    [Tooltip("Range radius")]
    [SerializeField] private float nuclearAreaRangeRadius = 1.0f;
    [Header("Damage Settings")]
    [Tooltip("Damage frequency to monsters inside the zone")]
    [SerializeField] private float nuclearDamageFrequency = 0.5f;
    [Tooltip("Damage multiplier to monsters inside the zone")]
    [SerializeField] private float nuclearDamageMultiplier = 0.5f;

    private List<NuclearArea> activeNuclearAreas = new List<NuclearArea>();
    private float damageTimer = 0.0f;

    protected override void Skill()
    {
        CreateSkillObjects();
    }

    protected override void Awake()
    {
        BallInit("Nuclearball");
    }

    protected override void Update()
    {
        base.Update();
        //Remove instances from the list if they have Destroyed themselves
        activeNuclearAreas.RemoveAll(x => x == null);

        if (activeNuclearAreas.Count > 0)
        {
            if (damageTimer <= 0.0f)
            {
                damageTimer += nuclearDamageFrequency;
                foreach (var c in activeNuclearAreas)
                {
                    List<MonsterController> instanceMonsters = c.GetMonsters();
                    foreach (MonsterController instanceMonster in instanceMonsters)
                    {
                        DamageMonster(instanceMonster, nuclearDamageMultiplier);
                    }
                }
            }
            else
            {
                damageTimer -= Time.deltaTime;
            }
        }
    }

    private void CreateSkillObjects()
    {
        // Get the tilted plane rotation
        Quaternion planeRotation = Quaternion.Euler(environmentRotation);

        // Get the size of a single nuclear area (assuming it's uniform)
        float areaRadius = nuclearPrefab.transform.localScale.x * 0.5f;

        for (int i = 0; i < areaCount; i++)
        {
            Vector3 spawnPosition = FindValidSpawnPosition(planeRotation, areaRadius);

            if (spawnPosition != Vector3.zero)
            {
                // Instantiate the nuclear area
                NuclearArea area = Instantiate(nuclearPrefab, spawnPosition, Quaternion.identity);

                // Align the area to the tilted plane
                area.transform.up = planeRotation * Vector3.up;
                float size = transform.lossyScale.x * zoneScale;
                area.transform.localScale = new Vector3(size, size, size);
                // Set the duration
                area.SetDuration(zoneDuration);

                // Add to active list
                activeNuclearAreas.Add(area);
            }
        }
    }

    private Vector3 FindValidSpawnPosition(Quaternion planeRotation, float areaRadius)
    {
        int maxAttempts = 30; // Limit attempts to avoid infinite loop

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            // Generate random position within the nuclear area range on the tilted plane
            Vector2 randomCircle = Random.insideUnitCircle * nuclearAreaRangeRadius;

            // Convert 2D circle position to 3D position on the tilted plane
            Vector3 localOffset = new Vector3(randomCircle.x, 0, randomCircle.y);
            Vector3 worldOffset = planeRotation * localOffset;
            Vector3 candidatePosition = transform.position + worldOffset;

            // Cast ray from ball position towards candidate position
            Vector3 rayDirection = (candidatePosition - transform.position).normalized;
            float rayDistance = Vector3.Distance(transform.position, candidatePosition);

            // Check if there's a Default layer obstacle in the way
            if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, rayDistance, LayerMask.GetMask("Default")))
            {
                // Hit an obstacle before reaching candidate position, try another position
                continue;
            }

            return candidatePosition;
        }

        // If no valid position found after max attempts, return current position as fallback
        return transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the nuclear area range radius
        Quaternion planeRotation = Quaternion.Euler(environmentRotation);

        // Draw the outer circle (nuclear area range)
        Gizmos.color = Color.red;
        DrawCircleOnPlane(transform.position, nuclearAreaRangeRadius, planeRotation, 64);

        // Draw the small area size preview if prefab exists
        if (activeNuclearAreas.Count > 0)
        {
            foreach (NuclearArea area in activeNuclearAreas)
            {
                if (area == null)
                    continue;

                Gizmos.color = Color.yellow;
                float areaRadius = area.transform.localScale.x * 0.5f;
                DrawCircleOnPlane(area.transform.position, areaRadius, planeRotation, 32);
            }
        }
    }

    private void DrawCircleOnPlane(Vector3 center, float radius, Quaternion planeRotation, int segments)
    {
        Vector3 previousPoint = Vector3.zero;

        for (int i = 0; i <= segments; i++)
        {
            float angle = (i / (float)segments) * Mathf.PI * 2;
            Vector3 localPoint = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            Vector3 worldPoint = center + planeRotation * localPoint;

            if (i > 0)
            {
                Gizmos.DrawLine(previousPoint, worldPoint);
            }

            previousPoint = worldPoint;
        }
    }


}
