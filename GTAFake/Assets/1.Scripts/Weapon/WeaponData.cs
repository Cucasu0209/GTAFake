using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Data/Weapon", order = 0)]
public class WeaponData : ScriptableObject
{
    public WeaponType Type;
    public string Name;
    public string Description;
    public BaseWeapon BaseWeapon;
    public float BaseDmg;
    public float Duration;
}
public enum WeaponType
{
    Melee,
    Light,
    Rifle,
    Special,
}