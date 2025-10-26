using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BallCounter : MonoBehaviour
{
    public int countActive = 0;
    public int countTotal = 0;

    private List<AbstractBall> balls; //for playtest feature

    private void Awake()
    {
        balls = new List<AbstractBall>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<AbstractBall>() != null)
        {
            countActive++;
            countTotal++;

            balls.Add(other.gameObject.GetComponent<AbstractBall>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<AbstractBall>() != null)
        {
            countActive--;

            balls.Remove(other.gameObject.GetComponent<AbstractBall>());
        }
    }

    //PLAYTEST FEATURE: Add random velocity to get balls unstuck
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            foreach (AbstractBall b in balls)
            {
                b.AddVelocity(Random.value, Random.value, Random.value);
            }
        }
    }
}
