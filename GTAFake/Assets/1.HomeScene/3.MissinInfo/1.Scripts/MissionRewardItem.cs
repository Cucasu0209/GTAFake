using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionRewardItem : SerializedMonoBehaviour
{
    public MissionRewardType Type;
    [SerializeField] public Dictionary<MissionRewardType, Sprite> RewardIcon;
    public Image Background;
    public Sprite[] BgSprites;
    public Image Icon;
    public TextMeshProUGUI Amount;

    public void UpdateReward(MissionRewardType type, int amount)
    {
        Icon.sprite = RewardIcon[type];
        Amount.SetText(amount.ToString());
        Background.sprite = BgSprites[((int)type) % BgSprites.Length];
    }
}
