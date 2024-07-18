using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierSkill : PlayerSkill
{
    public override void PlaySkill()
    {
        if (IsPlayingSkill == false)
        {
            base.PlaySkill();
            Jumping.SetCanJump(false);
            WeaponManager.SetCanSwitchWeapon(false);
            WeaponManager.SetCanAttack(false);
        }
        else
        {
            Controller.EndSkill();
            base.OnEndSkill();

            Jumping.SetCanJump(true);
            WeaponManager.SetCanSwitchWeapon(true);
            WeaponManager.SetCanAttack(true);
        }

    }

}
