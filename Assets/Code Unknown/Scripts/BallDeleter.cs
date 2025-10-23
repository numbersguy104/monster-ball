using System.Linq;
using UI;
using UnityEngine;

public class BallDeleter : MonoBehaviour
{
    PinballQueue queue;

    void Start()
    {
        queue = FindAnyObjectByType<PinballQueue>();
    }

    void OnTriggerEnter(Collider other)
    {
        AbstractBall ball = other.gameObject.GetComponent<AbstractBall>();
        if (ball != null)
        {
            Destroy(ball.gameObject);

            int ballsRemaining = FindObjectsByType<AbstractBall>(FindObjectsSortMode.None).Count();

            //For some reason Unity still considers the ball that was just destroyed? (at least I think so)
            if (ballsRemaining <= 1)
            {
                FindAnyObjectByType<UIGameMain>().OpenGameOverPanel();
            }
        }
    }
}
