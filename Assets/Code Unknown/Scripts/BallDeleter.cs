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
        AbstractBall ball = other.gameObject.GetComponent<AbstractBall>();
        if (ball != null)
        {
            Destroy(ball.gameObject);
            queue.NextBall();
        }
    }
}
