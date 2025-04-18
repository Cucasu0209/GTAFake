using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class MissionInfo_Item : MonoBehaviour
{
    private MissionDifficulty MissionDifficulty;
    private MissionType MissionType;
    private int MissionContentIndex;
    public TextMeshProUGUI Type;
    [Header("Icon")]
    public Image TypeIcon;
    public Sprite HuntIcon;
    public Sprite RescueIcon;
    public Sprite FindItemIcon;

    [Header("Header BG")]
    public Image DifficultyIcon;
    public Color EasyBG;
    public Color NormalBG;
    public Color HardBG;

    [Header("Content")]
    public TextMeshProUGUI Content;

    public Button ButtonStart;

    public MissionRewardItem[] missionRewardItems;

    private void Start()
    {
        ButtonStart.onClick.AddListener(() => SceneManager.LoadScene(GameConfig.GAME_PLAY_SCENE));
    }
    public void SetDifficulty(MissionDifficulty diff)
    {
        MissionDifficulty = diff;
        switch (diff)
        {
            case MissionDifficulty.Easy:
                DifficultyIcon.color = EasyBG;
                break;
            case MissionDifficulty.Normal:
                DifficultyIcon.color = NormalBG;
                break;
            case MissionDifficulty.Hard:
                DifficultyIcon.color = HardBG;
                break;

        }
    }

    public void SetContent(MissionType type, int contentIndex)
    {
        MissionType = type;
        MissionContentIndex = contentIndex;
        Type.SetText(GetMissionType(type));
        Content.SetText(GetMissionContent(type, contentIndex));

        switch (type)
        {
            case MissionType.Hunt:
                TypeIcon.sprite = HuntIcon;
                break;
            case MissionType.Rescue:
                TypeIcon.sprite = RescueIcon;
                break;
            case MissionType.FindItem:
                TypeIcon.sprite = FindItemIcon;
                break;
        }

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
