using System;
using UnityEngine;

public class ImpactBall : AbstractAbilityBall
{
    [Header("Impact Ball Settings")]
    [Tooltip("Number of bounces before the ball is destroyed")]
    [SerializeField] private int maxBounces = 20;

    //[Tooltip("Damage multiplier relative to the base damage")]
    [SerializeField] private float damageMultiplier = 1.0f;

    [Tooltip("Visual or particle effect played on bounce")]
    [SerializeField] private GameObject bounceVFX;

    [Tooltip("Visual or particle effect played when destroyed")]
    [SerializeField] private GameObject destroyVFX;

    private int currentBounceCount = 0;
    private Collider _collider;
    private Rigidbody _rigidbody;

    protected override void Awake()
    {
        BallInit("ImpactBall");
    }

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// 每次与地形或障碍碰撞时触发
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        // 检测是否为地形或墙壁（Layer 可根据项目设定调整）
        if (collision.gameObject.CompareTag("Terrain"))
        {
            currentBounceCount++;
            Debug.Log(currentBounceCount);
            // 播放弹跳特效
            if (bounceVFX != null)
            {
                Instantiate(bounceVFX, collision.contacts[0].point, Quaternion.identity);
            }

            // 若超过最大次数则销毁
            if (currentBounceCount >= maxBounces)
            {
                DestroyBall();
            }
        }

        // 如果碰撞敌人则造成伤害
        if (collision.gameObject.TryGetComponent(out MonsterController monster))
        {
            DamageMonster(monster, damageMultiplier);
        }
    }

    /// <summary>
    /// 造成伤害（倍率）
    /// </summary>
    private void DamageMonster(MonsterController mc, float multiplier)
    {
        float damage = baseDamage * multiplier;
        mc.TakeDamage(Mathf.RoundToInt(damage));
    }

    /// <summary>
    /// 销毁球体逻辑
    /// </summary>
    private void DestroyBall()
    {
        if (destroyVFX != null)
        {
            VFXManager.Instance.PlayVFX(VFXManager.Instance.Electro_hit,transform.position,Quaternion.identity,0.5f);
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// 如果球有主动技能，这里可扩展（本球暂不需）
    /// </summary>
    protected override void Skill()
    {
        // 可扩展主动技能逻辑
    }

    protected override void Update()
    {
        base.Update();
        // 若需要飞行或特效逻辑可在此扩展
    }
}
