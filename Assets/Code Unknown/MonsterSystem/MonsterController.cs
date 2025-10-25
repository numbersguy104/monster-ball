using UI;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [Header("Data")]
    public string monsterID;
    public int hp;
    public int point;
    public int gold;
    public int maxHP;
    public MonsterCollisionType collisionType;

    private CollisionSelector collisionSelector;
    private MonsterMovement movement;

    [Header("UI")]
    public GameObject healthBarPrefab;
    private MonsterHealthBar healthBarUI;

    [Header("Damage Popup")]
    public GameObject damagePopupPrefab; // 拖入DamagePopup prefab
    private Canvas damageCanvas;
    private Camera mainCam;

    void Awake()
    {
        collisionSelector = GetComponent<CollisionSelector>();
        movement = GetComponent<MonsterMovement>();
    }

    public int GetMaxHP()
    {
        return maxHP > 0 ? maxHP : 1;
    }

    public void InitializeFromParam(MonsterParam param)
    {
        if (param == null) return;
        monsterID = param.monsterID;
        hp = param.hp;
        maxHP = param.hp;
        point = param.point;
        gold = param.gold;
        collisionType = param.collisionType;

        if (collisionSelector != null)
            collisionSelector.SetCollisionType(collisionType);
    }

    public void SetMovement(MovementType type, float speed, float lengthOrRadius, Vector3 spawnCenter, Quaternion orbitRotation)
    {
        if (movement == null) return;

        movement.movementType = type;
        movement.speed = speed;
        if (type == MovementType.Horizontal || type == MovementType.Vertical)
            movement.length = lengthOrRadius;
        else if (type == MovementType.Circular)
            movement.radius = lengthOrRadius;

        movement.SetSpawnCenter(spawnCenter, orbitRotation);
    }

    public void SetSpawnCenter(Vector3 pos)
    {
        if (movement != null)
            movement.SetSpawnCenter(pos);
    }

    void Start()
    {
        // 初始化血条
        if (healthBarPrefab != null)
        {
            Canvas worldCanvas = FindFirstObjectByType<Canvas>();
            GameObject barObj = Instantiate(healthBarPrefab, worldCanvas.transform);

            BillboardFollow follow = barObj.GetComponent<BillboardFollow>();
            if (follow != null)
            {
                follow.target = this.transform;
                follow.offset = new Vector3(0, 2, 0);
            }

            healthBarUI = barObj.GetComponent<MonsterHealthBar>();
            if (healthBarUI != null)
            {
                healthBarUI.redBar.fillAmount = 1f;
                healthBarUI.yellowBar.fillAmount = 1f;
            }
        }

        mainCam = Camera.main;
        damageCanvas = GameObject.Find("DamageCanvas").GetComponent<Canvas>();
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        // ✅ 显示伤害跳字
        ShowDamagePopup(damage);

        // stats update
        GameStatsManager.Instance.AddDamage(damage);

        if (healthBarUI != null)
            healthBarUI.redBar.fillAmount = Mathf.Clamp01((float)hp / maxHP);

        if (hp <= 0)
            Die();
    }

    void ShowDamagePopup(int damage)
    {
        // 防止未绑Prefab报错
        if (damagePopupPrefab == null || damageCanvas == null) return;

        // 生成UI对象到DamageCanvas下
        GameObject popupObj = Instantiate(damagePopupPrefab, damageCanvas.transform);

        // 获取脚本
        DamagePopup popup = popupObj.GetComponent<DamagePopup>();

        // 头顶位置偏移
        Vector3 headPos = transform.position + Vector3.up * 2f;

        // 调用setup
        popup.Setup(damage, headPos, false);
    }

    public void Die()
    {
        GameStatsManager.Instance.AddGold(gold);
        var mainUI = FindAnyObjectByType<UIGameMain>();
        mainUI.Refresh();
        GameStatsManager.Instance.AddScore(point, ScoreSource.Monster);
        GameStatsManager.Instance.AddKill();
        GameStatsManager.Instance.AddScore(100, ScoreSource.Monster);

        Destroy(gameObject);
    }
}
