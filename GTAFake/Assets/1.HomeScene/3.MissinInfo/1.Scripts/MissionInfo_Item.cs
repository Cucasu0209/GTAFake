using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionInfo_Item : MonoBehaviour
{
    private MissionDifficulty _missionDifficulty;
    private MissionType _missionType;
    private int _missionContentIndex;
    public TextMeshProUGUI Type;
    public Image Type_Hunt_Icon;
    public Image Type_Rescue_Icon;
    public Image Type_Defeat_Icon;

    public Image Dissiculty_Easy_BG;
    public Image Dissiculty_Mid_BG;
    public Image Dissiculty_Hard_BG;

    public TextMeshProUGUI Content;

    public Button ButtonStart;

    public MissionRewardItem[] missionRewardItems;
    public void SetDifficulty(MissionDifficulty diff)
    {
        _missionDifficulty = diff;
        Dissiculty_Easy_BG.gameObject.SetActive(diff == MissionDifficulty.Easy);
        Dissiculty_Mid_BG.gameObject.SetActive(diff == MissionDifficulty.Normal);
        Dissiculty_Hard_BG.gameObject.SetActive(diff == MissionDifficulty.Hard);
    }

    public void SetContent(MissionType type, int contentIndex)
    {
        _missionType = type;
        _missionContentIndex = contentIndex;
        Type.SetText(GetMissionType(type));
        Content.SetText(GetMissionContent(type, contentIndex));
        Type_Hunt_Icon.gameObject.SetActive(type == MissionType.Hunt);
        Type_Rescue_Icon.gameObject.SetActive(type == MissionType.Rescue);
        Type_Defeat_Icon.gameObject.SetActive(type == MissionType.FindItem);
    }
    
    public void UpdateReward(MissionRewards Rewards)
    {
        for (int g = 0; g < missionRewardItems.Length; g++) missionRewardItems[g].gameObject.SetActive(false);
        int j = 0;
        if (Rewards.Gold > 0)
        {
            missionRewardItems[j].gameObject.SetActive(true);
            missionRewardItems[j].UpdateReward(MissionRewardType.Gold, Rewards.Gold);
            j++;
        }
        if (Rewards.Gunpowder > 0)
        {
            missionRewardItems[j].gameObject.SetActive(true);
            missionRewardItems[j].UpdateReward(MissionRewardType.Gunpowder, Rewards.Gunpowder);
            j++;
        }
        if (Rewards.Coal > 0)
        {
            missionRewardItems[j].gameObject.SetActive(true);
            missionRewardItems[j].UpdateReward(MissionRewardType.Coal, Rewards.Coal);
            j++;
        }
        if (Rewards.Uranium > 0)
        {
            missionRewardItems[j].gameObject.SetActive(true);
            missionRewardItems[j].UpdateReward(MissionRewardType.Uranium, Rewards.Uranium);
            j++;
        }
        if (Rewards.Steel > 0)
        {
            missionRewardItems[j].gameObject.SetActive(true);
            missionRewardItems[j].UpdateReward(MissionRewardType.Steel, Rewards.Steel);
            j++;
        }
        if (Rewards.Vitamin > 0)
        {
            missionRewardItems[j].gameObject.SetActive(true);
            missionRewardItems[j].UpdateReward(MissionRewardType.Vitamin, Rewards.Vitamin);
            j++;
        }
    }

    private string GetMissionContent(MissionType type, int contentIndex)
    {
        switch (type)
        {
            case MissionType.Hunt:
                return GameConfig.MissionInfo00;
            case MissionType.Rescue:
                return GameConfig.MissionInfo10;
            case MissionType.FindItem:
                if (contentIndex == 0) return GameConfig.MissionInfo20;
                else if (contentIndex == 1) return GameConfig.MissionInfo21;
                return GameConfig.MissionInfo22;

        }
        return "";
    }
    private string GetMissionType(MissionType type)
    {
        switch (type)
        {
            case MissionType.Hunt: return "Hunt";
            case MissionType.Rescue: return "Rescue";
            case MissionType.FindItem: return "Find Item";
        }
        return "";
    }
}
