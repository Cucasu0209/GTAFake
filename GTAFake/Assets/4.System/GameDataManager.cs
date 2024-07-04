using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using Sirenix.OdinInspector;

public class GameDataManager : CustomAPI
{
    public static GameDataManager Instance;

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

    #region USER DATA
    public void SetUserInfoData(UserInfoData userInfoData)
    {
        UserData = userInfoData;
    }

    public UserInfoData UserData { get; private set; }
    public void UpdateUserName(string userName)
    {
        UserInfoData updateData = new UserInfoData();
        updateData.UserName = userName;
        RequestUpdateData(updateData);
    }
    public void UpdateAvatar(int avatarIndex)
    {
        UserInfoData updateData = new UserInfoData();
        updateData.UserAvatar = avatarIndex;
        RequestUpdateData(updateData);
    }
    public void UpdateFrameIndex(int frameIndex)
    {
        UserInfoData updateData = new UserInfoData();
        updateData.UserFrame = frameIndex;
        RequestUpdateData(updateData);
    }

    private void RequestUpdateData(UserInfoData updateData)
    {
        string data = JsonConvert.SerializeObject(updateData, Formatting.Indented, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });

        SendPostRequest($"{GameConfig.ServerURL}/api/user/update?id={UserData.UserId}", data, UpdateUserInfo);
    }
    private void UpdateUserInfo(string responseData)
    {
        // TOAN TOAN TOAN
        // PHẦN NÀY CHƯA LÀM
        Debug.LogError(responseData);
    }
    #endregion USER DATA
    #region GAME DATA
    [HideInInspector] public GameData GameData;
    [HideInInspector] public Action<ItemType> OnChangeResourceValue;
    [HideInInspector] public Action<List<BaseItem>> OnShowClaimItemPanel;

    private List<BaseItem> ClaimItemList = new List<BaseItem>();
    public void ClaimItem(string serverResponseData, bool showClaimItemPanel = true)
    {
        Debug.LogError(serverResponseData);
        Dictionary<int, int> changeResourceDict = JsonConvert.DeserializeObject<Dictionary<int, int>>(serverResponseData);
        ClaimItemList.Clear();
        foreach (KeyValuePair<int, int> keyValuePair in changeResourceDict)
        {
            ClaimItemList.Add(new BaseItem((ItemType)keyValuePair.Key, keyValuePair.Value, SlotType.None));
            ClaimItem(ClaimItemList[ClaimItemList.Count - 1]);
        }
        if (showClaimItemPanel && OnShowClaimItemPanel != null)
        {
            OnShowClaimItemPanel(ClaimItemList);
        }
    }


    public void ClaimItem(BaseItem baseItem)
    {
        switch (baseItem.ItemType)
        {
            case ItemType.Gold:
                GameData.Gold += baseItem.Quantity;
                break;
            case ItemType.Platinum:
                if (baseItem.Quantity < 0)
                {
                }
                GameData.Gem += baseItem.Quantity;
                break;
            case ItemType.Exp:
                // TOAN TOAN TOAN
                Debug.LogError("chưa tăng exp");
                break;
            case ItemType.Key_Silver:
                GameData.Key_Silver += baseItem.Quantity;
                break;
            case ItemType.Key_Gold:
                GameData.Key_Golden += baseItem.Quantity;
                break;
            case ItemType.Card_Body:
                GameData.BodyCard += baseItem.Quantity;
                break;
            case ItemType.Card_Wheel:
                GameData.WheelCard += baseItem.Quantity;
                break;
            case ItemType.Card_Weapon:
                GameData.WeaponCard += baseItem.Quantity;
                break;
            case ItemType.Card_Character:
                GameData.CharacterCard += baseItem.Quantity;
                break;
            case ItemType.Fragment_Body:
                GameData.BodyFragment += baseItem.Quantity;
                break;
            case ItemType.Fragment_Wheel:
                GameData.WheelFragment += baseItem.Quantity;
                break;
            case ItemType.Fragment_Weapon:
                GameData.WeaponFragment += baseItem.Quantity;
                break;
            case ItemType.Fragment_Character:
                GameData.CharacterFragment += baseItem.Quantity;
                break;
        }

        if (OnChangeResourceValue != null)
        {
            OnChangeResourceValue(baseItem.ItemType);
        }
    }
    #endregion GAME DATA

    #region LOAD DATA
    [HideInInspector] public Action OnLoadGameDataFinish;

    [Button("LoadData")]
    public void LoadGameData()
    {
        SendGetRequest($"{GameConfig.ServerURL}/api/data/get?user_id={UserData.UserId}", OnGameDataResult);
    }
    public PlayerDataResult playerdataresult;
    private void OnGameDataResult(string responseData)
    {
        Debug.LogError(responseData);
        playerdataresult = JsonConvert.DeserializeObject<PlayerDataResult>(responseData);

        if (OnLoadGameDataFinish != null)
        {
            OnLoadGameDataFinish();
        }
    }

    [Button("LoadData daily mission")]
    public void LoadGameDatadailymission()
    {
        SendGetRequest($"{GameConfig.ServerURL}/api/data/daily_mission/get?user_id={UserData.UserId}", OnGameDatamisionResult);
    }
    private void OnGameDatamisionResult(string responseData)
    {
        Debug.LogError(responseData);

    }
    #endregion LOAD DATA
}

