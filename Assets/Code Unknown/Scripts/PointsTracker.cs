using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PointsTracker : MonoBehaviour
{
    //Track the total score
    long score = 0L;

    //Define point values for everything in the game
    //These are currently meant to be edited from the object the script is attached to in the Unity editor
    //(MechanicsTest/Points/Canvas)
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

    void Start()
    {
        pointsDictionary.Add(PointSources.Bumper, bumper);
        pointsDictionary.Add(PointSources.Spinner, spinnerBase);
        pointsDictionary.Add(PointSources.SwitchOne, oneSwitch);
        pointsDictionary.Add(PointSources.SwitchGroup, allSwitch);
        pointsDictionary.Add(PointSources.SmokeClearer, smokeClearer);
        pointsDictionary.Add(PointSources.Portal, portal);
        pointsDictionary.Add(PointSources.BossDamager, bossDamager);
    }

    //Add a fixed number of points
    void AddPoints(long amount)
    {
        score += amount;
        GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();
    }

    //Add base points associated with a given type of terrain
    public void AddTerrainPoints(PointSources source)
    {
        AddPoints(pointsDictionary[source]);
    }

    //Add points for a spinner specifically
    //Spinners use a unique points formula based on their number of rotations
    public void AddSpinnerPoints(int spins)
    {
        long points = (long)(pointsDictionary[PointSources.Spinner] * Mathf.Pow(spinnerMult, spins));
        AddPoints(points);
    }

    public long GetPoints()
    {
        return score;
    }
}
