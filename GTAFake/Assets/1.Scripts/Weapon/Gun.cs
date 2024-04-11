using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : BaseWeapon
{
    public BaseBullet Bullet;
    public override void StartAttack(Transform effectParent, Vector3 forward)
    {
        BaseBullet bullet = Instantiate(Bullet, effectParent.transform.position, Quaternion.identity);
        bullet.transform.parent = effectParent;
        bullet.SetDmg(Data.BaseDmg);
        bullet.Fly(forward);
    }
}