public class BaseItem
{
    public ItemType ItemType;
    /// <summary>
    /// Set ItemIndex trong trường hợp unlock
    /// </summary>
    public int Quantity;
    public SlotType SlotType;
    public BaseItem()
    {

    }

    public BaseItem(ItemType itemType, int quantity, SlotType slotType)
    {
        ItemType = itemType;
        Quantity = quantity;
        SlotType = slotType;
    }
    public BaseItem(BaseItemInt baseItemInt)
    {
        ItemType = (ItemType)baseItemInt.ItemType;
        Quantity = baseItemInt.Quantity;
        SlotType = (SlotType)baseItemInt.SlotType;
    }
}

public class BaseItemInt
{
    [JsonProperty("item_type")]
    public int ItemType;

    [JsonProperty("quantity")]
    public int Quantity;

    [JsonProperty("slot_type")]
    public int SlotType;
}

public enum ItemType
{
    Gold = 0,
    Platinum = 1, // gem
    Exp = 2,
    Key_Silver = 3, // hòm normal 80 gem - mở item được card + fragment
    Key_Gold = 4, // hòm excellent 300 gem - mở item card + fragment

    Card_Random = 5, // Card random, chỉ dùng để hiển thị
    Card_Body = 6, // thẻ xe
    Card_Wheel = 7, // thẻ bánh
    Card_Weapon = 8, // thẻ vũ khí
    Card_Character = 9, // thẻ nhân vật

    Fragment_Random = 10, // Fragment random, chỉ dùng để hiển thị
    Fragment_Body = 11, // thẻ xe
    Fragment_Wheel = 12, // thẻ bánh
    Fragment_Weapon = 13, // thẻ vũ khí
    Fragment_Character = 14, // thẻ nhân vật

    Gold_WinGame = 15, // GOLD THU ĐƯỢC TỪ WIN 1 GAME CAMPAIGN

    Medal = 16, // CHỈ ĐỂ HIỂN THỊ CHO CLAIM ITEM PANEL

    Body = 17, // CHỈ ĐỂ HIỂN THỊ CHO CLAIM ITEM PANEL
    Weapon = 18, // CHỈ ĐỂ HIỂN THỊ CHO CLAIM ITEM PANEL
    Wheel = 19, // CHỈ ĐỂ HIỂN THỊ CHO CLAIM ITEM PANEL
    Character = 20, // CHỈ ĐỂ HIỂN THỊ CHO CLAIM ITEM PANEL

}
public enum Rarity
{
    Normal = 0, // grey
    Good = 1, // green
    Rare = 2, // blue
    Excellent = 3, // purple
    Epic = 4, // yellow
    Heroic = 5, // red
}
public enum SlotType
{
    None = 0,
    Body = 1,
    Weapon = 2,
    Wheel = 3,
    Character = 4,
    Color = 5,
}

public enum PriceType
{
    Ad = 0,
    Platinum = 1,
    Gold = 2,
    Key = 3, // dùng chung các loại key (epic / excellent / better)
}

#region GAME DATA
public class GameData
{
    [JsonProperty("id")]
    public string ID;

    [JsonProperty("user_id")]
    public string UserID;

    [JsonProperty("user_name")]
    public string UserName;

    [JsonProperty("gold")]
    public int Gold;

    [JsonProperty("gem")]
    public int Gem;

    [JsonProperty("body_level")]
    public List<int> BodyLevel;

    [JsonProperty("body_grade")]
    public List<int> BodyGrade;

    [JsonProperty("body_card")]
    public int BodyCard;

    [JsonProperty("body_fragment")]
    public int BodyFragment;

    [JsonProperty("wheel_level")]
    public List<int> WheelLevel;

    [JsonProperty("wheel_grade")]
    public List<int> WheelGrade;

    [JsonProperty("wheel_card")]
    public int WheelCard;

    [JsonProperty("wheel_fragment")]
    public int WheelFragment;

    [JsonProperty("weapon_level")]
    public List<int> WeaponLevel;

    [JsonProperty("weapon_grade")]
    public List<int> WeaponGrade;

    [JsonProperty("weapon_card")]
    public int WeaponCard;

    [JsonProperty("weapon_fragment")]
    public int WeaponFragment;

    [JsonProperty("character_level")]
    public List<int> CharacterLevel;

    [JsonProperty("character_grade")]
    public List<int> CharacterGrade;

    [JsonProperty("character_card")]
    public int CharacterCard;

    [JsonProperty("character_fragment")]
    public int CharacterFragment;

    [JsonProperty("color")]
    public List<bool> Color;

