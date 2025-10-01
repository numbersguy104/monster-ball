using System.Collections.Generic;
using UnityEngine;

public class PinballQueue : MonoBehaviour
{
    //Prefab for the base ball object - the player will start with a given amount of these
    [SerializeField] GameObject ballPrefab;

    [Tooltip("Number of balls the player should start the game with")]
    [SerializeField] int startingBalls = 3;

    [Tooltip("World coordinates for balls in the queue (specifically where the bottom-most ball should be)")]
    [SerializeField] Vector3 spawnPosition = Vector3.zero;

    Queue<Ball> ballQueue = new Queue<Ball>();

    void Start()
    {
        Vector3 nextSpawn = spawnPosition;
        for (int i = 0; i < startingBalls; i++)
        {
            //Hack to spawn the balls in a nice stack
            //Instantiate the balls under the launcher, which should have the correct scale
            GameObject ballObject = Instantiate(ballPrefab, FindAnyObjectByType<BallLauncher>().transform);

            Ball ball = ballObject.GetComponent<Ball>();
            ballQueue.Enqueue(ball);

            ballObject.transform.position = nextSpawn;

            //Translate the next ball's spawn position in the correct direction by the correct amount
            nextSpawn += ballObject.transform.forward * ballObject.transform.lossyScale.x;
        }

        NextBall();
    }
    
    //Pop a ball from the queue and bring it into play
    public void NextBall()
    {
        if (ballQueue.Count > 0)
        {
            Ball newBall = ballQueue.Dequeue();
            newBall.Activate();
        }
        else
        {
            print("Game Over");
        }
    }
}
