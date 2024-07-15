using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    public WeaponData Data;
    public virtual void Attack(Transform character)
    {
        if (CheckRunoutOfBullet() == false)
        {
            Data.BulletCount--;
            GameManager.Instance?.OnPlayerFired?.Invoke();
        }

    }
    public virtual void StopAttack(Transform character)
    {
        
    }
    public virtual bool CheckRunoutOfBullet()
    {
        return Data.BulletCount <= 0 && Data.BulletMaxCount > 0;
    }
    public virtual void ReloadBullet()
    {
        Data.BulletCount = Data.BulletMaxCount;
        GameManager.Instance?.OnPlayerFired?.Invoke();
    }
}
