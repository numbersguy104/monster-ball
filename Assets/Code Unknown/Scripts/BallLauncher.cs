using NUnit.Framework;
using System.Collections.Generic;
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

    [Tooltip("How close the ball needs to be to the launcher to be launched")]
    [SerializeField] float reactivationRange = 0.1f;

    [Tooltip("Cooldown between uses of the launcher")]
    [SerializeField] float maxCooldown = 1.0f;

    float cooldown = 0.0f;

    //The current ball in play (including a ball still in the launcher). Null if there is none.
    Ball currentBall = null;

    //Whether the launcher can be used to launch the CurrentBall's game object.
    bool usable = true;

    //How long the button to "pull back" the launcher has been held, in seconds
    float chargeTime = 0.0f;

    InputAction chargeAction;

    void Start()
    {
        chargeAction = InputSystem.actions.FindAction("Charge");
    }

    void Update()
    {
        if (cooldown > 0.0f)
        {
            cooldown = Mathf.Max(cooldown - Time.deltaTime, 0.0f);
        }
        else
        {
            if (!usable && currentBall != null && cooldown == 0.0f)
            {
                float distance = Vector3.Distance(transform.position, currentBall.transform.position);
                if (distance <= reactivationRange)
                {
                    usable = true;
                }
            }

            if (usable && currentBall != null)
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
                    cooldown = maxCooldown;
                    usable = false;
                }
            }
        }
    }

    public void NewBall(Ball ball)
    {
        currentBall = ball;
        usable = true;
    }
}