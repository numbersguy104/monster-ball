using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Tooltip("The other teleporter")]
    [SerializeField] GameObject pairedTeleporter = null;

    //True if the opposite teleporter was just used
    //Gets reset to false after the ball leaves this teleporter
    //This prevents the ball from teleporting back and forth repeatedly
    bool onCooldown = false;

    Vector3 teleportOffset;

    PointsTracker pt;

    void Start()
    {
        teleportOffset = pairedTeleporter.transform.position - transform.position;
        pt = FindAnyObjectByType<PointsTracker>();
    }

    void Disable()
    {
        onCooldown = true;
    }

    //Teleport the ball when it enters the teleporter
    void OnTriggerEnter(Collider other)
    {
        if (!onCooldown)
        {
            AbstractBall ball = other.gameObject.GetComponent<AbstractBall>();
            if (ball != null)
            {
                pairedTeleporter.GetComponent<Teleporter>().Disable();
                ball.transform.position += teleportOffset;

                pt.AddTerrainPoints(PointsTracker.PointSources.Portal);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        AbstractBall ball = other.gameObject.GetComponent<AbstractBall>();
        if (ball != null)
        {
            onCooldown = false;
        }
    }
}
