using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionRewardItem : SerializedMonoBehaviour
{
    public MissionRewardType Type;
    [SerializeField] public Dictionary<MissionRewardType, Sprite> RewardIcon;
    public Image[] BGs;
    public Image Icon;
    public TextMeshProUGUI Amount;

    public void UpdateReward(MissionRewardType type, int amount)
    {
        Icon.sprite = RewardIcon[type];
        Amount.SetText(amount.ToString());
        for (int i = 0; i < BGs.Length; i++)
        {
            BGs[i].gameObject.SetActive(i == ((int)type) % BGs.Length);
        }
    }
}
