using System;
using UI;
using UnityEngine;
using DG.Tweening;

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
    public GameObject damagePopupPrefab; // drDamagePopup prefab
    private Canvas damageCanvas;
    private Camera mainCam;
    private bool movementIsPaused = false;
    public SkinnedMeshRenderer meshRenderer;
    private Material _material;

    public event Action<MonsterController> OnMonsterDestroy;

    private Animator animator;
    void Awake()
    {
        collisionSelector = GetComponent<CollisionSelector>();
        movement = GetComponent<MonsterMovement>();
        animator = GetComponentInChildren<Animator>();
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
        // init healthbar
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
        var dc = GameObject.Find("Canvas");
        if (dc != null)
            damageCanvas = dc.GetComponent<Canvas>();

        if (meshRenderer != null)
        {
            _material = meshRenderer.material;
        }
    }

    private void OnDestroy()
    {
        OnMonsterDestroy?.Invoke(this);
        OnMonsterDestroy = null;
    }

    public void TakeDamage(int damage,bool crit = false)
    {
        hp -= damage;

        DamageTextSpawner.Instance.Spawn(    transform.position + Vector3.up * 2f,damage,crit);


        if (healthBarUI != null)
            healthBarUI.redBar.fillAmount = Mathf.Clamp01((float)hp / maxHP);

        if (hp <= 0)
            Die();
    }

    public void PauseMovement()
    {
        if (movementIsPaused) return;
        movementIsPaused = true;
        movement.enabled = false;
    }

    public void ResumeMovement()
    {
        if (!movementIsPaused) return;
        movementIsPaused = false;
        movement.enabled = true;
    }



    public void Die()
    {
        GameStatsManager.Instance.AddGold(gold);
        GameStatsManager.Instance.AddScore(point, ScoreSource.Monster);
        GameStatsManager.Instance.AddKill();
        GameStatsManager.Instance.AddScore(100, ScoreSource.Monster);

        var mainUI = FindAnyObjectByType<UIGameMain>();
        mainUI?.Refresh();
        if (movement != null)
            movement.enabled = false;
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        if (_material != null)
        {
            _material.SetFloat("_AlphaClipThreshold", -0.2f);
            DOTween.To(
                () => _material.GetFloat("_AlphaClipThreshold"),   // getter
                x => _material.SetFloat("_AlphaClipThreshold", x), // setter
                1f,                                       // target
                3f                                        // duration
            );
        }
        
        Destroy(gameObject,3.0f);
    }
}
