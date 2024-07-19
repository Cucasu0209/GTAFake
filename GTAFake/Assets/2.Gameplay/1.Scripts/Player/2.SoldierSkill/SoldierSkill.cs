using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class SoldierSkill : PlayerSkill
{
    public int SkillDmg = 2;
    public float fireGap = 0.05f;
    public GameObject Bullet;
    public GameObject Minigun;
    public Transform[] HeadGuns;
    public override void PlaySkill()
    {
        if (IsPlayingSkill == false)
        {
            base.PlaySkill();


            Minigun.SetActive(true);
            Jumping.SetCanJump(false);
            WeaponManager.SetCanSwitchWeapon(false);
            WeaponManager.SetCanAttack(false);
            StopAllCoroutines();
            StartCoroutine(IAttack());
        }
        else
        {
            Controller.EndSkill();
            base.OnEndSkill();
            Minigun.SetActive(false);

            StopAllCoroutines();

            Jumping.SetCanJump(true);
            WeaponManager.SetCanSwitchWeapon(true);
            WeaponManager.SetCanAttack(true);
        }

    }
    IEnumerator IAttack()
    {
        int headInedx = 0;
        while (true)
        {
            GameObject newbu = LeanPool.Spawn(Bullet, HeadGuns[headInedx].position, Quaternion.identity);
            newbu.GetComponent<Bullet>().SetVelocity(transform.forward);
            newbu.GetComponent<Bullet>().SetDmg(SkillDmg);
            yield return new WaitForSeconds(fireGap);
            headInedx++;
            headInedx %= HeadGuns.Length;
        }
    }

}
