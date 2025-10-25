using Unity.VisualScripting;
using UnityEngine;

public class SplitBall : AbstractAbilityBall
{
    private bool split = false;

    //Create a random velocity vector from <-1, -1, -1> to <1, 1, 1>
    private Vector3 RandomVelocity()
    {
        return new Vector3(Random.value * 2 - 1, Random.value * 2 - 1, Random.value * 2 - 1);
    }

    protected override void Skill()
    {
        if (!split)
        {
            SplitBall newBall = Instantiate(this, transform.position, transform.rotation, transform.parent);

            PostSplit();
            newBall.PostSplit();
        }
    }

    public void PostSplit()
    {
        split = true;
        baseSize *= 0.8f;
        UpdateSize();
        AddVelocity(RandomVelocity());
    }

    protected override void Awake()
    {
        BallInit("Splitball");
    }
}
