using UnityEngine;

public class MonsterLauncher : MonoBehaviour
{
    public GameObject monsterObj;
    void Start()
    {
        var monster = Instantiate(monsterObj, transform, false);
    }
}
