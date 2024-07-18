using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Gun : BaseWeapon
{
    [Header("Attack")]
    public GameObject Bullet;
    public Transform[] HeadGun;
    public Transform DefaultHeadGun;


    private float LastTimeFire = -1;
    public override void Attack(Transform character)
    {
        base.Attack(character);
        if (Time.time - LastTimeFire > Data.Duration)
        {
            for (int i = 0; i < HeadGun.Length; i++)
            {
                LastTimeFire = Time.time;
                GameObject newbu = LeanPool.Spawn(Bullet, HeadGun[i].position, Quaternion.identity);
                newbu.GetComponent<Bullet>().SetVelocity(character.forward);
                newbu.GetComponent<Bullet>().SetDmg(Data.BaseDmg);
            }

        }

    }
    public override void StopAttack(Transform character)
    {
        base.StopAttack(character);

    }
}
