using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//������� MonsterParam �� ScriptableObject���ṩ�� ID / ��ײ����ɸѡ�ӿڣ�
[CreateAssetMenu(fileName = "MonsterDatabase", menuName = "Monsters/MonsterDatabase", order = 1)]
public class MonsterDatabase : ScriptableObject
{
    public List<MonsterParam> monsters = new List<MonsterParam>();

    // ���� collision type ɸѡ
    public List<MonsterParam> GetByCollision(MonsterCollisionType type)
    {
        return monsters.Where(m => m != null && m.collisionType == type).ToList();
    }

    // �������߼� - ��������
    public List<MonsterParam> GetAll()
    {
        return monsters.Where(m => m != null).ToList();
    }

    public MonsterParam GetByID(string id)
    {
        return monsters.FirstOrDefault(m => m != null && m.monsterID == id);
    }
}
