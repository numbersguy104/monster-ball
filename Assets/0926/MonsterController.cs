using UnityEngine;
//单个怪物运行时脚本（血量、掉落、碰撞设置、移动行为挂载点）
public class MonsterController : MonoBehaviour
{
    [Header("Data")]
    public string monsterID;
    public int hp;
    public int point;
    public int gold;
    public MonsterCollisionType collisionType;

    CollisionSelector collisionSelector;
    MonsterMovement movement;

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

    // called by spawner to set movement params
    public void SetMovement(MovementType type, float speed, float lengthOrRadius)
    {
        if (movement == null) return;
        movement.movementType = type;
        movement.speed = speed;
        if (type == MovementType.Horizontal || type == MovementType.Vertical)
            movement.length = lengthOrRadius;
        else if (type == MovementType.Circular)
            movement.radius = lengthOrRadius;

        movement.SetSpawnCenter(transform.position);
    }

    // Placeholder: on defeat, notify spawner system (future)
    public void Die()
    {
        // TODO: add death VFX/logic, notify spawn manager that this spawn point is free
        Destroy(gameObject);
    }
}
