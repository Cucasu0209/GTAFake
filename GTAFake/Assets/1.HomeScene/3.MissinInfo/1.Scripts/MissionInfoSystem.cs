
using Newtonsoft.Json;
using System;
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
        string data = JsonConvert.SerializeObject(new RandomMissionByAds()
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
        string data = JsonConvert.SerializeObject(new RandomMissionByAds()
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

        for (int i = 0; i < Missions.Length; i++)
        {
            MissionType type = (MissionType)(missions.MissionInfo[i].Type - 1);
            int contentIndex = 0;
            MissionDifficulty difficulty = (MissionDifficulty)(missions.MissionInfo[i].Difficulty - 1);
            float randomWeight = UnityEngine.Random.Range(0, 100f);
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

public class RandomMissionByAds
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
}
[Serializable]
public class MissionInfo
{
    [JsonProperty("type")]
    public int Type;

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