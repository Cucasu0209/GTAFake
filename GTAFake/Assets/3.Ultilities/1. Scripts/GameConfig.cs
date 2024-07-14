using EnhancedScrollerDemos.Chat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Reflection;
using UnityEngine;
using UnityEngine.XR;
using static Unity.Burst.Intrinsics.X86.Avx;

public static class GameConfig
{
    public const string LOADING_SCENE = "LoadingScene";
    public const string HOME_SCENE = "HomeScene";
    public const string GAME_PLAY_SCENE = "GamePlayScene";
    public const string BATTLE_ROYALE_SCENE = "Gameplay";
    public const string CAMPAIGN_SCENE = "ChapterScene";
    public const string Version = "1.0.0.0";


    #region SERVER CONFIG
    public const string ServerURL = "https://game-data-gta-rzurl5kesa-as.a.run.app";
    public const string PackageName = "com.falcon.p.fun.tankio";
    public static string Token = "";

    public static string PlayerID = "";
    #endregion SERVER CONFIG

    public static float SpawnWeightMissionEasy = 30;
    public static float SpawnWeightMissionNormal = 60;
    public static float SpawnWeightMissionHard = 10;

    public static string MissionInfo00 = "Kill 5000 Zombie ";
    //"\nNhiệm vụ hoàn thành khi giết dc hết zombie, các zombie được spawn đến 5000 con thì dừng, bắt buộc spawn 2-3 con boss tùy vào việc player giết boss nhanh hay chậm " +
    //"\nNhiệm vụ fail khi chết / hết giờ " +
    //"\nThời gian mission là 5'";
    public static string MissionInfo10 = "Find a survivor and lead them to your camp";
    //"\nNhiệm vụ hoàn thành khi tìm dc survivor, dẫn đường về camp của mình(đến tòa nhà đầu tiên xuất phát) trong thời gian cho phép" +
    //"\nNhiệm vụ fail khi chết / để cho survivor bị chết" +
    //"\nThời gian của mission là 5'";
    public static string MissionInfo20 = "Find wrench for the mechanic";
    //"\nNhiệm vụ hoàn thành khi tìm được cờ lê, spawn cờ lê ở 1 góc đối diện của map" +
    //"\nNhiệm vụ fail khi chết / hết giờ" +
    //"\n(có 3' để chơi mission)";
    public static string MissionInfo21 = "Find new shetler";
    //"\nNhiệm vụ hoàn thành khi tìm được nơi trú ẩn mới Spawn cửa vào nơi trú ẩn mới ở 1 góc của map, người chơi ở 1 góc(ko cần đối diện)" +
    //"\nNhiệm vụ fail khi chết / hết giờ" +
    //"\n(có 3' để chơi mission)";
    public static string MissionInfo22 = "Find the antidote";
    //"Nhiệm vụ hoàn thành khi tìm được thuốc giải(thuốc cho zombie về lại thành người) Spawn xa người chơi" +
    //"\nNhiệm vụ fail khi chết / hết giờ" +
    //"\n(có 3' để chơi mission)";

    public static string ZombieKilledDes = "Kill 10000 zombie";
    public static string BossKilledDes = "Kill 50 boss";
    public static string AliveInSurvivalDes = "Alive in survival mode 300 second(s)";
    public static string HardMissionCompleteTimesDes = "Complete mission mode 1 time(s) - Hard Difficulty";
    public static string RerollMissionDes = "Reroll mission 1 time(s)";
    public static string GetStreakDes = "Get 100 streak";


    public static int ZombieKilledTarget = 1000;
    public static int BossKilledTarget = 50;
    public static int AliveInSurvivalTarget = 300;
    public static int HardMissionCompleteTimesTarget = 1;
    public static int RerollMissionTarget = 1;
    public static int GetStreakTarget = 100;

    public static int ZombieKilledBaseReward = 5000;
    public static int BossKilledBaseReward = 5;
    public static int AliveInSurvivalBaseReward = 3;
    public static int HardMissionCompleteTimesBaseReward = 3;
    public static int RerollMissionBaseReward = 5;
    public static int GetStreakBaseReward = 5;

}
