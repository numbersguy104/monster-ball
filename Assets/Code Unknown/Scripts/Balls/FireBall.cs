using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : AbstractAbilityBall
{
    [SerializeField] GameObject firePrefab;

    //Ability variables
    //How long the ball will leave the fire trail for on activation, in seconds
    [SerializeField] private float LEAVE_DURATION = 10.0f;

    //How long the fire trail itself will last after placement, in seconds
    [SerializeField] private float TRAIL_DURATION = 5.0f;

    //How often the fire trail will damage enemies, in seconds
    //(example: a value of 0.5 is once every 0.5 seconds, or equivalently twice per second)
    [SerializeField] private float DAMAGE_FREQUENCY = 0.25f;

    //Scale of the fire trail, relative to the ball
    [SerializeField] private float TRAIL_SIZE = 1.0f;


    //How long the ball's ability is currently active for, in seconds
    private float durationTimer = 0.0f;

    //Deal damage from the fire trail after this many seconds
    private float damageTimer = 0.0f;

    //Time in seconds between placing instances of the fire trail when ability is active
    //This is to avoid placing one every frame and cluttering the game
    private const float FIRE_GAP = 0.05f;

    //Tracks time for the above
    private float fireGapTimer = 0.0f;

    //List of fire trail object instances created by this ball
    List<GameObject> trailInstances = new List<GameObject>();

    protected override void Skill()
    {
        durationTimer = LEAVE_DURATION;
        damageTimer = DAMAGE_FREQUENCY;
    }

    protected override void Awake()
    {
        BallInit("Fireball");
        TRAIL_DURATION *= GameStatsManager.Instance.Fireball_TrailDurationMulti;
        TRAIL_SIZE *= GameStatsManager.Instance.Fireball_TrailSize;
        LEAVE_DURATION *= GameStatsManager.Instance.Fireball_LeaveDuration;
    }

    protected override void Update()
    {
        base.Update();

        //Remove trail instances from the list if they have Destroyed themselves
        trailInstances.RemoveAll(x => x == null);

        if (durationTimer > 0.0f)
        {
            durationTimer -= Time.deltaTime;

            if (fireGapTimer <= 0.0f)
            {
                GameObject trail = Instantiate(firePrefab, transform.position, transform.rotation);
                trail.transform.up = GetVelocity();
                float size = transform.lossyScale.x * TRAIL_SIZE;
                trail.transform.localScale = new Vector3(size, size, size);
                trail.GetComponent<FireTrail>().SetDuration(TRAIL_DURATION);
                trailInstances.Add(trail);
                fireGapTimer = FIRE_GAP;
            }
            else
            {
                fireGapTimer -= Time.deltaTime;
            }
        }

        if (trailInstances.Count > 0)
        {
            if (damageTimer <= 0.0f)
            {
                damageTimer = DAMAGE_FREQUENCY;

                List<MonsterController> trailMonsters = new List<MonsterController>();
                foreach (GameObject trail in trailInstances)
                {
                    List<MonsterController> instanceMonsters = trail.GetComponent<FireTrail>().GetMonsters();
                    foreach (MonsterController instanceMonster in instanceMonsters)
                    {
                        if (!(trailMonsters.Contains(instanceMonster)))
                        {
                            trailMonsters.Add(instanceMonster);
                        }
                    }
                }

                foreach (MonsterController monster in trailMonsters)
                {
                    DamageMonster(monster);
                }
            }
            else
            {
                damageTimer -= Time.deltaTime;
            }
        }
    }
}