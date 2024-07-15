using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Gun : BaseWeapon
{
    [Header("Attack")]
    public GameObject Bullet;
    public Transform HeadGun;

    private bool isFiring = false;
    IEnumerator IFire;
    public override void Attack(Transform character)
    {
        base.Attack(character);

        isFiring = true;
        if (IFire != null)
        {
            StopCoroutine(IFire);
        }
        IFire = Fire(character);
        StartCoroutine(IFire);
    }
    public override void StopAttack(Transform character)
    {
        base.StopAttack(character);
        StopCoroutine(IFire);
        isFiring = false;
    }
    public IEnumerator Fire(Transform character)
    {
        while (isFiring)
        {
            yield return new WaitForSeconds(Data.Duration);
            GameObject newbu = LeanPool.Spawn(Bullet, HeadGun.position, Quaternion.identity);
            newbu.GetComponent<Bullet>().SetVelocity(character.forward);
            newbu.GetComponent<Bullet>().SetDmg(Data.BaseDmg);
        }

    }
}
