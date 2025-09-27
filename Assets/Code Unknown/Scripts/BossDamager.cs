using UnityEngine;

public class BossDamager : MonoBehaviour
{
    PointsTracker pt;
    
    void Start()
    {
        pt = FindAnyObjectByType<PointsTracker>();
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        Ball ball = other.GetComponent<Ball>();
        if (ball != null)
        {
            pt.AddTerrainPoints(PointsTracker.PointSources.BossDamager);

            //placeholder
            print("Boss Damaged!");
        }
    }
}
