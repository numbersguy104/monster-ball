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
    void Start()
    {
        teleportOffset = pairedTeleporter.transform.position - transform.position;
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
            Ball ball = other.gameObject.GetComponent<Ball>();
            if (ball != null)
            {
                pairedTeleporter.GetComponent<Teleporter>().Disable();
                ball.transform.position += teleportOffset;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        Ball ball = other.gameObject.GetComponent<Ball>();
        if (ball != null)
        {
            onCooldown = false;
        }
    }
}
