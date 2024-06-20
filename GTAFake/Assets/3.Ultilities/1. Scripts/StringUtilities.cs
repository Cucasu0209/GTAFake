using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringUtilities
{
    /// <summary>
    /// Lấy string cho hiển thị số
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string GetDisplayNumber(int value)
    {
        if (value > 1000000000)
        {
            // > 1 bil
            return $"{value * 0.000000001f:0.##}B";
        }
        if (value > 1000000)
        {
            // > 1 mil
            return $"{value * 0.000001f:0.##}M";
        }
        if (value >= 10000)
        {
            return $"{value / 1000}K";
        }
        return $"{value}";
    }
    /// <summary>
    /// Lấy string cho hiển thị số (thường là giá)
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string GetDisplayNumberWithFloat(int value)
    {
        if (value > 1000000000)
        {
            // > 1 bil
            return $"{value * 0.000000001f:0.#}B";
        }
        if (value > 1000000)
        {
            // > 1 mil
            return $"{value * 0.000001f:0.#}M";
        }
        if (value >= 10000)
        {
            return $"{value * 0.001f:0.#}K";
        }
        return $"{value}";
    }


    public static string GetDisplayTimer(int totalSecond)
    {
        int second = totalSecond % 60;
        totalSecond = (totalSecond - second) / 60;
        int minute = totalSecond % 60;
        totalSecond = (totalSecond - minute) / 60;
        int hour = totalSecond % 24;
        int day = (totalSecond - hour) / 24;

        string dayString = day > 0 ? $"{day:D2} day " : "";
        string hourString = hour > 0 ? $"{hour:D2} hour " : "";
        string minuteString = minute > 0 ? $"{minute:D2} minute " : "";
        string secondString = second > 0 ? $"{second:D2} second " : "";
        return $"{dayString}{hourString}{minuteString}{secondString}";
    }

    public static string GetShortDisplayTimer(int totalSecond)
    {
        int second = totalSecond % 60;
        totalSecond = (totalSecond - second) / 60;
        int minute = totalSecond % 60;
        totalSecond = (totalSecond - minute) / 60;
        int hour = totalSecond % 24;
        int day = (totalSecond - hour) / 24;

        string dayString = day > 0 ? $"{day:D2}d " : "";
        string hourString = hour > 0 ? $"{hour:D2}h " : "";
        string minuteString = minute > 0 ? $"{minute:D2}m " : "";
        string secondString = second > 0 ? $"{second:D2}s " : "";
        return $"{dayString}{hourString}{minuteString}{secondString}";
    }

    public static string GetDisplayDate(DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd");
    }

    public static string GetExpiryDay(DateTime dateTime)
    {
        int remainDay = (int)dateTime.Subtract(DateTime.UtcNow).TotalDays;
        return $"{remainDay}d to expiry";
    }

    public static string GetExpiryHour(DateTime dateTime)
    {
        int remainHour = (int)dateTime.Subtract(DateTime.UtcNow).TotalHours;
        return $"{remainHour / 24}d {remainHour % 24}h";
    }
}