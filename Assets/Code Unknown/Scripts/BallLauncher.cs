using UnityEngine;
using UnityEngine.InputSystem;

public class BallLauncher : MonoBehaviour
{
    //Maximum velocity of the ball when launching it
    //This is multiplied by how long the launcher has been charged, percentage-wise
    [Tooltip("Maximum velocity to set the ball to (in meters per second)")]
    [SerializeField] float maxPower = 8.0f;

    //Maximum time for the launcher to be charged
    //Holding the button longer than this will not add any more power
    [Tooltip("Time to reach maximum charge (in seconds)")]
    [SerializeField] float maxCharge = 1.0f;

    //The current ball about to be launched. Null if there is none (i.e. if there is a ball in play)
    Ball currentBall = null;

    //How long the button to "pull back" the launcher has been held, in seconds
    float chargeTime = 0.0f;

    InputAction chargeAction;

    void Start()
    {
        chargeAction = InputSystem.actions.FindAction("Charge");
    }

    void Update()
    {
        if (currentBall != null)
        {
            bool chargeHeld = chargeAction.IsPressed();
            if (chargeHeld)
            {
                chargeTime = chargeTime + Time.deltaTime;
            }
            else if (chargeTime > Mathf.Epsilon)
            {
                //Force on the ball scales with charge time, up to the maximum
                float power = Mathf.Min(chargeTime, maxCharge) / maxCharge * maxPower;

                currentBall.SetVelocity(0, 0, power);

                chargeTime = 0;
                currentBall = null;
            }
        }
    }

    public void NewBall(Ball ball)
    {
        currentBall = ball;
    }
}