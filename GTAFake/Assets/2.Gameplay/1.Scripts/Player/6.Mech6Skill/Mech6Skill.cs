using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech6Skill : PlayerSkill
{
    private readonly int SkillDmg = 2;
    private readonly float fireGap = 0.2f;
    public override void PlaySkill()
    {
        if (IsPlayingSkill == false)
        {

            Controller.ActiveLayerSkill(true);
            Controller.PlaySkill();
            WeaponManager.HideWeapon();
            IsPlayingSkill = true;
            StopAllCoroutines();
            StartCoroutine(IAttack());
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
            StopAllCoroutines();

        }

    }
    IEnumerator IAttack()
    {
        while (true)
        {
            foreach (var enemy in PlayerTakeDmgSystem.Instance.GetEnemyInCircleArea(transform.position, 7))
            {
                enemy.TakeDmg(SkillDmg);
            }
            yield return new WaitForSeconds(fireGap);

        }
    }
}
