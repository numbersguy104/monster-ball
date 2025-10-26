using System.Collections.Generic;
using UnityEngine;

public class IceBall : AbstractAbilityBall
{
    [SerializeField] IceTrail iceTrailPrefab;
    //Ability variables
    //How long the ball will leave the ice trail for on activation, in seconds
    private const float LEAVE_DURATION = 10.0f;
    //How long the ice trail itself will last after placement, in seconds
    private const float TRAIL_DURATION = 5.0f;
    //Scale of the ice trail, relative to the ball
    private const float TRAIL_SIZE = 1.0f;

    //How long the ball's ability is currently active for, in seconds
    private float durationTimer = 0.0f;

    //Time in seconds between placing instances of the fire trail when ability is active
    //This is to avoid placing one every frame and cluttering the game
    private const float ICE_GAP = 0.05f;

    //Tracks time for the above
    private float iceGapTimer = 0.0f;

    //List of fire trail object instances created by this ball
    List<IceTrail> trailInstances = new List<IceTrail>();

    protected override void Skill()
    {
        durationTimer = LEAVE_DURATION;
    }

    protected override void Awake()
    {
        BallInit("Iceball");
    }

    protected override void Update()
    {
        base.Update();

        //Remove trail instances from the list if they have Destroyed themselves
        trailInstances.RemoveAll(x => x == null);

        if (durationTimer > 0.0f)
        {
            durationTimer -= Time.deltaTime;

            if (iceGapTimer <= 0.0f)
            {
                CreateTrail();
                iceGapTimer += ICE_GAP;
            }
            else
            {
                iceGapTimer -= Time.deltaTime;
            }
        }
    }

    private void CreateTrail()
    {
        var trail = Instantiate(iceTrailPrefab, transform.position, transform.rotation);
        trail.transform.up = GetVelocity();
        float size = transform.lossyScale.x * TRAIL_SIZE;
        trail.transform.localScale = new Vector3(size, size, size);
        trail.GetComponent<IceTrail>().SetDuration(TRAIL_DURATION);
        trailInstances.Add(trail);
    }

}
