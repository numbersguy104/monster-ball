using UnityEngine;

public class BallDeleter : MonoBehaviour
{
    PinballQueue queue;

    void Start()
    {
        queue = FindAnyObjectByType<PinballQueue>();
    }

    void OnTriggerExit(Collider other)
    {
        Ball ball = other.gameObject.GetComponent<Ball>();
        if (ball != null)
        {
            Destroy(ball.gameObject);
            queue.NextBall();
        }
    }
}
