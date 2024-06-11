using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Data/Weapon", order = 0)]
public class WeaponData : ScriptableObject
{
    public WeaponType Type;
    public int WeaponIndex;
    public string Name;
    public string Description;
    public float BaseDmg;
    public float Duration;
    public int BulletCount;
    public int BulletMaxCount;
    public string LinkPrefab;
}
public enum WeaponType
{
    Melee,
    Pistol,
    Rifle,
    Special,
    None
}