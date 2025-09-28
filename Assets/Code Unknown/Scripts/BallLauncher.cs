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

    [SerializeField] private GameObject light;

    bool active = true; //Whether the launcher is currently usable
    float chargeTime = 0.0f; //How long the button to "pull back" the launcher has been held, in seconds
    GameObject ballObject = null;

    [SerializeField] GameObject ballPrefab;

    InputAction chargeAction;

    void Start()
    {
        chargeAction = InputSystem.actions.FindAction("Charge");
        ballObject = SpawnBall();
        light.transform.position = new Vector3(
            ballObject.transform.position.x,
            ballObject.transform.position.y + 0.31f,
            ballObject.transform.position.z - 0.1f
        );
    }

    public GameObject SpawnBall()
    {
        return Instantiate(ballPrefab, transform);
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
                float power = Mathf.Min(chargeTime, maxCharge) / maxCharge * maxPower;

                ballObject.GetComponent<Ball>().SetVelocity(0, 0, power);

                chargeTime = 0;
                active = false;
                ballObject = null;
            }
        }
    }
}
