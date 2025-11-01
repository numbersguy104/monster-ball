using System.Collections.Generic;
using UnityEngine;

public class SpikeBall : AbstractAbilityBall
{
    [Header("References")]
    [SerializeField] Spike spikePrefab;
    [Header("Ball Settings")]
    [SerializeField] private Vector3 environmentRotation;
    [Tooltip("Spawned skill object count")]
    [SerializeField] private float spawnObjectCount = 20f;
    [Tooltip("Duration of the skill object")]
    [SerializeField] private float spawnObjectDuration = 2.0f;
    [Tooltip("Spawned skill object size ralated to ball")]
    [SerializeField] private float spawnObjectScale = 0.5f;
    [Tooltip("Spawned skill object speed")]
    [SerializeField] private float spawnObjectSpeed = 2.0f;
    [Header("Damage Settings")]
    [Tooltip("Damage multiplier to monsters inside the zone")]
    [SerializeField] private float spawnObjectDamageMultiplier = 0.2f;

    protected override void Skill()
    {
        CreateSkillObjects();
    }

    protected override void Awake()
    {
        BallInit("Spikeball");
    }

    private void CreateSkillObjects()
    {
        for (int i = 0; i < spawnObjectCount; i++)
        {
            var step = 360f / spawnObjectCount;
            var so = Instantiate(spikePrefab, transform.position, transform.rotation);
            float size = transform.lossyScale.x * spawnObjectScale;
            so.transform.localScale = new Vector3(size, size, size);
            so.transform.localRotation = Quaternion.identity;
            so.SetDuration(spawnObjectDuration);

            var forward = Quaternion.Euler(environmentRotation) * Vector3.forward;
            var up = Quaternion.Euler(environmentRotation) * Vector3.up;
            Vector3 dir =  Quaternion.AngleAxis(step * i, up) * forward;
            so.SetVelocity(dir * spawnObjectSpeed);
            so.OnHitMonster += OnSpikeHitMonster;
        }

    }

    private void OnSpikeHitMonster(MonsterController mc)
    {
        DamageMonster(mc, spawnObjectDamageMultiplier);
    }

}
