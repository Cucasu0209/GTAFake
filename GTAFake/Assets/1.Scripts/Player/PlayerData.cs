using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int GetCurrentWeaponIndex()
    {
        return 0;
    }
    public WeaponData GetCurrentWeaponData()
    {
        return new WeaponData() { Type = WeaponType.Melee };
    }
    public List<WeaponData> GetReverseWeapons()
    {
        return new List<WeaponData> { new WeaponData() };
    }
}
