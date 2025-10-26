using System.Drawing;
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
        AbstractBall ball = other.GetComponent<AbstractBall>();
        if (ball != null)
        {
            pt.AddTerrainPoints(PointsTracker.PointSources.BossDamager, ball);

            VFXManager.Instance.PlayVFX(
                VFXManager.Instance.Star_hit,
                collision.contacts[0].point,
                Quaternion.identity,
                0.9f
            );

            //placeholder
            print("Boss Damaged!");
        }
    }
}
