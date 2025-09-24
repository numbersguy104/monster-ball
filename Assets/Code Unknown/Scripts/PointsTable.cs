using UnityEngine;

public class PointsTable : MonoBehaviour
{
    //Define point values for everything in the game
    //These are meant to be edited from the Unity editor, under the "PointsTable" object
    //All default values are 0, as a fallback

    //Note: All point amounts are stored as longs, in case they go above 2 billion
    //(Pinball is known for its crazy scoring numbers)

    [Tooltip("Points awarded for hitting a bumper")]
    [SerializeField] long Bumper = 0L;
    [Tooltip("Points awarded for spinning a spinner for the first time")]
    [SerializeField] long SpinnerBase = 0L;
    [Tooltip("Compounding multiplier to points awarded for consecutive spinner spins after the first")]
    [SerializeField] float SpinnerMult = 0f;
    [Tooltip("Points awarded for activating one drop switch")]
    [SerializeField] long Switch = 0L;
    [Tooltip("Points awarded for activating all drop switches in a group")]
    [SerializeField] long AllSwitch = 0L;
    [Tooltip("Points awarded for hitting a smoke clearer")]
    [SerializeField] long SmokeClearer = 0L;
    [Tooltip("Points awarded for using a portal / teleporter")]
    [SerializeField] long Portal = 0L;
    [Tooltip("Points awarded for hitting a boss-damaging terrain")]
    [SerializeField] long BossDamager = 0L;
}
