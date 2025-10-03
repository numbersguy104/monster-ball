using UnityEngine;

// Single monster data (ScriptableObject element data structure)
public enum MonsterCollisionType { WithCollision, WithoutCollision }

[CreateAssetMenu(fileName = "MonsterParam", menuName = "Monsters/MonsterParam", order = 0)]
public class MonsterParam : ScriptableObject
{
    public string monsterID;       // Unique ID
    public GameObject prefab;      // Corresponding prefab (includes MonsterController)
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
