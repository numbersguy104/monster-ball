using cfg;
using UnityEngine;

public class MonsterLauncher : MonoBehaviour
{
    public GameObject monsterObj;
    void Start()
    {
        var monster = Instantiate(monsterObj, transform, false);
        // TbPerson ps = LubanTablesMgr.Instance.tables.TbPerson;
        // var age = ps["Jin"].Age;
        // Debug.Log(age);
    }
}
