using UnityEngine;
using UnityEngine.InputSystem;

public class BallLauncher : MonoBehaviour
{
    //Multiplier for how much force the ball launcher should apply to the ball
    const float POWER_SCALE = 15.0f;

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
                //Force on the ball scales with charge time, up to a maximum
                float power = Mathf.Min(chargeTime, 1.0f) * POWER_SCALE;

                ball.GetComponent<Rigidbody>().linearVelocity = new Vector3(0, 0, power);

                chargeTime = 0;
                active = false;
                ball = null;
            }
        }
    }
}
