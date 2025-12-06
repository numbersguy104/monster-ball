using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BallCounter : MonoBehaviour
{
    float nudgeTimer = 0.0f;

    public int CountActive()
    {
        return ballsActive.Count;
    }

    public int CountTotal()
    {
        return ballsActive.Count;
    }

    private List<AbstractBall> ballsActive;
    private List<AbstractBall> ballsTotal;

    private void Awake()
    {
        ballsActive = new List<AbstractBall>();
        ballsTotal = new List<AbstractBall>();
    }

    private void OnTriggerEnter(Collider other)
    {
        AbstractBall ball = other.gameObject.GetComponent<AbstractBall>();
        if (ball != null)
        {
            ballsActive.Add(ball);

            if (!ballsTotal.Contains(ball))
            {
                ballsTotal.Add(other.gameObject.GetComponent<AbstractBall>());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        AbstractBall ball = other.gameObject.GetComponent<AbstractBall>();
        if (ball != null)
        {
            ballsActive.Remove(ball);
        }
    }

    //Pressing T adds random velocity to get balls unstuck
    private void Update()
    {
        if (nudgeTimer < Mathf.Epsilon)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                foreach (AbstractBall b in ballsActive)
                {
                    b.AddVelocity(Random.value, Random.value, Random.value);
                }
                nudgeTimer = 5.0f;
            }
        }
        else
        {
            nudgeTimer -= Time.deltaTime;
        }
    }
}
