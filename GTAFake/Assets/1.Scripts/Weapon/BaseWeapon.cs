using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    public WeaponData Data;
    public virtual void Attack(Transform character)
    {
        Data.BulletCount--;
        GameManager.Instance?.OnPlayerFired?.Invoke();
    }
}
