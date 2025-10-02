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
    public long scoreThreshold = 10000; // Example: level up when reaching 10,000 points
    public UnityEvent OnLevelUp;        // Level-up event interface (can be bound externally)

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
            // Optional: adjust the next level-up threshold, e.g., double it
            scoreThreshold *= 2;
        }
    }

    // ====== API Methods ======

    public void AddScore(long amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
    }

    public void AddGold(long amount)
    {
        gold += amount;
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
