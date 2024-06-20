using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSystem: MonoBehaviour
{
    public static TimeSystem Instance;
    public Action OnNewDay;
    public Action OnNewWeek;
    public Action OnNewMonth;
    [HideInInspector] public int CurrentDate;

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
    private void Start()
    {
        CurrentDate = DateTime.UtcNow.DayOfYear;
        CountdownTimeToNextDay = 24 * 3600 - Mathf.RoundToInt((float)DateTime.UtcNow.TimeOfDay.TotalSeconds);
    }


#if UNITY_ANDROID
    private void OnApplicationPause(bool pause)
    {
        if (pause == false)
        {
            // resume game
            CountdownTimeToNextDay = 24 * 3600 - Mathf.RoundToInt((float)DateTime.UtcNow.TimeOfDay.TotalSeconds);
            if (DateTime.UtcNow.DayOfYear != CurrentDate)
            {
                CurrentDate = DateTime.UtcNow.DayOfYear;
                if (OnNewDay != null)
                {
                    OnNewDay();
                }
            }
        }
    }
#endif
#if UNITY_IOS
    private void OnApplicationFocus(bool hasFocus)
    {
        if(hasFocus)
        {
            // resume game
            CountdownTime = 24 * 3600 - Mathf.RoundToInt((float)DateTime.UtcNow.TimeOfDay.TotalSeconds);
            if (DateTime.UtcNow.DayOfYear != CurrentDate)
            {
                CurrentDate = DateTime.UtcNow.DayOfYear;
                if (OnNewDay != null)
                {
                    OnNewDay();
                }
            }
        }
    }
#endif

    private float Timer = 0;
    /// <summary>
    /// Thời gian đếm đến cuối ngày
    /// </summary>
    public int CountdownTimeToNextDay { get; private set; }
    /// <summary>
    /// Thời gian đếm đến đầu tuần sau
    /// </summary>
    public int CountdownTimeToNextWeek { get; private set; }
    /// <summary>
    /// Thời gian đếm đến đầu tháng sau
    /// </summary>
    public int CountdownTimeToNextMonth { get; private set; }

    void Update()
    {
        Timer += Time.unscaledDeltaTime;
        if (Timer >= 1)
        {
            CountdownTimeToNextDay = 24 * 3600 - Mathf.RoundToInt((float)DateTime.UtcNow.TimeOfDay.TotalSeconds);
            CountdownTimeToNextWeek = CountdownTimeToNextDay + (6 - ((int)DateTime.UtcNow.DayOfWeek + 6) % 7) * 24 * 3600;
            // TOAN TOAN TOAN
            // CHECK LAI
            DateTime now = DateTime.UtcNow;
            CountdownTimeToNextMonth = (int)(new DateTime(now.AddMonths(1).Year, now.AddMonths(1).Month, 1) - now).TotalSeconds;

            Timer--;
            if (CountdownTimeToNextDay <= 0)
            {
                if (OnNewDay != null)
                {
                    OnNewDay();
                }
            }
        }
    }

    public bool IsSameWeek(DateTime date_1, DateTime date_2)
    {
        // ngày thứ 2(monday) của date_1
        int monday_1 = GetDayOfYearForMonday(date_1);

        // ngày thứ 2(monday) của date_2
        int monday_2 = GetDayOfYearForMonday(date_2);
        return monday_1 == monday_2;
    }
    /// <summary>
    /// Lấy ngày trong năm của thứ 2 (monday) đầu tuần
    /// </summary>
    /// <returns></returns>
    public int GetDayOfYearForMonday(DateTime date)
    {
        int monday = date.DayOfYear - ((int)date.DayOfWeek + 6) % 7;
        if (monday <= 0)
        {
            if (IsLeapYear(date.Year - 1))
            {
                monday += 366;
            }
            else
            {
                monday += 365;
            }
        }
        return monday;
    }

    public bool IsSameMonth(DateTime date_1, DateTime date_2)
    {
        return date_1.Month == date_2.Month;
    }

    private bool IsLeapYear(int yearNo)
    {
        return yearNo % 4 == 0;
    }
}
