using TMPro;
using UnityEngine;

public class DamageTextController : MonoBehaviour
{
    public float floatSpeed = 40f;   // 像素速度（UI）
    public float duration = 1.2f;

    private float timer;
    private TextMeshProUGUI text;
    private Camera cam;

    // 缓存世界坐标
    private Vector3 worldPos;
    // 用来渲染的屏幕坐标
    private Vector3 screenPos;

    public void Init(int damage, bool crit, Vector3 wp)
    {
        worldPos = wp;
        cam = Camera.main;

        text.text = damage.ToString();
        text.color = crit ? Color.yellow : Color.white;
        text.fontSize = crit ? 40 : 30;

        timer = duration;
    }

    void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
            return;
        }

        // 将世界坐标转屏幕坐标
        screenPos = cam.WorldToScreenPoint(worldPos);

        // UI屏幕空间向上漂浮
        screenPos.y += floatSpeed * Time.deltaTime;

        // 同步 UI 元素位置
        transform.position = screenPos;
    }
}
