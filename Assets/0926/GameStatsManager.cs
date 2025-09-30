using UnityEngine;
using UnityEngine.Events;

public class GameStatsManager : MonoBehaviour
{
    public static GameStatsManager Instance { get; private set; }

    [Header("Player Stats")]
    public long score = 0;         // 总分
    public long gold = 0;          // 总金币（可减少）
    public int killCount = 0;      // 击杀怪物数
    public long totalDamage = 0;   // 总伤害
    public float dps = 0f;         // 每秒伤害（运行时计算）

    [Header("Level Up Settings")]
    public long scoreThreshold = 10000; // 示例：达到 1w 分数触发升级
    public UnityEvent OnLevelUp;        // 升级事件接口（留给外部绑定）

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
        DontDestroyOnLoad(gameObject); // 全局持久化
    }

    void Update()
    {
        // 计算 DPS
        damageTimer += Time.deltaTime;
        if (damageTimer >= 1f)
        {
            dps = damageThisSecond / damageTimer;
            damageTimer = 0f;
            damageThisSecond = 0;
        }

        // 检查升级条件
        if (score >= scoreThreshold)
        {
            OnLevelUp?.Invoke();
            // 可选：调整下一次升级阈值，比如翻倍
            scoreThreshold *= 2;
        }
    }

    // ====== API 接口 ======

    public void AddScore(long amount)
    {
        score += amount;
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
        return false; // 钱不够
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
