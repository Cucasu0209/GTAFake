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

#region GAME DATA




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