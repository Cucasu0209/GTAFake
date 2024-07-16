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


    private float LastTimeFire = -1;
    public override void Attack(Transform character)
    {
        base.Attack(character);
        if (Time.time - LastTimeFire > Data.Duration)
        {
            LastTimeFire = Time.time;
            GameObject newbu = LeanPool.Spawn(Bullet, HeadGun.position, Quaternion.identity);
            newbu.GetComponent<Bullet>().SetVelocity(character.forward);
            newbu.GetComponent<Bullet>().SetDmg(Data.BaseDmg);
        }

    }
    public override void StopAttack(Transform character)
    {
        base.StopAttack(character);

    }
}
