using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class CustomAuthentication : CustomAPI
{
    public static CustomAuthentication Instance;
    private void Awake()
    {
        Instance = this;
        GetUserID();
    }

    public void LoadUserData()
    {
        SendGetRequest($"{GameConfig.ServerURL}/api/user/get?{SubURL}", OnGetUserSuccess);
    }

    private void OnGetUserSuccess(string responseData)
    {
        UserInfoData userInfoData = JsonConvert.DeserializeObject<UserInfoData>(responseData);
        GameDataManager.Instance.SetUserInfoData(userInfoData);
        PlayerPrefs.SetString(UserIDCache, userInfoData.UserId);
        LoadingManager.Instance.OnLoadingActionDone(LoadingDataLabel.LoadUserData.ToString());
    }


    private const string UserIDCache = "UserID";
    private string UserID;
    private string SubURL;
    /// <summary>
    /// Lấy UserID (đúng hơn là token cho user id + game id)
    /// </summary>
    /// <returns></returns>
    private void GetUserID()
    {
        if (PlayerPrefs.HasKey(UserIDCache))
        {
            UserID = PlayerPrefs.GetString(UserIDCache);
            SubURL = $"user_id={UserID}";
            return;
        }

#if UNITY_ANDROID
        UserID = "yasdsx145j";
        SubURL = $"android_id={UserID}";
        //SubURL = $"user_name={UserID}"; // TOAN TOAN TOAN
        return;
#endif
#if UNITY_IOS
        UserID = "iOS";
        SubURL = $"ios_id={UserID}";
#endif        

        UserID = "Editor";
        SubURL = $"user_id={UserID}";
        return;
    }


    public void Login()
    {
        UserLoginData requestData = new UserLoginData();
        requestData.Username = GameDataManager.Instance.UserData.UserId;
        requestData.Password = "";
        string data = JsonConvert.SerializeObject(requestData);
        SendPostRequest($"{GameConfig.ServerURL}/api/auth/login", data, OnLoginSuccess);
    }

    private void OnLoginSuccess(string responseData)
    {
        UserLoginResponse loginResponse = JsonConvert.DeserializeObject<UserLoginResponse>(responseData);
        GameConfig.Token = loginResponse.Token;
        GameConfig.PlayerID = loginResponse.UserId;
        LoadingManager.Instance.OnLoadingActionDone(LoadingDataLabel.Login.ToString());

    }
}

public class UserInfoData
{
    // TOAN TOAN TOAN
    // STRING NULLABLE?
    [JsonProperty("user_id")]
    public string UserId;

    // TOAN TOAN TOAN
    // NÓI HOÀNG THÊM USERNAME VÀO
    [JsonProperty("user_name")]
    public string UserName;

    [JsonProperty("android_id")]
    public string AndroidId;

    [JsonProperty("user_avatar")]
    public int? UserAvatar;

    [JsonProperty("user_frame")]
    public int? UserFrame;
}

public class UserLoginData
{
    [JsonProperty("username")]
    public string Username;

    [JsonProperty("password")]
    public string Password;
}

public class UserLoginResponse
{
    [JsonProperty("token")]
    public string Token;

    [JsonProperty("user_id")]
    public string UserId;
}
