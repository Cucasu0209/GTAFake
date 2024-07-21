using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
            foreach (var enemy in PlayerTakeDmgSystem.Instance.GetEnemyInCircleArea(HeadGun.position, 6))
            {
                enemy.TakeDmg(SkillDmg);
            }
            yield return new WaitForSeconds(fireGap);

        }
    }
}
