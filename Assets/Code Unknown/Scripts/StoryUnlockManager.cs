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
        //TODO: Check number of entries from a data table, instead of just using 10
        entryUnlocks = new bool[10];
        for (int i = 0; i < 10; i++)
        {
            entryUnlocks[i] = false;
        }

        //DEBUG: Unlock some entries for testing
        entryUnlocks[0] = true;
        entryUnlocks[1] = true;
        entryUnlocks[3] = true;
        entryUnlocks[6] = true;
        entryUnlocks[7] = true;
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
