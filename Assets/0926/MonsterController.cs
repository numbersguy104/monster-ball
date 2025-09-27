using UnityEngine;

// 单个怪物运行时脚本（血量、掉落、碰撞设置、移动行为挂载点）
public class MonsterController : MonoBehaviour
{
    [Header("Data")]
    public string monsterID;
    public int hp;
    public int point;
    public int gold;
    public MonsterCollisionType collisionType;

    private CollisionSelector collisionSelector;
    private MonsterMovement movement;

    void Awake()
    {
        collisionSelector = GetComponent<CollisionSelector>();
        movement = GetComponent<MonsterMovement>();
    }

    public void InitializeFromParam(MonsterParam param)
    {
        if (param == null) return;
        monsterID = param.monsterID;
        hp = param.hp;
        point = param.point;
        gold = param.gold;
        collisionType = param.collisionType;

        // Set collide
        if (collisionSelector != null)
            collisionSelector.SetCollisionType(collisionType);
    }

    /// <summary>
    /// 由 Spawner 调用，设置运动参数
    /// </summary>
    /*public void SetMovement(MovementType type, float speed, float lengthOrRadius, Vector3 spawnCenter)
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
*/
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
    public void Die()
    {
        // TODO: add death VFX/logic, notify spawn manager that this spawn point is free
        Destroy(gameObject);
    }
}
