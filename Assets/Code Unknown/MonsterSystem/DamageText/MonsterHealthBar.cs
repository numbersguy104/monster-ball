using System.Linq;
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
    private AbstractBall[] _balls;
    
    void Start()
    {
        monster = GetComponentInParent<MonsterController>();
        if (monster == null)
        {
            Debug.LogWarning("MonsterHealthBar: MonsterController not found!");
        }

        AbstractBall[] balls = FindObjectsOfType<AbstractBall>();
        _balls = balls;
    }

    void Update()
    {
        if (monster == null) return;

        if (_balls == null)
        {
            return;
        }

        float minDis = float.MaxValue;
        
        foreach (var ball in _balls.Where(b => b != null))
        {
            if (ball == null)
            {
                continue;
            }
            var dis = Vector3.Distance(ball.gameObject.transform.position, gameObject.transform.position);
            if (dis < minDis)
            {
                minDis = dis;
            }
        }
        healthbar.SetActive(minDis < 1f);
        
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
