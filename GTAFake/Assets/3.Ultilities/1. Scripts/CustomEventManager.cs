using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEventManager : MonoBehaviour
{
    public static CustomEventManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Pos + circle size
    /// </summary>
    //public Action<Vector2, float, Collider2D> OnDestructible;

    public Action<Vector2, List<Vector2Int>, PolygonCollider2D> OnDestructible;

    //Setting 
    public Action OnOpenSettingPopup;

    //Shop 
    public Action OnOpenShopPopup;
    public Action OnCheckAtiveNotiButtonShop;
    public Action OnSetDataShop;

    //StarterPack 
    public Action OnOpenPackPopup;
    public Action<bool> OnActiveFalseStarterPackButton;

    //RemoveAds x
    public Action OnOpenRemoveAdsPopup;
    public Action<bool> OnActiveFalseRemoveAdsButton;

    //Quit game 
    public Action OnOpenQuitGamePopup;

    //Box Coin
    public Action<int> OnChangeCoinText;
    public Action<Vector2> OnCoinEfect;

    //Box Rank
    public Action<bool> OnSetAcitiveBoxRank;

    //Garage
    public Action OnOpenGarage;
    public Action OnLoadGarageHome;
    public Action OnActiveFalseAllButton;
    public Action<int> OnSetActiveBuyButton;
    public Action<int> OnSetActiveUpgardeButton;

    public Action OnCheckNotiUpgradeBody;
    public Action OnCheckNotiUpgradeWeapon;
    public Action OnCheckNotiUpgradeWheel;
    public Action LoadDataTextBody;

    //GarageBody
    public Action<int> OnSetActiveUpgardeButtonGarageBody;
    public Action<int> OnSetActiveBuyButtonGarageBody;
    public Action OnActiveFalseAllButtonGarageBody;
    public Action<int> OnSetLevelTextInPropertiesBonusGarageBody;
    public Action<string, int, bool,int> OnChangePropertiesBodyBonus;
    public Action<int, int, float, bool, int> OnEquipBodyItem;
    public Action<int> OnSetCoinUpgradeGarageBody;

    public Action<bool> OnSetMaxLevelBodyItem;
    public Action<int> OnSetUseBodyItemSprite;
    public Action<int> OnSetSelectBodyItemSprite;
    public Action<int, int> OnSetLevelTextBody;
    public Action<int> OnSetActiveBodyPriceText;
    public Action<int> OnUnlockBodyItem;

    public Action<bool, int> OnActiveInstructionUnlockBoxBody;

    //GarageWeapon
    public Action<int> OnSetActiveUpgardeButtonGarageWeapon;
    public Action<int> OnSetActiveBuyButtonGarageWeapon;
    public Action OnActiveFalseAllButtonGarageWeapon;
    public Action<int> OnSetLevelTextInPropertiesBonusGarageWeapon;
    public Action<string, int, bool, int> OnChangePropertiesWeaponBonus;
    public Action<int, float,float,bool,int> OnEquipWeaponItem;
    public Action<int> OnSetCoinUpgradeGarageWeapon;

    public Action<bool> OnSetMaxLevelWeaponItem;
    public Action<int> OnSetUseWeaponItemSprite;
    public Action<int> OnSetSelectWeaponItemSprite;
    public Action<int, int> OnSetLevelTextWeapon;
    public Action<int> OnSetActiveWeaponPriceText;

    public Action<bool, int> OnActiveInstructionUnlockBoxWeapon;

    //GarageWheel
    public Action<int> OnSetActiveUpgardeButtonGarageWheel;
    public Action<int> OnSetActiveBuyButtonGarageWheel;
    public Action OnActiveFalseAllButtonGarageWheel;
    public Action<int> OnSetLevelTextInPropertiesBonusGarageWheel;
    public Action<string> OnBuyWheelItem;
    public Action<string> OnUpgradeWheelItem;
    public Action<string, float, bool, int> OnChangePropertiesWheelBonus;
    public Action<float, bool,float> OnEquipWheelItem;
    public Action<int> OnSetCoinUpgradeGarageWheel;

    public Action<bool> OnSetMaxLevelWheelItem;
    public Action<int> OnSetSelectWheelItemSprite;
    public Action<int> OnSetUseWheelItemSprite;
    public Action<int, int> OnSetLevelTextWheel;
    public Action<int> OnSetActiveWheelPriceText;

    public Action<bool, int> OnActiveInstructionUnlockBoxWheel;

    //GarageCharacter
    public Action<bool> OnSetActiveUnlockCharacterWithAdsButton;
    public Action<int> OnSetItemNameGarageCharacter;
    public Action<int> OnUnlockCharacterItem;
    public Action<int> OnSetUseCharacterItemSprite;
    public Action<int> OnSetSelectCharacterItemSprite;

    //GarageColor
    public Action<bool> OnSetActiveUnlockColorWithAdsButton;
    public Action<int> OnSetItemNameGarageColor;
    public Action<int> OnUnlockColorItem;
    public Action<int> OnSetUseColorItemSprite;
    public Action<int> OnSetSelectColorItemSprite;

    //Tank prefab in home scene
    public Action<int> OnSetWeaponTankInHomeScene;
    public Action<int> OnSetWheelTankInHomeScene;
    public Action<int> OnSetBodyTank;
    public Action<int> OnSetColorTankInHomeScene;
    public Action<int> OnSetCharacterInHomeScene;
    public Action OnLoadTankInHomeScene;
    public Action OnMoveMidTankPrefab;
    public Action OnMoveLeftTankPrefab;
    public Action OnDespawnTankBody;
    public Action OnMoveUpTankPrefab;






}
