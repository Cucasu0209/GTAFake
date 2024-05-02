
using System.Runtime.InteropServices.WindowsRuntime;

public static class WeaponConfig
{
    public static string MeeleDataLink = "Weapon/Data/MeeleDemo";
    public static string PistalDataLink = "Weapon/Data/PistalDemo";
    public static string RifleDataLink = "Weapon/Data/RifleDemo";
    public static string SpecialWeaponDataLink = "Weapon/Data/SpecialDemo";

    public static string MeeleIcon = "Weapon/Icon/MeeleIcon";
    public static string PistolIcon = "Weapon/Icon/PistolIcon";
    public static string RifleIcon = "Weapon/Icon/RifleIcon";
    public static string SpecialIcon = "Weapon/Icon/SpecialIcon";
    public static string GetIconLink(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Melee: return MeeleIcon;
            case WeaponType.Pistol: return PistolIcon;
            case WeaponType.Rifle: return RifleIcon;
            case WeaponType.Special: return SpecialIcon;
        }
        return SpecialIcon;
    }

    public static string GetDataLink(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Melee: return MeeleDataLink;
            case WeaponType.Pistol: return PistalDataLink;
            case WeaponType.Rifle: return RifleDataLink;
            case WeaponType.Special: return SpecialWeaponDataLink;
        }
        return SpecialWeaponDataLink;
    }
}
