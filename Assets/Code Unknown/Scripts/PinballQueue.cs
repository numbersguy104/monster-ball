using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PinballQueue : MonoBehaviour
{
    //Prefab for the base ball object - the player will start with a given amount of these
    [SerializeField] GameObject ballPrefab;

    [Tooltip("Number of balls the player should start the game with")]
    [SerializeField] int startingBalls = 3;

    [Tooltip("World coordinates for where to spawn the bottom-most ball")]
    [SerializeField] Vector3 spawnPosition = Vector3.zero;

    public Queue<AbstractBall> ballQueue = new Queue<AbstractBall>();
    public static PinballQueue Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        for (int i = 0; i < startingBalls; i++)
        {
            AddBall();
        }
    }

    public void AddBall(string ballName = null)
    {
        //Hack: Use the ball launcher to get the board rotation and scale correct
        Transform launcherTransform = FindAnyObjectByType<BallLauncher>().transform;

        //Calculate the new ball's spawn position based on the number of current balls
        Vector3 offsetPerBall = (launcherTransform.forward * launcherTransform.lossyScale.x);
        Vector3 totalOffset = offsetPerBall * ballQueue.Count;
        Vector3 position = spawnPosition + totalOffset;

        //Instantiate the ball and add it to the queue
        GameObject ballObject;
        if (string.IsNullOrEmpty(ballName))
        {
            ballObject = Instantiate(
                ballPrefab, 
                position, 
                Quaternion.identity, 
                launcherTransform
                );
        }
        else
        {
            ballObject = Instantiate(
                BattleCommonUtils.GetPinballPrefab(ballName), 
                position, 
                Quaternion.identity, 
                launcherTransform
                );
        }
        AbstractBall ball = ballObject.GetComponent<AbstractBall>();
        ballQueue.Enqueue(ball);
    }
    
    //Pop a ball from the queue and bring it into play
    public void NextBall()
    {
        if (ballQueue.Count > 0)
        {
            AbstractBall newBall = ballQueue.Dequeue();
            newBall.Activate();
        }
    }
}
