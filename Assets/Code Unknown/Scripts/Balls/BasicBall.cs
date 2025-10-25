using UnityEngine;

//Default ball with no special features.
public class BasicBall : AbstractBall
{
    protected override void Awake()
    {
        BallInit("Pinball");
    }
}
