using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStatsManager : MonoBehaviour
{
    public static GameStatsManager Instance { get; private set; }

    [Header("Player Stats")]
    public long score = 0;         // Total score
    public long gold = 0;          // Total gold (can decrease)
    public int killCount = 0;      // Number of monsters killed
    public long totalDamage = 0;   // Total damage dealt
    public float dps = 0f;         // Damage per second (calculated at runtime)

    /*
     * Level Up Example:
     *  Starting Threshold = 10000
     *  Threshold Multipliers = {1.1, 1.25, 1.5}
     *  Threshold Increase Levels = {5, 10}
     * These values will cause the following behavior:
     *  The first level-up (level 1) will happen at 10000 points.
     *  The next four level-ups (levels 2-5) will multiply that by 1.1 each time:
     *  11000, 12100, 13310, 14641.
     *  After that, the next five level-ups (levels 6-10) will multiply by 1.25:
     *  18301, 22876, 28595, 35743, 44678.
     *  After level 10, the threshold will multiply by 1.5 per level for the rest of the game:
     *  67017, 100525, 150787... (continuing forever)
     */

    [Header("LEVEL UP SETTINGS\nThe level mechanic uses two queues\nto track the amount of points\nneeded to level up for each level.\n\nThreshold Increase Levels is a list of level numbers,\nand Threshold Multipliers is a list of multipliers,\nthe first of which is initially\nset as the \"current multiplier.\"\n\nWhenever the score threshold is reached,\nit is multiplied by the current multiplier.\n\nThe current multiplier increases to its next value\nwhenever the current level reaches one of\nthe values in Threshold Increase Levels.\n\nSee the comments in GameStatsManager.cs\nfor an example.")]
    [SerializeField] long startingThreshold = 10000;
    [SerializeField] List<int> thresholdIncreaseLevels = null;
    [SerializeField] List<float> thresholdMultipliers = null;

    long levelUpThreshold; //Store the amount of points required to level up
    int level = 0;
    
    public UnityEvent OnLevelUp; // Level-up event interface (can be bound externally)

    private float damageTimer = 0f;
    private long damageThisSecond = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist globally
    }

    void Start()
    {
        levelUpThreshold = startingThreshold;

        //Initialize the level-up thresholds to defaults if not set in the inspector
        if (thresholdIncreaseLevels == null || thresholdIncreaseLevels.Count == 0)
        {
            print("Warning: thresholdIncreaseLevels was not initialized! Initializing with no levels. Source: GameStatsManager.cs on object " + gameObject.ToString());
            thresholdIncreaseLevels = new List<int>();
        }
        if (thresholdMultipliers == null || thresholdMultipliers.Count == 0)
        {
            print("Warning: thresholdMultipliers was not initialized! Initializing with a single multiplier of x2 per level. Source: GameStatsManager.cs on object " + gameObject.ToString());
            thresholdMultipliers = new List<float>();
            thresholdMultipliers.Add(2.0f);
        }

        PointsTracker pt = FindAnyObjectByType<PointsTracker>();
        OnLevelUp.AddListener(pt.LevelUp);
        OnLevelUp.AddListener(LevelUp);

        SoundManager.Instance.PlayBGM();
    }

    void Update()
    {
        // Calculate DPS
        damageTimer += Time.deltaTime;
        if (damageTimer >= 1f)
        {
            dps = damageThisSecond / damageTimer;
            damageTimer = 0f;
            damageThisSecond = 0;
        }

        // Check level-up condition
        if (score >= levelUpThreshold)
        {
            OnLevelUp?.Invoke();
        }
    }

    void LevelUp()
    {
        level++;

        //Check if we're at a level where the threshold multiplier changes
        if (thresholdIncreaseLevels.Count > 0 && thresholdIncreaseLevels[0] == level)
        {
            thresholdIncreaseLevels.RemoveAt(0);
            thresholdMultipliers.RemoveAt(0);
        }

        //Increase the score threshold
        levelUpThreshold = (long)(levelUpThreshold * thresholdMultipliers[0]);

        print("DEBUG: Level " + level + " reached. Threshold is " + levelUpThreshold);
    }

    // ====== API Methods ======

    public void AddScore(long amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
        SoundManager.Instance.PlaySFX(SoundManager.Instance.pointAccumulate);
    }

    public void AddGold(long amount)
    {
        gold += amount;
        SoundManager.Instance.PlaySFX(SoundManager.Instance.goldAccumulate);
    }

    public bool SpendGold(long amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            return true;
        }
        return false; // Not enough gold
    }

    public void AddKill()
    {
        killCount++;
    }

    public void AddDamage(long amount)
    {
        totalDamage += amount;
        damageThisSecond += amount;
    }
}
