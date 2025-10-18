using UnityEngine;

public abstract class AbstractBall : MonoBehaviour
{
    //Base stats; should be set for each ball by calling SetStats() within Start()
    protected float baseDamage; //Base damage when hitting a monster
    protected float baseSpeed; //Inverse of dynamic friction for this ball
    protected float baseSize; //Relative size of the ball
    protected float baseCritDamage; //Damage multiplier on a critical hit
    protected float baseCritChance; //Chance for a critical hit when dealing damage (0 - 1)

    //Number of upgrades to each stat
    protected int upgradesDamage = 0;
    protected int upgradesSpeed = 0;
    protected int upgradesSize = 0;
    protected int upgradesCritDamage = 0;
    protected int upgradesCritChance = 0;

    //Sound settings
    protected const float MIN_SOUND_SPEED = 0.5f; //Minimum speed for the rolling sound to play
    protected const float MAX_VOLUME = 1f; //Volume of the rolling sound
    protected const float VOLUME_FADE = 5f; // sound trasmitting speed

    public bool active;
    protected Rigidbody rb;
    protected Collider col;
    
    protected void SetStats(float damage, float speed, float size, float critDamage, float critChance)
    {
        baseDamage = damage;
        baseSpeed = speed;
        baseSize = size;
        baseCritDamage = critDamage;
        baseCritChance = critChance;
    }

    protected void DamageMonster(MonsterController monster)
    {
        if (monster != null) {
            float damage = GetDamage();

            float rand = Random.value;
            if (rand < GetCritChance())
            {
                damage *= GetCritDamage();
            }

            monster.TakeDamage((int)GetDamage(), GetVelocity().magnitude);
        }
    }

    //Set the ball's velocity to a given 3D vector
    public void SetVelocity(Vector3 v)
    {
        rb.linearVelocity = v;
    }

    //Set the ball's X, Y, and Z velocity individually
    //Accepts null for any of the 3 arguments, representing no change to velocity
    public void SetVelocity(float? x, float? y, float? z)
    {
        Vector3 vel = rb.linearVelocity;
        if (x.HasValue)
        {
            vel.x = x.Value;
        }
        if (y.HasValue)
        {
            vel.y = y.Value;
        }
        if (z.HasValue)
        {
            vel.z = z.Value;
        }
        rb.linearVelocity = vel;
    }

    //Add a velocity vector to the ball's existing velocity
    public void AddVelocity(Vector3 v)
    {
        rb.linearVelocity += v;
    }

    //Set the ball's speed (magnitude of its velocity) by scaling its velocity to match
    public void SetSpeed(float speed)
    {
        rb.linearVelocity = rb.linearVelocity.normalized * speed;
    }

    //Get the ball's velocity vector
    public Vector3 GetVelocity()
    {
        return rb.linearVelocity;
    }

    //Get the ball's stats after modifiers...
    //(TODO: Hook this up to the sheets)
    //Damage
    public virtual float GetDamage()
    {
        return baseDamage * (upgradesDamage + 1);
    }

    //Speed
    public virtual float GetSpeed()
    {
        return baseSpeed * (upgradesSpeed + 1);
    }

    //Size
    public virtual float GetSize()
    {
        return baseSize * (upgradesSize + 1);
    }

    //Crit damage
    public virtual float GetCritDamage()
    {
        return baseCritDamage * (upgradesCritDamage + 1);
    }

    //Crit chance
    public virtual float GetCritChance()
    {
        return baseCritChance * (upgradesCritChance + 1);
    }

    //Upgrade one of the ball's stats...
    //Damage
    public void UpgradeDamage()
    {
        upgradesDamage++;
    }

    //Speed
    public void UpgradeSpeed()
    {
        upgradesSpeed++;
        UpdateFriction();
    }

    //Update the ball's physical friction when its speed stat changes
    protected void UpdateFriction()
    {
        float s = GetSpeed();
        col.material.dynamicFriction = 1 / s;
    }

    //Size
    public void UpgradeSize()
    {
        upgradesSize++;
        UpdateSize();
    }

    //Update the ball's physical size when its size stat changes
    protected void UpdateSize()
    {
        float s = GetSize();
        transform.localScale = new Vector3(s, s, s);
    }

    //Crit damage
    public void UpgradeCritDamage()
    {
        upgradesCritDamage++;
    }

    //Crit chance
    public void UpgradeCritChance()
    {
        upgradesCritChance++;
    }

    //Activate the ball, bringing it into play
    public virtual void Activate()
    {
        BallLauncher launcher = FindAnyObjectByType<BallLauncher>();
        transform.position = launcher.transform.position;
        launcher.NewBall(this);

        var light = GameObject.Find("Spot Light");

        light.transform.position = new Vector3(
            transform.position.x,
            transform.position.y + 0.31f,
            transform.position.z - 0.1f
        );
    }

    protected void BallInit()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        UpdateFriction();
        UpdateSize();
    }

    protected void BallSound()
    {
        //TODO
    }

    //SetStats should be BEFORE BallInit!
    protected virtual void Start()
    {
        SetStats(8.0f, 5.0f, 2.0f, 0.05f, 1.25f);
        BallInit();
    }

    protected virtual void Update()
    {
        BallSound();

        //TODO: Replace this with something that works for multiple balls at once
        var light = GameObject.Find("Spot Light");
        if (light != null)
        {
            light.transform.position = new Vector3(
                transform.position.x,
                transform.position.y + 0.31f,
                transform.position.z - 0.1f
            );
        }
    }

    protected virtual void OnCollisionEnter(Collision col)
    {
        MonsterController mc = col.gameObject.GetComponent<MonsterController>();
        if (mc != null)
        {
            DamageMonster(mc);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        MonsterController mc = other.gameObject.GetComponent<MonsterController>();
        if (mc != null)
        {
            DamageMonster(mc);
        }
    }
}
