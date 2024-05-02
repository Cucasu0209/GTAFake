using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public static WeaponData CurrentWeapon = null;
    public static int GetCurrentWeaponIndex()
    {
        return 0;
    }
    public static WeaponData GetCurrentWeaponData()
    {
        return CurrentWeapon;
    }
    public static List<WeaponData> GetReverseWeapons()
    {
        return new List<WeaponData> { new WeaponData() };
    }

    public static void SetCurrentWeaponData(WeaponData data)
    {
        CurrentWeapon = data;
    }
}
