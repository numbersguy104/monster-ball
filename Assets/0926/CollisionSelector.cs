using UnityEngine;
//辅助组件：怪物预制体上用于在 Spawn 时启用正确碰撞体
public class CollisionSelector : MonoBehaviour
{
    // 你可以把两个Collider分别挂到这两个字段
    public Collider withCollision;      // 正常碰撞体（例如 BoxCollider）
    public Collider withoutCollision;   // 不碰撞（可以用 trigger 或者直接禁用）

    // 如果 monster prefab 包含多个 collider，可以在这里扩展为数组并按规则启/停
    public void SetCollisionType(MonsterCollisionType type)
    {
        if (withCollision != null) withCollision.enabled = (type == MonsterCollisionType.WithCollision);
        if (withoutCollision != null) withoutCollision.enabled = (type == MonsterCollisionType.WithoutCollision);
    }
}
