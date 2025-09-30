using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthBar : MonoBehaviour
{
    [Header("UI References")]
    public Image redBar;   // 红色血条（立即下降）
    public Image yellowBar; // 黄色血条（延迟下降）

    [Header("Settings")]
    public float yellowBarSpeed = 0.5f; // 黄条下降速度（越大越快）

    private MonsterController monster;
    private float targetFill = 1f; // 红条目标比例
    private float yellowFill = 1f; // 黄条当前比例

    void Start()
    {
        monster = GetComponentInParent<MonsterController>();
        if (monster == null)
        {
            Debug.LogWarning("MonsterHealthBar: 没找到 MonsterController！");
        }
    }

    void Update()
    {
        if (monster == null) return;

        // 计算血量比例
        float hpRatio = Mathf.Clamp01((float)monster.hp / monster.GetMaxHP());

        // 红条立即更新
        targetFill = hpRatio;
        redBar.fillAmount = targetFill;

        // 黄条缓慢追随
        if (yellowBar.fillAmount > targetFill)
        {
            yellowBar.fillAmount = Mathf.MoveTowards(
                yellowBar.fillAmount,
                targetFill,
                yellowBarSpeed * Time.deltaTime
            );
        }
        else
        {
            yellowBar.fillAmount = targetFill;
        }
    }
}
