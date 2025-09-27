using UnityEngine;
using UnityEngine.InputSystem;

public class Bumper: MonoBehaviour
{
    //The velocity the bumper sets balls to on contact
    [SerializeField] float Power = 6.0f;

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
            Vector3 newVelocity = Vector3.Normalize(normal) * -Power;
            ball.SetVelocity(newVelocity);
        }
    }
}
