
using Newtonsoft.Json;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionInfoSystem : CustomAPI
{
    public MissionInfo_Item[] Missions;
    public Button ButtonReroll;
    public string KeyMissionInfo = "MISSION_INFO_DATA";
    public GameObject ConfirmPopup;
    public Button btnCloseConfirmPopup;
    public Button BtnRerollByAds;
    public Button btnRerollByGold;
    public TextMeshProUGUI randomAdsRemaining;

    private void Start()
    {
        GetMissionInfo();
        ButtonReroll.onClick.AddListener(OpenConfirmPopup);
        BtnRerollByAds.onClick.AddListener(RerollByAds);
        btnRerollByGold.onClick.AddListener(RerollByGold);
        btnCloseConfirmPopup.onClick.AddListener(CloseConfirmPopup);
    }
    private void OpenConfirmPopup()
    {
        ConfirmPopup.gameObject.SetActive(true);
    }
    private void CloseConfirmPopup()
    {
        ConfirmPopup.gameObject.SetActive(false);
    }
    private void RerollByAds()
    {
        string data = JsonConvert.SerializeObject(new PackageInfo()
        {
            PackageName = "",
            ProductId = "",
            PurchaseToken = ""
        }
                  , Formatting.Indented, new JsonSerializerSettings
                  {
                      NullValueHandling = NullValueHandling.Ignore
                  });

        SendPostRequest($"{GameConfig.ServerURL}/api/game/random_mission_by_ads?id={GameDataManager.Instance.UserData.UserId}", data, UpdateMissionInfoUI);
    }
    private void RerollByGold()
    {
        string data = JsonConvert.SerializeObject(new PackageInfo()
        {
            PackageName = "",
            ProductId = "",
            PurchaseToken = ""
        }
                  , Formatting.Indented, new JsonSerializerSettings
                  {
                      NullValueHandling = NullValueHandling.Ignore
                  });

        SendPostRequest($"{GameConfig.ServerURL}/api/game/random_mission_by_gold?id={GameDataManager.Instance.UserData.UserId}", data, UpdateMissionInfoUI);
    }
    private void GetMissionData()
    {
        SendGetRequest($"{GameConfig.ServerURL}/api/game/mission_data?user_id={GameDataManager.Instance.UserData.UserId}", UpdateMissionInfoUI);
    }
    public void GetMissionInfo()
    {
        if (PlayerPrefs.HasKey(KeyMissionInfo) == false)
        {
            RerollByAds();
        }
        else
        {
            UpdateMissionInfoUI(PlayerPrefs.GetString(KeyMissionInfo));
        }

    }

    private void UpdateMissionInfoUI(string responseData)
    {
        CloseConfirmPopup();
        PlayerPrefs.SetString(KeyMissionInfo, responseData);
        Debug.LogError(responseData);
        MissionsResponse missions = JsonConvert.DeserializeObject<MissionsResponse>(responseData);
        randomAdsRemaining.SetText(missions.RandomRemaining.ToString());
        for (int i = 0; i < Missions.Length; i++)
        {
            MissionType type = (MissionType)(int.Parse(missions.MissionInfo[i].Type.Split("-")[0]));
            int contentIndex = (int.Parse(missions.MissionInfo[i].Type.Split("-")[1]));
            MissionDifficulty difficulty = (MissionDifficulty)(missions.MissionInfo[i].Difficulty - 1);
            Missions[i].SetContent(type, contentIndex);
            Missions[i].SetDifficulty(difficulty);
            Missions[i].UpdateReward(missions.MissionInfo[i].Rewards);
        }
    }

}

public enum MissionType
{
    Hunt = 0,
    Rescue = 1,
    FindItem = 2,
}
public enum MissionDifficulty
{
    Easy = 0,
    Normal = 1,
    Hard = 2
}

public class PackageInfo
{
    [JsonProperty("package_name")]
    public string PackageName;

    [JsonProperty("product_id")]
    public string ProductId;

    [JsonProperty("purchase_token")]
    public string PurchaseToken;
}

[Serializable]
public class MissionsResponse
{
    [JsonProperty("mission_info")]
    public MissionInfo[] MissionInfo;
    [JsonProperty("random_remaining")]
    public int RandomRemaining;


}
[Serializable]
public class MissionInfo
{
    [JsonProperty("type")]
    public string Type;

    [JsonProperty("difficulty")]
    public int Difficulty;

    [JsonProperty("modifier")]
    public int Modifier;

    [JsonProperty("rewards")]
    public MissionRewards Rewards;
}
[Serializable]
public class MissionRewards
{

    [JsonProperty("steel")] public int Steel;
    [JsonProperty("uranium")] public int Uranium;
    [JsonProperty("gold")] public int Gold;
    [JsonProperty("coal")] public int Coal;
    [JsonProperty("gunpowder")] public int Gunpowder;
    [JsonProperty("vitamin")] public int Vitamin;
}
public enum MissionRewardType
{
    Steel, Uranium, Gold, Coal, Gunpowder, Vitamin,
}