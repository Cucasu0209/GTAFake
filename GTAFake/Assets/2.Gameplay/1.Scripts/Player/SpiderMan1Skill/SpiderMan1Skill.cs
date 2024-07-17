using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMan1Skill : PlayerSkill
{
    public override void PlaySkill()
    {
        if (IsPlayingSkill == false)
        {
            base.PlaySkill();
        }
        else
        {
            Controller.EndSkill();
            Movement.SetScaleSpeed(1);
            Movement.SetCanRun(false);
            IsPlayingSkill = false;
        }

    }
    public override void TakeDmg()
    {
        base.TakeDmg();
        Movement.SetScaleSpeed(2);
        Movement.SetCanRun(true);
    }
}
