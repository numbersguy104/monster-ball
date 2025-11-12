//Singleton class to track the player's unlocked story entries

using UnityEngine;

public class StoryUnlockManager : MonoBehaviour
{
    public static StoryUnlockManager Instance;

    bool[] entryUnlocks;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        int count = LubanTablesMgr.Instance.tables.TbStoryParam.DataList.Count;
        entryUnlocks = new bool[count];
        for (int i = 0; i < count; i++)
        {
            entryUnlocks[i] = false;
        }
    }

    //Mark a given log entry as unlocked
    public void Unlock(int entry)
    {
        entryUnlocks[entry] = true;
    }

    //Check whether a given entry is unlocked
    public bool IsUnlocked(int entry)
    {
        return entryUnlocks[entry];
    }
}
