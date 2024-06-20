using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System.Globalization;
using DG.Tweening;
using System.Linq;


public enum GameMode
{
    Chapter = 0,
    BatleRoyale = 1,
}

public enum DailyChallengeUpside
{
    WeaponUpToLegend = 0, // vũ khí lên đỏ hoặc +50% dmg
    BodyArmourUpToLegend = 1, // áo lên đỏ hoặc +50% life
    ReduceLevelExp = 2, // giảm exp cần để lên level
    ReduceChargeDamageTaken = 3, // dmg nhận từ va chạm được giảm (50% hoặc 100%)
    ReduceProjectileDamageTaken = 4, // dmg nhận từ projectile được giảm (50% hoặc 100%)
    EvoTwoRandomSkill = 5, // 2 skill được lên evo, ko đi kèm passive, có thể là 2 skill khác với skill vũ khí
}

public enum DailyChallengeDownside
{
    MaxSkillReduceOne = 0, // giảm 1 slot passive, active
    EternalDarkness = 1, // bật darkness mode
    IncLevelExp = 2, // tăng số exp cần để lên level
}

[Serializable]
public class SkillDataConfig
{
    ///DAMAGE
    [Tooltip("Hệ số nhân dmg của skill")]
    public float[] SkillDamageMultiplier = new float[6];
    ///SPEED
    [Tooltip("Speed của skill (bay, xoay...)")]
    public float[] SkillSpeed = new float[6];
    ///COOLDOWN
    [Tooltip("Cooldown theo cấp của skill, tính theo frame, 60fps")]
    public int[] BaseCooldown = new int[6];
    [Tooltip("Cooldown tối thiểu của skill theo cấp, tính theo frame, 60fps, ko thể bypass")]
    public int[] MinCooldown = new int[6];
    ///DURATION
    [Tooltip("Duration cơ bản theo cấp của skill, tính theo frame, 60fps")]
    public int[] BaseDuration = new int[6];
    ///AOE
    [Tooltip("Scale hình ảnh tùy vào grade của skill")]
    public float[] ScaleMultiplierByGrade = new float[6];
}


public enum GameplayPopupType
{
    PausePopup = 0,
    LuckyTrain = 1,
    SkillChoice = 2,
    TrialMode = 3,
    Tutorial = 4,
    RevivePopup = 5,
    WinGamePopup = 6,
    GameOverPopup = 7,
    TestSkill = 8, // test cho creative
}

/// <summary>
/// Class dùng để lưu game lại khi bị crash / kill app
/// </summary>
public class SavedGameData
{
    public GameMode GameMode;
    public int Chapter;
    public int CurrentTimeInSecond;
    public int CurrentGold;
    public int CurrentLevel;
    ///// <summary>
    ///// Progress exp của level đó
    ///// </summary>
    //public int CurrentExp;
    public int KillCount;
    public int BossKillCount;
    /// <summary>
    /// Số đồ đã nhặt trong lượt chơi
    /// </summary>
    public int ItemChestCount;
    /// <summary>
    /// Số thẻ nhặt dc trong lượt chơi
    /// </summary>
    public int DesignCount;
    /// <summary>
    /// Số lần hồi sinh
    /// </summary>
    public int ReviveCount;
    /// <summary>
    /// Số lần crash / kill game phải vào lại bằng cái này, giới hạn là 3 để user ko dc kill game quá nhiều
    /// </summary>
    public int CrashTime;
}