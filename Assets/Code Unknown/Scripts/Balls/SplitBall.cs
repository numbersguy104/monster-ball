using Unity.VisualScripting;
using UnityEngine;

public class SplitBall : AbstractAbilityBall
{
    //Number of balls this ball splits into
    [SerializeField] private int SPLIT_COUNT = 3;

    //Whether this ball already split or not
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
            //Create splitCount - 1 balls
            //The last ball is the one executing the split; it stays in the game
            for (int i = 1; i < SPLIT_COUNT; i++) {
                SplitBall newBall = Instantiate(this, transform.position, transform.rotation, transform.parent);
                newBall.PostSplit();
            }
        }

        PostSplit();
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
        SPLIT_COUNT = (int)(SPLIT_COUNT *GameStatsManager.Instance.Splitball_SplitCount);
        
        gameObject.transform.localScale = gameObject.transform.localScale * GameStatsManager.Instance.Splitball_BallSize;
    }
}
