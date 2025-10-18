using UnityEngine;

public class Spinner : MonoBehaviour
{
    //Exponential decay for the spinner's speed each second
    //Lower number = spinner comes to rest faster
    [Tooltip("Multiplier for the spinner's speed each second (visual only; no gameplay effect)")]
    [SerializeField] float spinDecay = 0.2f;

    //How many rotations the spinner should rotate before returning to rest
    //This can be negative (indicating rotations in the opposite direction)
    float storedRotations = 0.0f;

    //The rotating graphic for the spinner
    Transform graphic = null;

    //Starting rotation (used when resetting the spinner)
    Quaternion startRotation = Quaternion.identity;

    PointsTracker pt;

    void Start()
    {
        Transform parent = transform.parent;
        foreach (Transform sibling in parent)
        {
            if (sibling != transform)
            {
                graphic = sibling;
                startRotation = sibling.rotation;
                break;
            }
        }

        pt = FindAnyObjectByType<PointsTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (graphic != null)
        {
            if (Mathf.Abs(storedRotations) < 0.001f)
            {
                //Reset if the rotation count is low, to avoid floating point error buildup
                storedRotations = 0.0f;
                graphic.rotation = startRotation;
            }
            else
            {
                float oldRotations = storedRotations;
                storedRotations *= Mathf.Pow(spinDecay, Time.deltaTime);
                float degreesChange = (oldRotations - storedRotations) * 360.0f;
                graphic.rotation *= Quaternion.AngleAxis(degreesChange, -transform.right);

                //Whenever the spinner completes a full rotation...
                //(i.e. the number of rotations crossed an integer threshold this frame)
                float threshold = Mathf.Ceil(Mathf.Abs(storedRotations));
                if (threshold > 0 && Mathf.Abs(oldRotations) >= threshold)
                {
                    //...award points, scaling based on how many rotations it has left
                    pt.AddSpinnerPoints((int)threshold - 1);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        AbstractBall ball = other.gameObject.GetComponent<AbstractBall>();
        if (ball != null)
        {
            Vector3 ballVelocity = ball.GetVelocity();
            float velocityComponent = Vector3.Dot(ballVelocity, transform.forward);
            storedRotations += Mathf.Floor(velocityComponent);
        }
    }
}
