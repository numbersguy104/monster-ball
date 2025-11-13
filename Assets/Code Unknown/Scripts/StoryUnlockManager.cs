//Singleton class to track the player's unlocked story entries

using UnityEngine;

public class StoryUnlockManager : MonoBehaviour
{
    public static StoryUnlockManager Instance;

    [Tooltip("Chance (from 0 to 1) to unlock a story entry when calling MaybeUnlockRandomNew()")]
    [SerializeField] float unlockChance = 0.1f;

    [SerializeField] GameObject unlockPopupPrefab;
    private GameObject currentPopup = null;

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

    //Unlock a given entry
    //Also display the "entry unlocked" popup, unless the second argument is false
    public void Unlock(int entry, bool showPopup = true)
    {
        entryUnlocks[entry] = true;

        if (showPopup)
        {
            if (currentPopup != null)
            {
                Destroy(currentPopup);
            }
            GameObject canvas = GameObject.Find("Canvas");
            currentPopup = Instantiate(unlockPopupPrefab, canvas.transform);
            currentPopup.GetComponent<StoryPopup>().entryIndex = entry;
        }

        //Sound effect should be played somewhere around here if we want one
    }

    //Unlock a random entry the player doesn't already have
    //Also display the "entry unlocked" popup, unless the argument is false
    //Returns the index of the entry unlocked, or null if no entry was unlocked
    //(this happens if the player has every entry already)
    public int? UnlockRandomNew(bool showPopup = true)
    {
        int countLocked = 0;
        foreach (bool unlocked in entryUnlocks) {
            if (!unlocked)
            {
                countLocked++;
            }
        }
        int toUnlock = Random.Range(0, countLocked);
        countLocked = 0;
        int i = 0;
        foreach (bool unlocked in entryUnlocks)
        {
            if (!unlocked)
            {
                if (countLocked == toUnlock)
                {
                    Unlock(i);
                    return i;
                }

                countLocked++;
            }
            i++;
        }

        return null;
    }

    //Randomly either grant an entry the player does not have, or do nothing
    //If the argument is false, do not show a popup if an entry is granted
    //Returns the index of the story entry given, or null if none is granted
    public int? MaybeUnlockRandomNew(bool showPopup = true)
    {
        if (Random.Range(0.0f, 1.0f) < unlockChance)
        {
            return UnlockRandomNew(showPopup);
        }
        return null;
    }

    //Check whether a given entry is unlocked
    public bool IsUnlocked(int entry)
    {
        return entryUnlocks[entry];
    }
}
