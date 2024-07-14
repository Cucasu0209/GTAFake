using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyRewardSystem : MonoBehaviour
{
    public DailyRewardItemUI[] Items;
    string KeyDaily = "DailyRewardKey";
    int today;
    public bool TookAttendance = false;
    public List<DailyRewardData> ItemData;
    private void OnEnable()
    {
        TookAttendance = false;
        SetupButton();
    }
    private void Start()
    {
        today = PlayerPrefs.GetInt(KeyDaily, 0);
        SetupButton();
        for (int i = 0; i <= 6; i++)
        {
            Items[i].OnAttendance = OnItemClick;
            Items[i].Setup(i + 1, ItemData[i]);
        }
    }
    public void OnItemClick(int BtnIndex)
    {
        if (BtnIndex == (today % 7) && TookAttendance == false)
        {
            TookAttendance = true;
            SetupButton();
            today++;
            PlayerPrefs.SetInt(KeyDaily, today);
        }

    }

    private void SetupButton()
    {
        for (int i = 0; i <= 6; i++)
        {
            Items[i].Setup(i > (today % 7) ? DailyRewardItemUI.State.Future : (i == (today % 7) && TookAttendance == false ? DailyRewardItemUI.State.Today : DailyRewardItemUI.State.PassBy));
        }
    }
}
