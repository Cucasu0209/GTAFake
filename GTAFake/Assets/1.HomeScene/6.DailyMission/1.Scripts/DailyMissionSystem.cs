using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;

public class DailyMissionSystem : CustomAPI
{
    public DailyMissionItem[] Items;
    void Start()
    {
        GetDailyMission();
    }
    public void GetDailyMission()
    {
        SendGetRequest($"{GameConfig.ServerURL}/api/data/daily_mission/get?user_id={GameDataManager.Instance.UserData.UserId}", OnGetDataResult);

    }

    public void OnGetDataResult(string result)
    {
        Debug.LogError(result);
        MissionDailyData missions = JsonConvert.DeserializeObject<MissionDailyData>(result);
        foreach (var item in Items)
        {
            int claim = 0;
            switch (item.Type)
            {
                case DailyMissionType.ZombieKilled:
                    claim = missions.ZombieKilled.ClaimState;

                    break;
                case DailyMissionType.BossKilled:
                    claim = missions.BossKilled.ClaimState;
                    break;
                case DailyMissionType.AliveInSurvival:
                    claim = missions.AliveInSurvival.ClaimState;
                    break;
                case DailyMissionType.HardMissionCompleteTimes:
                    claim = missions.HardMissionCompleteTimes.ClaimState;
                    break;
                case DailyMissionType.Reroll1Time:
                    claim = PlayerPrefs.GetInt(item.Type.ToString(), 0);
                    break;
                case DailyMissionType.GetStreak:
                    claim = missions.GetStreak.ClaimState;
                    break;
            }
            claim = PlayerPrefs.GetInt(item.Type.ToString(), 0);
            item.Setup(PlayerPrefs.GetInt(item.Type.ToString(), 0), PlayerPrefs.GetInt(item.Type.ToString() + "claim", 0));
            item.OnClickGoNow += CompleteDailyMission;
            item.OnClickClaim += ClaimDailyMission;
        }
    }
    public void CompleteDailyMission(DailyMissionType type)
    {
        switch (type)
        {
            case DailyMissionType.ZombieKilled:
                PlayerPrefs.SetInt(type.ToString(), GameConfig.ZombieKilledTarget);
                break;
            case DailyMissionType.BossKilled:
                PlayerPrefs.SetInt(type.ToString(), GameConfig.BossKilledTarget);
                break;
            case DailyMissionType.AliveInSurvival:
                PlayerPrefs.SetInt(type.ToString(), GameConfig.AliveInSurvivalTarget);
                break;
            case DailyMissionType.HardMissionCompleteTimes:
                PlayerPrefs.SetInt(type.ToString(), GameConfig.HardMissionCompleteTimesTarget);
                break;
            case DailyMissionType.Reroll1Time:
                PlayerPrefs.SetInt(type.ToString(), GameConfig.RerollMissionTarget);
                break;
            case DailyMissionType.GetStreak:
                PlayerPrefs.SetInt(type.ToString(), GameConfig.GetStreakTarget);
                break;
        }
        foreach (var item in Items) item.Setup(PlayerPrefs.GetInt(item.Type.ToString()));

        string data = JsonConvert.SerializeObject(new ClaimDailyMissionData()
        {
            MissionType = "zombie_killed"
        }
             , Formatting.Indented, new JsonSerializerSettings
             {
                 NullValueHandling = NullValueHandling.Ignore
             });

        SendPostRequest($"{GameConfig.ServerURL}/api/data/daily_mission/claim?id={GameDataManager.Instance.UserData.UserId}", data, OnClaimDataResult);
    }
    public void ClaimDailyMission(DailyMissionType type)
    {
        PlayerPrefs.SetInt(type.ToString() + "claim", 1);
        foreach (var item in Items) item.Setup(PlayerPrefs.GetInt(item.Type.ToString()), PlayerPrefs.GetInt(item.Type.ToString() + "claim", 0));

        string data = JsonConvert.SerializeObject(new ClaimDailyMissionData()
        {
            MissionType = "zombie_killed"
        }
             , Formatting.Indented, new JsonSerializerSettings
             {
                 NullValueHandling = NullValueHandling.Ignore
             });

        SendPostRequest($"{GameConfig.ServerURL}/api/data/daily_mission/claim?id={GameDataManager.Instance.UserData.UserId}", data, OnClaimDataResult);
    }
    public void OnClaimDataResult(string result)
    {
        Debug.LogError(result);
    }
}
public class ClaimDailyMissionData
{
    [JsonProperty("mission_type")] public string MissionType;
}
public class MissionDailyData
{
    [JsonProperty("alive_in_survival")] public SingleMissionDailyProgess AliveInSurvival;
    [JsonProperty("boss_killed")] public SingleMissionDailyProgess BossKilled;
    [JsonProperty("get_streak")] public SingleMissionDailyProgess GetStreak;
    [JsonProperty("hard_mission_complete_times")] public SingleMissionDailyProgess HardMissionCompleteTimes;
    [JsonProperty("zombie_killed")] public SingleMissionDailyProgess ZombieKilled;
}

[Serializable]
public class SingleMissionDailyProgess
{
    [JsonProperty("state")] public int State;
    [JsonProperty("claim_state")] public int ClaimState;
}

public enum DailyMissionType
{
    ZombieKilled,
    BossKilled,
    AliveInSurvival,
    HardMissionCompleteTimes,
    Reroll1Time,
    GetStreak,

}