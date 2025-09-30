using UnityEngine;
using UnityEngine.Events;

public class GameStatsManager : MonoBehaviour
{
    public static GameStatsManager Instance { get; private set; }

    [Header("Player Stats")]
    public long score = 0;         // �ܷ�
    public long gold = 0;          // �ܽ�ң��ɼ��٣�
    public int killCount = 0;      // ��ɱ������
    public long totalDamage = 0;   // ���˺�
    public float dps = 0f;         // ÿ���˺�������ʱ���㣩

    [Header("Level Up Settings")]
    public long scoreThreshold = 10000; // ʾ�����ﵽ 1w ������������
    public UnityEvent OnLevelUp;        // �����¼��ӿڣ������ⲿ�󶨣�

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
        DontDestroyOnLoad(gameObject); // ȫ�ֳ־û�
    }

    void Update()
    {
        // ���� DPS
        damageTimer += Time.deltaTime;
        if (damageTimer >= 1f)
        {
            dps = damageThisSecond / damageTimer;
            damageTimer = 0f;
            damageThisSecond = 0;
        }

        // �����������
        if (score >= scoreThreshold)
        {
            OnLevelUp?.Invoke();
            // ��ѡ��������һ��������ֵ�����緭��
            scoreThreshold *= 2;
        }
    }

    // ====== API �ӿ� ======

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
        return false; // Ǯ����
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
