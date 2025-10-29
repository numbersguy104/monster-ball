using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PointsTracker : MonoBehaviour
{
    //Track the total score
    //long score = 0L;

    //Define point values for everything in the game
    //These are currently meant to be edited from the object the script is attached to in the Unity editor
    //All default values are 0, as a fallback

    //Note: All point amounts are stored as longs, in case they go above 2 billion
    //(Pinball is known for its crazy scoring numbers)
    [Tooltip("Points awarded for hitting a bumper")]
    [SerializeField] long bumper = 0L;
    [Tooltip("Points awarded for spinning a spinner for the first time")]
    [SerializeField] long spinnerBase = 0L;
    [Tooltip("Compounding multiplier to points awarded for consecutive spinner spins after the first")]
    [SerializeField] float spinnerMult = 0f;
    [Tooltip("Points awarded for activating one drop switch")]
    [SerializeField] long oneSwitch = 0L;
    [Tooltip("Points awarded for activating all drop switches in a group")]
    [SerializeField] long allSwitch = 0L;
    [Tooltip("Points awarded for hitting a smoke clearer")]
    [SerializeField] long smokeClearer = 0L;
    [Tooltip("Points awarded for using a portal / teleporter")]
    [SerializeField] long portal = 0L;
    [Tooltip("Points awarded for hitting a boss-damaging terrain")]
    [SerializeField] long bossDamager = 0L;

    //Queue of global multipliers for terrain points
    //Each level up (points threshold) will advance the queue
    [Tooltip("Multipliers to terrain points; starts with the first value and advances to the next on each level up")]
    [SerializeField] List<float> terrainPointMults = null;
    //The current mulitplier value, starting with the first value in the queue
    float terrainPointsMult;

    //Track the number of points gotten from terrains specifically
    public long terrainPoints = 0;

    public enum PointSources
    {
        Bumper,
        Spinner,
        SwitchOne,
        SwitchGroup,
        SmokeClearer,
        Portal,
        BossDamager
    }

    Dictionary<PointSources, long> pointsDictionary = new Dictionary<PointSources, long>();

    BallCounter bc;

    void Start()
    {
        //Use the GameStatsManager hook for tracking level ups
        GameStatsManager gm = FindAnyObjectByType<GameStatsManager>();
        gm.OnLevelUp.AddListener(LevelUp);

        pointsDictionary.Add(PointSources.Bumper, bumper);
        pointsDictionary.Add(PointSources.Spinner, spinnerBase);
        pointsDictionary.Add(PointSources.SwitchOne, oneSwitch);
        pointsDictionary.Add(PointSources.SwitchGroup, allSwitch);
        pointsDictionary.Add(PointSources.SmokeClearer, smokeClearer);
        pointsDictionary.Add(PointSources.Portal, portal);
        pointsDictionary.Add(PointSources.BossDamager, bossDamager);

        //Default value for terrainPointMults if not initialized in inspector
        if (terrainPointMults == null || terrainPointMults.Count == 0)
        {
            Debug.LogWarning("Terrain points multiplier queue (terrainPointMults) was not initialized! Initializing with a single multiplier of 1.0. Source: PointsTracker.cs on object " + gameObject.ToString());
            terrainPointMults = new List<float>();
            terrainPointMults.Add(1.0f);
        }

        LevelUp();

        bc = FindAnyObjectByType<BallCounter>();
    }

    //Add a fixed number of points
    void AddPoints(long amount)
    {
        GameStatsManager.Instance.AddScore(amount, ScoreSource.Terrain);
        RefreshPoint();
    }

    //Add base points associated with a given type of terrain
    public void AddTerrainPoints(PointSources source, AbstractBall ball = null)
    {
        long points = pointsDictionary[source];
        points = (long)(points * terrainPointsMult);
        points *= bc.countActive;

        terrainPoints += points;
        AddPoints(points);

        if (ball != null && ball is AbstractAbilityBall)
        {
            ((AbstractAbilityBall)ball).AddSkillPoints(points);
        }
    }

    //Add points for a spinner specifically
    //Spinners use a unique points formula based on their number of rotations
    public void AddSpinnerPoints(int spins, AbstractBall ball = null)
    {
        long points = (long)(pointsDictionary[PointSources.Spinner] * Mathf.Pow(spinnerMult, spins));
        points = (long)(points * terrainPointsMult);
        points *= bc.countActive;

        terrainPoints += points;
        AddPoints(points);

        if (ball != null && ball is AbstractAbilityBall)
        {
            ((AbstractAbilityBall)ball).AddSkillPoints(points);
        }
    }

    public long GetTerrainPoints()
    {
        return terrainPoints;
    }

    //Advance the queue for multipliers (should be called after reaching a points threshold)
    void LevelUp()
    {
        if (terrainPointMults.Count > 0)
        {
            terrainPointsMult = terrainPointMults[0];
            terrainPointMults.RemoveAt(0);
        }
    }

    public void RefreshPoint()
    {
        GetComponent<TextMeshProUGUI>().text = GameStatsManager.Instance.score.ToString() + 
                                               "\n" + 
                                               ((float)GameStatsManager.Instance.score * 100 / (float)GameStatsManager.Instance.levelUpThreshold).ToString("F1") + 
                                               "%";
    }
}
