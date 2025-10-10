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

    [Header("Level Up Settings")]

    //Queue of point thresholds for the player to reach
    [Tooltip("Amonunts of points that will increase various stats and allow an upgrade when reached")]
    [SerializeField] List<long> scoreThresholds = null;

    public long scoreThreshold; //Store the current threshold
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
        //Initialize the score thresholds to a default if not set in the inspector
        if (scoreThresholds == null || scoreThresholds.Count == 0)
        {
            print("Warning: Point threshold queue (scoreThresholds) was not initialized! Initializing with a single multiplier of 10000. Source: GameStatsManager.cs on object " + gameObject.ToString());
            scoreThresholds = new List<long>();
            scoreThresholds.Add(10000L);
        }

        PointsTracker pt = FindAnyObjectByType<PointsTracker>();
        OnLevelUp.AddListener(pt.LevelUp);
        OnLevelUp.AddListener(LevelUp);

        LevelUp();
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
        if (score >= scoreThreshold)
        {
            OnLevelUp?.Invoke();
        }
    }

    void LevelUp()
    {
        //Move on to the next threshold
        if (scoreThresholds.Count > 0)
        {
            scoreThreshold = scoreThresholds[0];
            scoreThresholds.RemoveAt(0);
        }
        else
        {
            scoreThreshold = long.MaxValue;
        }
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
