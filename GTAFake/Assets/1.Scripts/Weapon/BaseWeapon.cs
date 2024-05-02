using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    public WeaponData Data;
    public virtual  void StartAttack(Transform character)
    {
    }
    public virtual void Damage(Transform parent, Vector3 forward)
    {

    }
}
