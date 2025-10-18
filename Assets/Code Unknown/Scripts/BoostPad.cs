using UnityEngine;

public class BoostPad : MonoBehaviour
{
    //The velocity the boost pad will set the ball to
    [Tooltip("Velocity to set the ball to (in meters per second)")]
    [SerializeField] float power = 6.0f;

    void OnTriggerStay(Collider other)
    {
        AbstractBall ball = other.gameObject.GetComponent<AbstractBall>();
        if (ball != null)
        {
            //Don't slow down the ball if it's going faster
            float currentSpeed = ball.GetVelocity().magnitude;
            if (currentSpeed < power)
            {
                ball.SetSpeed(power);
            }
        }
    }
}
