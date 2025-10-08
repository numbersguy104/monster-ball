using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//存放所有 MonsterParam 的 ScriptableObject（提供按 ID / 碰撞类型筛选接口）
[CreateAssetMenu(fileName = "MonsterDatabase", menuName = "Monsters/MonsterDatabase", order = 1)]
public class MonsterDatabase : ScriptableObject
{
    public List<MonsterParam> monsters = new List<MonsterParam>();

    // 根据 collision type 筛选
    public List<MonsterParam> GetByCollision(MonsterCollisionType type)
    {
        return monsters.Where(m => m != null && m.collisionType == type).ToList();
    }

    // 如果混合逻辑 - 返回所有
    public List<MonsterParam> GetAll()
    {
        return monsters.Where(m => m != null).ToList();
    }

    public MonsterParam GetByID(string id)
    {
        return monsters.FirstOrDefault(m => m != null && m.monsterID == id);
    }
}
