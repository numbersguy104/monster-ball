using UnityEngine;
using UnityEngine.InputSystem;

public class Flipper : MonoBehaviour
{
    //How far the flipper will rotate, in degrees
    //Higher number = more powerful flipper that also moves farther
    const int ROTATION_RANGE = 30;

    //How long the flipper should take to reach END_ROTATION, in seconds
    //Lower time = faster and more powerful flipper
    const float FLIP_TIME = 0.05f;

    float flipperProgress = 0.0f;
    enum Side { Left, Right };
    Quaternion startingRotation;

    [SerializeField] Side flipperSide;

    InputAction flipAction;
    Rigidbody rb;


    void Start()
    {
        startingRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
        if (flipperSide == Side.Left)
        {
            flipAction = InputSystem.actions.FindAction("Left Flipper");
        }
        else
        {
            flipAction = InputSystem.actions.FindAction("Right Flipper");
        }
    }
    void Update()
    {
        bool flipPressed = flipAction.IsPressed();
        if (flipPressed)
        {
            flipperProgress = Mathf.Min(flipperProgress + Time.deltaTime, FLIP_TIME);
        }
        else
        {
            flipperProgress = Mathf.Max(flipperProgress - Time.deltaTime, 0.0f);
        }

        float degrees = (flipperProgress / FLIP_TIME) * ROTATION_RANGE;
        if (flipperSide == Side.Left)
        {
            degrees = -degrees;
        }

        Quaternion rotation = startingRotation * Quaternion.Euler(0.0f, degrees, 0.0f);
        rb.MoveRotation(rotation);
    }
}
