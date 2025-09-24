using UnityEngine;
using UnityEngine.InputSystem;

public class BallLauncher : MonoBehaviour
{
    //Maximum velocity of the ball when launching it
    //This is multiplied by how long the launcher has been charged, percentage-wise
    [Tooltip("Maximum velocity to set the ball to (in meters per second)")]
    [SerializeField] float Power = 15.0f;

    //Maximum time for the launcher to be charged
    //Holding the button longer than this will not add any more power
    [Tooltip("Time to reach maximum charge (in seconds)")]
    [SerializeField] float MaxCharge = 1.0f;

    bool active = true; //Whether the launcher is currently usable
    float chargeTime = 0.0f; //How long the button to "pull back" the launcher has been held, in seconds
    GameObject ball = null;

    [SerializeField] GameObject ballPrefab;

    InputAction chargeAction;

    void Start()
    {
        chargeAction = InputSystem.actions.FindAction("Charge");
        SpawnBall();
    }

    public void SpawnBall()
    {
        ball = Instantiate(ballPrefab, this.transform);
    }

    void Update()
    {
        if (active)
        {
            bool chargeHeld = chargeAction.IsPressed();
            if (chargeHeld)
            {
                chargeTime = chargeTime + Time.deltaTime;
            }
            else if (chargeTime > Mathf.Epsilon)
            {
                //Force on the ball scales with charge time, up to the maximum
                float power = Mathf.Min(chargeTime, MaxCharge) / MaxCharge * Power;

                ball.GetComponent<Rigidbody>().linearVelocity = new Vector3(0, 0, power);

                chargeTime = 0;
                active = false;
                ball = null;
            }
        }
    }
}
