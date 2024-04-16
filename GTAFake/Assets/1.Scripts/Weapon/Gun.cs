using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : BaseWeapon
{
    [Header("Attack")]
    public GameObject Bullet;
    public Transform HeadGun;

    public override void StartAttack(Vector3 forward)
    {
        GameObject newbu = LeanPool.Spawn(Bullet, HeadGun.position, Quaternion.identity);
        newbu.GetComponent<Bullet>().SetVelocity(forward);
    }
}
