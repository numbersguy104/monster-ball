using UnityEngine;

// Runtime script for a single monster (HP, loot, collision settings, movement behavior anchor)
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
    public GameObject healthBarPrefab;      // Drag in health bar prefab
    private MonsterHealthBar healthBarUI;   // Reference to health bar script

    void Awake()
    {
        collisionSelector = GetComponent<CollisionSelector>();
        movement = GetComponent<MonsterMovement>();
    }

    public int GetMaxHP()
    {
        // If more flexibility is needed, add a maxHp field in MonsterParam
        //return hp <= 0 ? 1 : hp;
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

        // Set collision
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


    /// <summary>
    /// Called by Spawner to explicitly set the anchor (to avoid conflicting with prefab’s own position)
    /// </summary>
    public void SetSpawnCenter(Vector3 pos)
    {
        if (movement != null)
            movement.SetSpawnCenter(pos);
    }

    // Placeholder: on defeat, notify spawner system (future)
    void Start()
    {
        // Generate health bar
        if (healthBarPrefab != null)
        {
            // Assume there is a global WorldSpace Canvas
            Canvas worldCanvas = FindFirstObjectByType<Canvas>();
            GameObject barObj = Instantiate(healthBarPrefab, worldCanvas.transform);

            // Set health bar to follow
            BillboardFollow follow = barObj.GetComponent<BillboardFollow>();
            if (follow != null)
            {
                follow.target = this.transform;   // Make the health bar follow the monster
                follow.offset = new Vector3(0, 2, 0);
            }

            // Set health bar value control
            healthBarUI = barObj.GetComponent<MonsterHealthBar>();
            if (healthBarUI != null)
            {
                healthBarUI.redBar.fillAmount = 1f;
                healthBarUI.yellowBar.fillAmount = 1f;
            }
        }
    }

    public void TakeDamage(int baseDamage, float acceleration)
    {

        int finalDamage = Mathf.Max(baseDamage, Mathf.RoundToInt(baseDamage * acceleration));


        hp -= finalDamage;

        // Global stats update
        GameStatsManager.Instance.AddDamage(finalDamage);
        GameStatsManager.Instance.AddScore(point / 10);

        if (healthBarUI != null)
        {
            healthBarUI.redBar.fillAmount = Mathf.Clamp01((float)hp / maxHP);
        }

        if (hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // TODO: add death VFX/logic, notify spawn manager that this spawn point is free
        GameStatsManager.Instance.AddGold(gold);
        GameStatsManager.Instance.AddScore(point);
        GameStatsManager.Instance.AddKill();
        // Test
        GameStatsManager.Instance.AddScore(100);
        // TODO: death effect
        Destroy(gameObject);
    }
}
