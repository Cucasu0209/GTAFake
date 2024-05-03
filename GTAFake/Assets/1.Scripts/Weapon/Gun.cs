using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : BaseWeapon
{
    [Header("Attack")]
    public GameObject Bullet;
    public Transform HeadGun;

    public override void Attack(Transform character)
    {
        base.Attack(character);
        GameObject newbu = LeanPool.Spawn(Bullet, HeadGun.position, Quaternion.identity);
        newbu.GetComponent<Bullet>().SetVelocity(character.forward);
        newbu.GetComponent<Bullet>().SetDmg(Data.BaseDmg);
    }
}
