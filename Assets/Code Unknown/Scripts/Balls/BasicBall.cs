using UnityEngine;

//Default ball with no special features.
public class BasicBall : AbstractBall
{
    protected override void Start()
    {
        SetStats(8.0f, 5.0f, 1.0f, 0.05f, 1.25f);
        BallInit();
    }
}
