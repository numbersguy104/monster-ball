using cfg;
using UnityEngine;

public abstract class AbstractAbilityBall : AbstractBall
{
    //Ability ID
    protected string skillID;

    //Number of points this ball must gain to activate its ability
    protected long skillReq;

    //Progress this ball has made to activating its skill, in points
    //Once it reaches skillReq, that amount is subtracted
    protected long skillProgress = 0;

    //Add code for the ball's skill here when implementing.
    protected abstract void Skill();

    //Add points to this ball's skill progress
    public void AddSkillPoints(long amount)
    {
        skillProgress += amount;
    }

    protected override void BallInit(string id)
    {
        base.BallInit(id);

        BallParam abilityData = data.TbBallParam.Get(id);

        skillReq = abilityData.BallSkillReq;
        skillID = abilityData.BallSkillID;

        if (id == "Splitball")
        {
            skillReq *= (long)GameStatsManager.Instance.Splitball_SkillReq;
        }
        if (id == "Iceball")
        {
            skillReq *= (long)GameStatsManager.Instance.Iceball_BallSkillReq;
        }
        if (id == "Lightningball")
        {
            skillReq *= (long)GameStatsManager.Instance.Lightningball_BallSkillReq;
        }
    }

    //If overriding Update, make sure to call "base.Update()" somewhere!
    //This contains the logic for handling abilities,
    //and also (through extending AbstractBall) the logic shared by all pinballs.
    protected override void Update()
    {
        base.Update();

        if (skillProgress > skillReq) {
            Skill();
            skillProgress -= skillReq;
        }
    }
}
