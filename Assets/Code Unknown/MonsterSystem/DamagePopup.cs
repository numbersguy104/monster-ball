using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    // ======= 可在 Inspector 调整 =======
    [Header("References")]
    public TextMeshProUGUI damageText;
    public CanvasGroup canvasGroup;

    [Header("Move Settings")]
    public Vector2 moveSpeed = new Vector2(0f, 80f);

    [Header("Lifetime")]
    public float lifeTime = 1f;

    [Header("Crit Style")]
    public Color normalColor = Color.white;
    public Color critColor = Color.yellow;
    public float critScaleMultiplier = 1.4f;

    // 内部变量
    private float timer;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        // 往上移动
        rectTransform.anchoredPosition += moveSpeed * Time.deltaTime;

        // 倒计时渐隐
        timer += Time.deltaTime;
        canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / lifeTime);

        // 销毁
        if (timer >= lifeTime)
            Destroy(gameObject);
    }

    /// <summary>
    /// 初始化伤害弹字
    /// </summary>
    public void Setup(int damageAmount, Vector3 worldPos, bool isCrit)
    {
        // 设置文字
        damageText.text = damageAmount.ToString();

        // 设置颜色
        damageText.color = isCrit ? critColor : normalColor;

        // 暴击更大一些
        if (isCrit)
            damageText.fontSize *= critScaleMultiplier;

        // 世界→屏幕
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        transform.position = screenPos;
    }

    // ======= 工厂方法：直接调用即可创建 =======
    public static DamagePopup Create(DamagePopup prefab, int damageAmount, Vector3 worldPos, bool isCrit = false)
    {
        DamagePopup popup = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        popup.Setup(damageAmount, worldPos, isCrit);
        return popup;
    }
}
