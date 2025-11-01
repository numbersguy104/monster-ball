using System.Collections.Generic;
using UnityEngine;

public class CrystalBall : AbstractAbilityBall
{
    [Header("References")]
    [SerializeField] CrystalBurst crystalPrefab;
    [Header("Ball Settings")]
    [Tooltip("Spawned skill object duration")]
    [SerializeField] private float zoneDuration = 5.0f;
    [Tooltip("Spawned skill object size ralated to ball")]
    [SerializeField] private float zoneScale = 1.0f;
    [Header("Damage Settings")]
    [Tooltip("Damage frequency to monsters inside the zone")]
    [SerializeField] private float crystalDamageFrequency = 0.5f;
    [Tooltip("Damage multiplier to monsters inside the zone")]
    [SerializeField] private float crystalDamageMultiplier = 0.5f;

    private List<CrystalBurst> activeCrystals = new List<CrystalBurst>();
    private float damageTimer = 0.0f;

    protected override void Skill()
    {
        CreateSkillObject();
        damageTimer = crystalDamageFrequency;
    }

    protected override void Awake()
    {
        BallInit("CrystalBall");
    }

    protected override void Update()
    {
        base.Update();
        //Remove instances from the list if they have Destroyed themselves
        activeCrystals.RemoveAll(x => x == null);

        if (activeCrystals.Count > 0)
        {
            if (damageTimer <= 0.0f)
            {
                damageTimer += crystalDamageFrequency;
                foreach (var c in activeCrystals)
                {
                    List<MonsterController> instanceMonsters = c.GetMonsters();
                    foreach (MonsterController instanceMonster in instanceMonsters)
                    {
                        DamageMonster(instanceMonster, crystalDamageMultiplier);
                    }
                }
            }
            else
            {
                damageTimer -= Time.deltaTime;
            }
        }
    }

    private void CreateSkillObject()
    {
        var trail = Instantiate(crystalPrefab, transform.position, transform.rotation);
        trail.transform.up = GetVelocity();
        float size = transform.lossyScale.x * zoneScale;
        trail.transform.localScale = new Vector3(size, size, size);
        trail.transform.localRotation = Quaternion.identity;
        trail.SetDuration(zoneDuration);
        activeCrystals.Add(trail);
    }

}
