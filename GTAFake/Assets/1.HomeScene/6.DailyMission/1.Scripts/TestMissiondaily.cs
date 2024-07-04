using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMissiondaily : CustomAPI
{
    public int MissionIndex;
    public int zombie_killed;
    public int boss_killed;
    public int highest_streak;
    public int score;
    public int play_time;
    [Button("POST Play GameMode")]

    public void PlayMission()
    {
        string data = JsonConvert.SerializeObject(new indexClass()
        {
            mode = "survival",
        },
         Formatting.Indented, new JsonSerializerSettings
         {
             NullValueHandling = NullValueHandling.Ignore
         });

        SendPostRequest($"{GameConfig.ServerURL}/api/game/play_mode?id={GameDataManager.Instance.UserData.UserId}", data, OnrecieveData);
    }

    [Button("POST Play Mission")]

    public void PlayMissions()
    {
        string data = JsonConvert.SerializeObject(new indexClassa()
        {
            index = MissionIndex,
        },
         Formatting.Indented, new JsonSerializerSettings
         {
             NullValueHandling = NullValueHandling.Ignore
         });

        SendPostRequest($"{GameConfig.ServerURL}/api/game/play_mission?id={GameDataManager.Instance.UserData.UserId}", data, OnrecieveData);
    }


    [Button("POST Data endgame")]
    public void PostendGame()
    {

        string data = JsonConvert.SerializeObject(new EndgameData()
        {
            zombie_killed = zombie_killed,
            boss_killed = boss_killed,
            highest_streak = highest_streak,
            score = score,
            play_time = play_time,
        },
            Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

        SendPostRequest($"{GameConfig.ServerURL}/api/game/end?id={GameDataManager.Instance.UserData.UserId}", data, OnrecieveData);
    }
    public void OnrecieveData(string data)
    {
        Debug.LogError(data);
    }
}
public class EndgameData
{
    public int zombie_killed;
    public int boss_killed;
    public int highest_streak;
    public int score;
    public int play_time;
}
public class indexClass
{
    public string mode;
}

public class indexClassa
{
    public int index;
}