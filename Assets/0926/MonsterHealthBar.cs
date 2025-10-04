using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthBar : MonoBehaviour
{
    [Header("UI References")]
    public Image redBar;    // Red health bar (immediate decrease)
    public Image yellowBar; // Yellow health bar (delayed decrease)
    public GameObject healthbar;
    
    [Header("Settings")]
    public float yellowBarSpeed = 0.5f; // Yellow bar decrease speed (higher = faster)

    private MonsterController monster;
    private float targetFill = 1f; // Target ratio for red bar
    private float yellowFill = 1f; // Current ratio for yellow bar
    private Ball _ball;
    
    void Start()
    {
        monster = GetComponentInParent<MonsterController>();
        if (monster == null)
        {
            Debug.LogWarning("MonsterHealthBar: MonsterController not found!");
        }
        
        Ball[] balls = FindObjectsOfType<Ball>();
        foreach (var ball in balls)
        {
            if (ball.active)
            {
                _ball = ball;
                break;
            }
        }
    }

    void Update()
    {
        if (monster == null) return;

        if (_ball == null)
        {
            return;
        }

        healthbar.SetActive(Vector3.Distance(_ball.gameObject.transform.position, gameObject.transform.position) < 1f);
        
        // Calculate HP ratio
        float hpRatio = Mathf.Clamp01((float)monster.hp / monster.GetMaxHP());

        // Red bar updates instantly
        targetFill = hpRatio;
        redBar.fillAmount = targetFill;

        // Yellow bar gradually follows
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
