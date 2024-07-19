using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech5Skill : PlayerSkill
{
    public GameObject Fire;
    public Transform HeadGun;
    private readonly int SkillDmg = 1;
    private readonly float fireGap = 0.1f;
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
            Fire.SetActive(true);
        }
        else
        {
            Controller.EndSkill();
            Controller.ActiveLayerSkill(false);
            WeaponManager.ShowWeapon();
            IsPlayingSkill = false;
            WeaponManager.SetCanSwitchWeapon(true);
            WeaponManager.SetCanAttack(true);
            Fire.SetActive(false);
            StopAllCoroutines();

        }

    }
    IEnumerator IAttack()
    {
        while (true)
        {
            PlayerTakeDmgSystem.Instance.TakeDmgInCircleArea(HeadGun.position, 6, SkillDmg);
            yield return new WaitForSeconds(fireGap);

        }
    }
}
