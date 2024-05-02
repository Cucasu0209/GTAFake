
using System.Runtime.InteropServices.WindowsRuntime;

public static class WeaponConfig
{
    public static string GunLink = "Weapon/Data/SimpleGun";
    public static string AxeLink = "Weapon/Data/Axe";
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
}
