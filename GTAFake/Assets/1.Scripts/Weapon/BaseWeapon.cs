using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    public WeaponData Data;
    public virtual  void StartAttack( Vector3 forward)
    {
    }
    public virtual void Damage(Transform parent, Vector3 forward)
    {

    }
}
