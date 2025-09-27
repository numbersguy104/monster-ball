using UnityEngine;
//单个怪物数据（ScriptableObject 元素数据结构）
public enum MonsterCollisionType { WithCollision, WithoutCollision }

[CreateAssetMenu(fileName = "MonsterParam", menuName = "Monsters/MonsterParam", order = 0)]
public class MonsterParam : ScriptableObject
{
    public string monsterID; // 唯一 ID
    public GameObject prefab; // 对应预制体（包含 MonsterController）
    public MonsterCollisionType collisionType;

    public MovementType movementType;
    public float speed;
    public float length;
    public float radius;

    [Header("Stats")]
    public int hp = 10;
    public int point = 0;
    public int gold = 0;
}
