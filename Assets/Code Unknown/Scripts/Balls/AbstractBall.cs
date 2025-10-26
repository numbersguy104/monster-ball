using cfg;
using UnityEngine;

public abstract class AbstractBall : MonoBehaviour
{
    //Ball ID; should match the ID column in the BallParam table
    protected string ballID;

    //Store the data tables
    protected Tables data;

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

    protected Rigidbody rb;
    protected Collider col;
    
    protected void SetStats(string id)
    {
        BallParam stats = data.TbBallParam.Get(id);
        baseDamage = stats.BallAttack;
        baseSpeed = stats.BallSpd;
        baseSize = stats.BallSize;
        baseCritDamage = stats.BallCritDmg;
        baseCritChance = stats.BallCritChance;
    }

    protected void DamageMonster(MonsterController monster)
    {
        if (monster == null) return;
        bool crit = false;
        if (monster != null) {
            float damage = GetDamage();

            float rand = Random.value;
            if (rand < GetCritChance())
            {
                crit = true;
                damage *= GetCritDamage();
                
            }
            monster.TakeDamage((int)damage,crit);
            crit = false;

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
        SetVelocity(vel);
    }

    //Add a velocity vector to the ball's existing velocity
    public void AddVelocity(Vector3 v)
    {
        rb.linearVelocity += v;
    }

    //Add velocity components to the ball's existing velocity
    public void AddVelocity(float x, float y, float z)
    {
        Vector3 v = new Vector3(x, y, z);
        AddVelocity(v);
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

    //Get the stat multiplier for a given amount of a certain upgrade
    //Example: How much should 3 damage upgrades multiply the ball's base damage?
    //upgradeName should be an upgrade type from the UpgradeParam data table
    private float GetUpgradeMult(string upgradeName, int upgradeCount)
    {
        //If no upgrades, don't change the stat (multiply by 1)
        if (upgradeCount == 0)
        {
            return 1.0f;
        }

        //Otherwise check the table
        UpgradeParam upgradeData = data.TbUpgradeParam.Get(upgradeCount);

        //No way to convert a string to a parameter here...
        //Must check each case individually
        switch (upgradeName)
        {
            case "UpgradeAtk":
                return upgradeData.UpgradeAtk;
            case "UpgradeSpd":
                return upgradeData.UpgradeSpd;
            case "UpgradeSize":
                return upgradeData.UpgradeSize;
            case "UpgradeCDmg":
                return upgradeData.UpgradeCDmg;
            case "UpgradeCChance":
                return upgradeData.UpgradeCChance;
            default:
                Debug.LogWarning("Warning: GetUpgradeMult was called with an invalid upgrade name!");
                return 1.0f;
        }
    }

    //Get the ball's stats after modifiers...
    //Damage
    public virtual float GetDamage()
    {
        return baseDamage * GetUpgradeMult("UpgradeAtk", upgradesDamage);
    }

    //Speed
    public virtual float GetSpeed()
    {
        return baseSpeed * GetUpgradeMult("UpgradeSpd", upgradesSpeed);
    }

    //Size
    public virtual float GetSize()
    {
        return baseSize * GetUpgradeMult("UpgradeSize", upgradesSize);
    }

    //Crit damage
    public virtual float GetCritDamage()
    {
        return baseCritDamage * GetUpgradeMult("UpgradeCDmg", upgradesCritDamage);
    }

    //Crit chance
    public virtual float GetCritChance()
    {
        return baseCritChance * GetUpgradeMult("UpgradeCChance", upgradesCritChance);
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

        
        var light = GameObject.Find("Spot Light");

        if (light == null)
        {
            return;
        }
        light.transform.position = new Vector3(
            transform.position.x,
            transform.position.y + 0.31f,
            transform.position.z - 0.1f
        );
        
    }

    protected virtual void BallInit(string id)
    {
        ballID = id;

        data = LubanTablesMgr.Instance.tables;

        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        SetStats(id);

        UpdateFriction();
        UpdateSize();
    }

    protected void BallSound()
    {
        //TODO
    }

    //When implementing Awake be sure to call BallInit(string)
    //where the string is the ball's ID in the BallParam table!
    //This sets up all of the ball's stats.
    protected abstract void Awake();

    //If overriding Update, make sure to call "base.Update()" somewhere!
    //This contains update logic shared by all pinballs.
    protected virtual void Update()
    {
        BallSound();

        Transform light = null;
        foreach (Transform t in transform)
        {
            if (t.GetComponent<Light>() != null)
            {
                light = t;
                break;
            }
        }
        if (light != null)
        {
            light.position = new Vector3(
                transform.position.x,
                transform.position.y + 0.31f,
                transform.position.z - 0.1f
            );
            light.rotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
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