    //[JsonProperty("use_index")]
    //public UseIndex UseIndex;

    [JsonProperty("gold_win_game")]
    public int GoldWinGame;

    [JsonProperty("key")]
    public int Key_Silver;

    [JsonProperty("epic_key")]
    public int Key_Golden;

    [JsonProperty("claim_daily_shop")]
    public Dictionary<int, ClaimDailyShopElement> ClaimDailyShop = new Dictionary<int, ClaimDailyShopElement>();

    [JsonProperty("gold_ads_claim_count")]
    public int GoldAdsClaimCount;

    [JsonProperty("gold_ads_claim_time")]
    public DateTime GoldAdsClaimTime;

    [JsonProperty("unlocked_chapter_index")]
    public int UnlockedChapterNo;

    [JsonProperty("claim_starter_pack")]
    public bool ClaimStarterPack;

    [JsonProperty("daily_reward")]
    public DailyReward DailyReward;

    [JsonProperty("monthly_card")]
    public MonthlyCard MonthlyCard;

    [JsonProperty("chest_open")]
    public ChestOpen ChestOpen;

    [JsonProperty("achievement_point")]
    public int AchievementPoint;


    [JsonProperty("battle_pass")]
    public BattlePass BattlePass;

    [JsonProperty("claim_daily_discount")]
    public DailyDiscountData DailyDiscountData = new DailyDiscountData();


}

//public class UseIndex
//{
//    [JsonProperty("body")]
//    public int Body;

//    [JsonProperty("wheel")]
//    public int Wheel;

//    [JsonProperty("weapon")]
//    public int Weapon;

//    [JsonProperty("character")]
//    public int Character;

//    [JsonProperty("color")]
//    public int Color;
//}
public class DailyReward
{
    [JsonProperty("last_time_claim")]
    public DateTime LastTimeClaim;

    /// <summary>
    /// Tổng số ngày đã claim
    /// </summary>
    [JsonProperty("claim_count")]
    public int ClaimCount;

    /// <summary>
    /// Tổng số ngày đã claim (đến 5 thì reset về 0)
    /// </summary>
    [JsonProperty("10_day_claim_count")]
    public int DayClaimCount_10;

    /// <summary>
    /// Tổng số ngày đã claim (đến 7 thì reset về 0)
    /// </summary>
    [JsonProperty("7_day_claim_count")]
    public int DayClaimCount_7;
}
public class MonthlyCard
{
    [JsonProperty("purchased")]
    public bool Purchased;

    [JsonProperty("expiration_date")]
    public DateTime ExpirationDate;

    [JsonProperty("last_claim_date")]
    public DateTime LastClaimDate;
}
public class ChestOpen
{
    [JsonProperty("chest_good_ads_open_time")]
    public DateTime LastTimeOpenChestSilver;

    [JsonProperty("chest_excellent_ads_open_time")]
    public DateTime LastTimeOpenChestGold;
}
public class BattlePass
{
    [JsonProperty("is_premium")]
    public bool IsPremium;

    [JsonProperty("expiration_date")]
    public DateTime ExpirationDate;

    [JsonProperty("medal")]
    public int Medal;

    [JsonProperty("level")]
    public int Level;

    [JsonProperty("claimed_index_free")]
    public int ClaimedFreeLevel;

    [JsonProperty("claimed_index_premium")]
    public int ClaimedPremiumLevel;

    [JsonProperty("claimed_index_extra")]
    public int ClaimedExtraLevel;

    [JsonProperty("session_number")]
    public int SessionNumber;

}

public class DailyDiscountData
{
    [JsonProperty("daily_pack")]
    public Dictionary<int, DailyDiscount_Date> DailyPack;

    [JsonProperty("weekly_pack")]
    public Dictionary<int, DailyDiscount_Date> WeeklyPack;

    [JsonProperty("monthly_pack")]
    public Dictionary<int, DailyDiscount_Date> MonthlyPack;

    [JsonProperty("total_days_purchased")]
    public int TotalDaysPurchased;

    [JsonProperty("total_days_purchased_claimed_index")]
    public int TotalDaysPurchasedClaimedIndex;
}

public class DailyDiscount_Date
{
    [JsonProperty("date")]
    public DateTime Date;
}



//public class ClaimDailyShop
//{
//    public Dictionary<int, ClaimDailyShopElement> 
//}
public class ClaimDailyShopElement
{
    [JsonProperty("date")]
    public DateTime Date;
    [JsonProperty("claim_count")]
    public int ClaimCount;
}
#endregion GAME DATA

public class PlayerDataResult
{
    [JsonProperty("id")]
    public string Id;

    [JsonProperty("user_id")]
    public string user_id;

    [JsonProperty("user_name")]
    public string user_name;

    [JsonProperty("claim_remove_ads")]
    public bool claim_remove_ads;
    [JsonProperty("claim_starter_pack")]
    public bool claim_starter_pack;
    [JsonProperty("mission_random_ads_count")]
    public int mission_random_ads_count;

}