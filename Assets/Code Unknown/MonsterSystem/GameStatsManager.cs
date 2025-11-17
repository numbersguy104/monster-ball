using System;
using System.Collections.Generic;
using cfg;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class GameStatsManager : MonoBehaviour
{
    public static GameStatsManager Instance { get; private set; }

    public Material fullscreenRenderFeature;
    
    [Header("Player Stats")]
    public long score = 0;         // Total score
    public long gold = 0;          // Total gold (can decrease)
    public long totalGoldGained = 0;
    public int killCount = 0;      // Number of monsters killed
    public long totalDamage = 0;   // Total damage dealt
    public float dps = 0f;         // Damage per second (calculated at runtime)
    public float terMulti = 1f;    // All terrain's point multiplier value at this milestone
    public float monsMulti = 1f;   // All monster's point multiplier value at this milestone
    public List<ArtifactParam> artifacts = new List<ArtifactParam>();

    /*
     * Level Up Example:
     *  Starting Threshold = 10000
     *  Threshold Multipliers = {1.1, 1.25, 1.5}
     *  Threshold Increase Levels = {5, 10}
     * These values will cause the following behavior:
     *  The first level-up (level 1) will happen at 10000 points.
     *  The next four level-ups (levels 2-5) will multiply that by 1.1 each time:
     *  11000, 12100, 13310, 14641.
     *  After that, the next five level-ups (levels 6-10) will multiply by 1.25:
     *  18301, 22876, 28595, 35743, 44678.
     *  After level 10, the threshold will multiply by 1.5 per level for the rest of the game:
     *  67017, 100525, 150787... (continuing forever)
     */

    [Header("LEVEL UP SETTINGS\nThe level mechanic uses two queues\nto track the amount of points\nneeded to level up for each level.\n\nThreshold Increase Levels is a list of level numbers,\nand Threshold Multipliers is a list of multipliers,\nthe first of which is initially\nset as the \"current multiplier.\"\n\nWhenever the score threshold is reached,\nit is multiplied by the current multiplier.\n\nThe current multiplier increases to its next value\nwhenever the current level reaches one of\nthe values in Threshold Increase Levels.\n\nSee the comments in GameStatsManager.cs\nfor an example.")]
    [SerializeField] long startingThreshold = 10000;
    [SerializeField] List<int> thresholdIncreaseLevels = null;
    [SerializeField] List<float> thresholdMultipliers = null;

    public long levelUpThreshold; //Store the amount of points required to level up
    public int killReq;
    public int lvlKills;
    int level = 0;
    
    public UnityEvent OnLevelUp; // Level-up event interface (can be bound externally)

    private float damageTimer = 0f;
    private long damageThisSecond = 0;

    #region artifact

    // Fireball
    [HideInInspector]
    public float Fireball_TrailDurationMulti = 1.0f;
    [HideInInspector]
    public float Fireball_TrailSize = 1.0f;
    [HideInInspector]
    public float Fireball_LeaveDuration = 1.0f;
    
    //Splitball
    [HideInInspector]
    public float Splitball_SplitCount = 1.0f;
    [HideInInspector]
    public float Splitball_BallAttack = 1.0f;
    [HideInInspector]
    public float Splitball_BallSize = 1.0f;
    [HideInInspector]
    public float Splitball_SkillReq = 1.0f;
    
    //Stenball
    [HideInInspector]
    public float Stenball_Speed = 1.0f;

    //Iceball
    [HideInInspector]
    public float Iceball_ZoneSize = 1.0f;
    [HideInInspector]
    public float Iceball_BallAttack = 1.0f;
    [HideInInspector]
    public float Iceball_BallSkillReq = 1.0f;
    
    //Lightningball
    [HideInInspector]
    public float Lightningball_LightningSize = 1.0f;
    [HideInInspector]
    public float Lightningball_BallPrice = 1.0f;
    [HideInInspector]
    public float Lightningball_LightningFreq = 1.0f;
    [HideInInspector]
    public float Lightningball_BallAttack = 1.0f;
    [HideInInspector]
    public float Lightningball_BallSkillReq = 1.0f;
    [HideInInspector]
    public float Lightningball_BallSpd = 1.0f;

    
    //Invester
    [HideInInspector] 
    public float startingGold = 0;
    //Headhunter
    [HideInInspector] 
    public float artifactDamageMulti = 1;
    [HideInInspector] 
    public float artifactScoreMulti = 1;
    //Stargate
    [HideInInspector] 
    public float artifactScoreTeleporterMulti = 1;
    //BumperPoint
    [HideInInspector] 
    public float artifactScoreBumperMulti = 1;
    //SpinnerPoint
    [HideInInspector] 
    public float artifactScoreSpinnerMulti = 1;
    //SwitchPoint
    [HideInInspector] 
    public float artifactScoreSwitchMulti = 1;
    //DamagerPoint
    [HideInInspector] 
    public float artifactScoreDamagerMulti = 1;
    //GoldIncrease
    [HideInInspector] 
    public float GoldMulti = 1;
    public float BallPriceMulti = 1;
    
    #endregion
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist globally
    }

    void Start()
    {
        // Read milestone Configs
        TbMilestoneParam tbMilestoneParam = LubanTablesMgr.Instance.tables.TbMilestoneParam;
        levelUpThreshold = tbMilestoneParam.DataList[0].MilestoneReq;
        killReq = tbMilestoneParam.DataList[0].MonsterKillReq;
        // multi
        terMulti = tbMilestoneParam.DataList[0].TerPointInc;
        monsMulti = tbMilestoneParam.DataList[0].MonPointInc;

        //Initialize the level-up thresholds to defaults if not set in the inspector
        if (thresholdIncreaseLevels == null || thresholdIncreaseLevels.Count == 0)
        {
            Debug.LogWarning("thresholdIncreaseLevels was not initialized! Initializing with no levels. Source: GameStatsManager.cs on object " + gameObject.ToString());
            thresholdIncreaseLevels = new List<int>();
        }
        if (thresholdMultipliers == null || thresholdMultipliers.Count == 0)
        {
            Debug.LogWarning("thresholdMultipliers was not initialized! Initializing with a single multiplier of x2 per level. Source: GameStatsManager.cs on object " + gameObject.ToString());
            thresholdMultipliers = new List<float>();
            thresholdMultipliers.Add(2.0f);
        }

        ApplyArtifacts();
        AddGold((long)startingGold);
        
        OnLevelUp.AddListener(LevelUp);

        SoundManager.Instance.PlayBGM();
    }

    void Update()
    {
        // Calculate DPS
        damageTimer += Time.deltaTime;
        if (damageTimer >= 1f)
        {
            dps = damageThisSecond / damageTimer;
            damageTimer = 0f;
            damageThisSecond = 0;
        }

        // Check level-up condition
        if (score >= levelUpThreshold && lvlKills >= killReq)
        {
            OnLevelUp?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            AddGold(300);
            AddScore(1000, ScoreSource.Terrain);
            AddKill();
            AddKill();
        }
    }

    void LevelUp()
    {
        level++;

        SoundManager.Instance.PlayNextRandomBGM();
        //Check if we're at a level where the threshold multiplier changes
        if (thresholdIncreaseLevels.Count > 0 && thresholdIncreaseLevels[0] == level)
        {
            thresholdIncreaseLevels.RemoveAt(0);
            thresholdMultipliers.RemoveAt(0);
        }

        //Increase the score threshold
        // levelUpThreshold = (long)(levelUpThreshold * thresholdMultipliers[0]);
        //caculate point requirement
        TbMilestoneParam tbMilestoneParam = LubanTablesMgr.Instance.tables.TbMilestoneParam;
        float multi = 1f;
        for (int i = 0; i < tbMilestoneParam.DataList.Count; i++)
        {
            if (level + 1 > tbMilestoneParam.DataList[i].MilstoneLevel)
            {
                multi = tbMilestoneParam.DataList[i].MilestoneMulti;
                // multi update
                terMulti = tbMilestoneParam.DataList[i].TerPointInc;
                monsMulti = tbMilestoneParam.DataList[i].MonPointInc;
            }
            else
            {
                break;
            }
        }

        levelUpThreshold = (long)((multi + 1) * levelUpThreshold);

        print("DEBUG: Level " + level + " reached. Threshold is " + levelUpThreshold);
        
        killReq = tbMilestoneParam.DataList[level].MonsterKillReq;
        lvlKills = 0;
        
    }

    void ApplyArtifacts()
    {
        if (artifacts == null || artifacts.Count <= 0)
        {
            return;
        }

        // var artifactsTable = LubanTablesMgr.Instance.tables.TbArtifactParam;
        foreach (var artifact in artifacts)
        {
            if (!string.IsNullOrEmpty(artifact.BallRelation)) // ball
            {
                ApplyBallArtifact(artifact);
            }
            else
            {
                ApplySpecialArtifact(artifact);
            }
        }
    }

    void ApplyBallArtifact(ArtifactParam ap)
    {
        switch (ap.ID)
        {
            case cfg.Artifacts.ID.FireTrailDuration:
                Fireball_TrailDurationMulti *= ap.ArtifactStat1;
                break;
            case cfg.Artifacts.ID.FireTrailSize:
                Fireball_TrailSize *= ap.ArtifactStat1;
                break;
            case cfg.Artifacts.ID.FireTrailUptime:
                Fireball_LeaveDuration *= ap.ArtifactStat1;
                break;
            case cfg.Artifacts.ID.SplitCount:
                Splitball_SplitCount *= ap.ArtifactStat1;
                Splitball_BallAttack *= ap.ArtifactStat2;
                break;
            case cfg.Artifacts.ID.SplitSize:
                Splitball_BallSize *= ap.ArtifactStat1;
                break;
            case cfg.Artifacts.ID.Splitfire:
                Splitball_SkillReq *= ap.ArtifactStat1;
                break;
            case cfg.Artifacts.ID.StenSpeed:
                Stenball_Speed *= ap.ArtifactStat1;
                break;
            case cfg.Artifacts.ID.IceSize:
                Iceball_ZoneSize *= ap.ArtifactStat1;
                break;
            case cfg.Artifacts.ID.IceAtk:
                Iceball_BallAttack *= ap.ArtifactStat1;
                Iceball_BallSkillReq *= ap.ArtifactStat2;
                break;
            case cfg.Artifacts.ID.LightningSize:
                Lightningball_LightningSize *= ap.ArtifactStat1;
                Lightningball_BallPrice *= ap.ArtifactStat2;
                break;
            case cfg.Artifacts.ID.LightningFrequency:
                Lightningball_LightningFreq *= ap.ArtifactStat1;
                Lightningball_BallAttack *= ap.ArtifactStat2;
                break;
            case cfg.Artifacts.ID.LightingReq:
                Lightningball_BallSkillReq *= ap.ArtifactStat1;
                Lightningball_BallSpd *= ap.ArtifactStat2;
                break;
        }
    }

    void ApplySpecialArtifact(ArtifactParam ap)
    {
        switch (ap.ID)
        {   
            case cfg.Artifacts.ID.StartingGold:
                startingGold = ap.ArtifactStat1;
                break;
            case cfg.Artifacts.ID.MonsterDamage:
                artifactDamageMulti = ap.ArtifactStat1;
                artifactScoreMulti = ap.ArtifactStat2;
                break;
            case cfg.Artifacts.ID.TeleportPoint:
                artifactScoreTeleporterMulti = ap.ArtifactStat1;
                break;
            case cfg.Artifacts.ID.BumperPoint:
                artifactScoreBumperMulti = ap.ArtifactStat1;
                break;
            case cfg.Artifacts.ID.SpinnerPoint:
                artifactScoreSpinnerMulti = ap.ArtifactStat1;
                break;
            case cfg.Artifacts.ID.SwitchPoint:
                artifactScoreSwitchMulti = ap.ArtifactStat1;
                break;
            case cfg.Artifacts.ID.DamagerPoint:
                artifactScoreDamagerMulti = ap.ArtifactStat1;
                break;
            case cfg.Artifacts.ID.GoldIncrease:
                GoldMulti = ap.ArtifactStat1;
                BallPriceMulti = ap.ArtifactStat2;
                break;
        }
    }

    // ====== API Methods ======

    public void AddScore(long amount, ScoreSource source)
    {
        switch (source)
        {
            case ScoreSource.Monster:
                score += amount * (long)monsMulti * (long)artifactScoreMulti;
                break;
            case ScoreSource.Terrain:
                score += amount * (long)terMulti * (long)artifactScoreMulti;
                break;
        }
        
        SoundManager.Instance.PlaySFX(SoundManager.Instance.pointAccumulateSource,SoundManager.Instance.pointAccumulateVolume);
    }

    public void AddGold(long amount)
    {
        amount *= (long)GoldMulti;
        gold += amount;
        totalGoldGained += amount;
        SoundManager.Instance.PlaySFX(SoundManager.Instance.goldAccumulateSource,SoundManager.Instance.goldAccumulateVolume);
    }

    public bool SpendGold(long amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            return true;
        }
        return false; // Not enough gold
    }

    public void AddKill()
    {
        killCount++;
        lvlKills++;
    }

    public void AddDamage(long amount)
    {
        totalDamage += amount;
        damageThisSecond += amount;
    }

    public void AddArtifacts(List<ArtifactParam> af)
    {
        artifacts.Clear();
        artifacts.AddRange(af);
    }

    public void GlitchEffect()
    {
        fullscreenRenderFeature.SetFloat("_IsEnable", 1f);
        
        DOVirtual.DelayedCall(1f, () => fullscreenRenderFeature.SetFloat("_IsEnable", 0f));
    }
}

public enum ScoreSource
{
    Monster,
    Terrain
}
