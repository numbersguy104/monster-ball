using UnityEngine;
//�����������ݣ�ScriptableObject Ԫ�����ݽṹ��
public enum MonsterCollisionType { WithCollision, WithoutCollision }

[CreateAssetMenu(fileName = "MonsterParam", menuName = "Monsters/MonsterParam", order = 0)]
public class MonsterParam : ScriptableObject
{
    public string monsterID; // Ψһ ID
    public GameObject prefab; // ��ӦԤ���壨���� MonsterController��
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
