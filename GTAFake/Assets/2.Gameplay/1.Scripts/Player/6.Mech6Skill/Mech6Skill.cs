using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech6Skill : PlayerSkill
{
    public override void PlaySkill()
    {
        if (IsPlayingSkill == false)
        {

            Controller.ActiveLayerSkill(true);
            Controller.PlaySkill();
            WeaponManager.HideWeapon();
            IsPlayingSkill = true;

            WeaponManager.SetCanSwitchWeapon(false);
            WeaponManager.SetCanAttack(false);
        }
        else
        {
            Controller.EndSkill();
            Controller.ActiveLayerSkill(false);
            WeaponManager.ShowWeapon();
            IsPlayingSkill = false;
            WeaponManager.SetCanSwitchWeapon(true);
            WeaponManager.SetCanAttack(true);
        }

    }
}
