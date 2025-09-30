using UnityEngine;

// 单个怪物运行时脚本（血量、掉落、碰撞设置、移动行为挂载点）
public class MonsterController : MonoBehaviour
{
    [Header("Data")]
    public string monsterID;
    public int hp;
    public int point;
    public int gold;
    public int maxHP;
    public MonsterCollisionType collisionType;

    private CollisionSelector collisionSelector;
    private MonsterMovement movement;

    void Awake()
    {
        collisionSelector = GetComponent<CollisionSelector>();
        movement = GetComponent<MonsterMovement>();
    }

    public int GetMaxHP()
    {
        // 如果需要更灵活，可以在 MonsterParam 里加一个 maxHp 字段
        return hp <= 0 ? 1 : hp;
    }


    public void InitializeFromParam(MonsterParam param)
    {
        if (param == null) return;
        monsterID = param.monsterID;
        hp = param.hp;
        maxHP = param.hp;
        point = param.point;
        gold = param.gold;
        collisionType = param.collisionType;

        // Set collide
        if (collisionSelector != null)
            collisionSelector.SetCollisionType(collisionType);
    }
    public void SetMovement(MovementType type, float speed, float lengthOrRadius, Vector3 spawnCenter)
    {
        if (movement == null) return;

        movement.movementType = type;
        movement.speed = speed;
        if (type == MovementType.Horizontal || type == MovementType.Vertical)
            movement.length = lengthOrRadius;
        else if (type == MovementType.Circular)
            movement.radius = lengthOrRadius;

        movement.SetSpawnCenter(spawnCenter); // 这里传入 spawnCenter
    }


    /// <summary>
    /// 由 Spawner 调用，显式指定锚点（避免和 prefab 自身位置冲突）
    /// </summary>
    public void SetSpawnCenter(Vector3 pos)
    {
        if (movement != null)
            movement.SetSpawnCenter(pos);
    }

    // Placeholder: on defeat, notify spawner system (future)
    public void TakeDamage(int baseDamage, float acceleration)
    {
        // 计算伤害
        int finalDamage = Mathf.RoundToInt(baseDamage * acceleration);

        // 扣血
        hp -= finalDamage;

        // 更新全局统计
        GameStatsManager.Instance.AddDamage(finalDamage);
        GameStatsManager.Instance.AddScore(point / 10); // 可选：受伤时给少量分数，击杀时再加大分

        // 判断死亡
        if (hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // TODO: add death VFX/logic, notify spawn manager that this spawn point is free
        GameStatsManager.Instance.AddGold(gold);
        GameStatsManager.Instance.AddScore(point);
        GameStatsManager.Instance.AddKill();
        //test
        GameStatsManager.Instance.AddScore(100);
        //todo: die effect
        Destroy(gameObject);
    }
}
