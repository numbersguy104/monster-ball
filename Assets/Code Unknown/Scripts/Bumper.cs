using UnityEngine;
using UnityEngine.InputSystem;

public class Bumper: MonoBehaviour
{
    //The speed the bumper sets balls to on contact
    [Tooltip("How fast the bumper should set the ball to go on contact")]
    [SerializeField] float power = 6.0f;

    PointsTracker pt;

    private void Start()
    {
        pt = FindAnyObjectByType<PointsTracker>();
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        Ball ball = other.GetComponent<Ball>();
        if (ball != null)
        {
            //Get the direction of the collision
            ContactPoint point = collision.GetContact(0); //Assume only 1 collision point
            Vector3 normal = point.normal;

            //Set the ball's velocity in that direction
            Vector3 newVelocity = Vector3.Normalize(normal) * -power;
            ball.SetVelocity(newVelocity);

            //Add points
            pt.AddTerrainPoints(PointsTracker.PointSources.Bumper);
        }
    }
}
