using UnityEngine;

public class LightningBall : AbstractAbilityBall
{
    [Header("References")]
    [SerializeField] LightningStrike strikePrefab;
    [SerializeField] GameObject duringSkillIndicator;
    [SerializeField] LayerMask enemyLayer;

    [Header("Ball Settings")]
    [Tooltip("Skill duration")]
    [SerializeField] private float skillDuration = 10.0f;
    [Tooltip("Skill range")]
    [SerializeField] private float skillRangeRadius = 2.0f;
    [Tooltip("Scale of the summon, relative to the ball")]
    [SerializeField] private float summonScale = 1.0f;
    [Tooltip("Time in seconds between strikes when ability is active")]
    [SerializeField] private const float attackInterval = 1f;

    //How long the ball's ability is currently active for, in seconds
    private float durationTimer = 0.0f;
    //Tracks time for the attackInterval
    private float attakIntervalTimer = 0.0f;

    //Cache for Physics.OverlapSphereNonAlloc
    Collider[] hits;

    protected override void Skill()
    {
        durationTimer = skillDuration;
        if (durationTimer > 0f)
        {
            duringSkillIndicator.SetActive(true);
        }
    }

    protected override void Awake()
    {
        BallInit("Lightningball");
        duringSkillIndicator.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();

        if (durationTimer > 0.0f)
        {
            durationTimer -= Time.deltaTime;
            if (durationTimer <= 0)
            {
                durationTimer = 0;
                duringSkillIndicator.SetActive(false);
            }

            if (attakIntervalTimer <= 0.0f)
            {
                var successful = TryAttackMonsters();
                if (successful)
                    attakIntervalTimer += attackInterval;
            }
            else
            {
                attakIntervalTimer -= Time.deltaTime;
            }
        }
    }

    private bool TryAttackMonsters()
    {
        var result = false;
        //if enemy layer is specifically set, use OverlapSphereNonAlloc for better performance
        var hits = Physics.OverlapSphere(transform.position, skillRangeRadius, enemyLayer);
        foreach (var col in hits)
        {
            if(col != null && col.TryGetComponent(out MonsterController mc))
            {
                SummonStrikeAtMonster(mc);
                result = true;
            }
        }
        return result;
    }

    private void SummonStrikeAtMonster(MonsterController mc)
    {
        var s = Instantiate(strikePrefab, mc.transform.position, transform.rotation);
        var size = transform.lossyScale.x * summonScale;
        s.transform.localScale = new Vector3(size, size, size);
        s.transform.localRotation = Quaternion.identity;
        s.transform.position = mc.transform.position + new Vector3(0, 0.5f, 0f);
        s.SetDuration(0.5f);
        DamageMonster(mc);
    }

}
