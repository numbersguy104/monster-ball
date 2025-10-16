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

    Queue<Ball> ballQueue = new Queue<Ball>();
    public static PinballQueue Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        
        for (int i = 0; i < startingBalls; i++)
        {
            AddBall();
        }

        NextBall();
    }

    //TEMPORARY DEV TOOL:
    //Add a keybind to manually add a new ball
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            AddBall();
            if (FindObjectsByType<Ball>(FindObjectsSortMode.None).Length == 1)
            {
                NextBall();
            }
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
        Ball ball = ballObject.GetComponent<Ball>();
        ballQueue.Enqueue(ball);
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
