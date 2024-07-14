using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyMissionItem : SerializedMonoBehaviour
{
    public DailyMissionType Type;
    public Image IconBackground;
    public Sprite[] IconBackgroundSprites;
    public Image Icon;
    public Dictionary<Fuel, Sprite> IconSprite;
    public TextMeshProUGUI Quantity;
    public TextMeshProUGUI Description;
    public Button Gonow;
    public Button Claim;
    public Slider ProgressSlider;
    public TextMeshProUGUI ProgressText;


    public Action<DailyMissionType> OnClickGoNow;
    public Action<DailyMissionType> OnClickClaim;
    private void Start()
    {
        Gonow.onClick.AddListener(OnGoNowClickF);
        Claim.onClick.AddListener(OnClaimClickF);
    }
    public void Setup(int claimQuantity, int claimState = 0)
    {
        switch (Type)
        {
            case DailyMissionType.ZombieKilled:
                Icon.sprite = IconSprite[Fuel.Gold];
                IconBackground.sprite = IconBackgroundSprites[1];
                Quantity.SetText(GameConfig.ZombieKilledBaseReward.ToString());
                Description.SetText(GameConfig.ZombieKilledDes);
                ProgressSlider.SetValueWithoutNotify(claimQuantity * 1f / GameConfig.ZombieKilledTarget);
                ProgressText.SetText($"{claimQuantity}/{GameConfig.ZombieKilledTarget}");
                if (claimQuantity == GameConfig.ZombieKilledTarget) Gonow.gameObject.SetActive(false);

                break;
            case DailyMissionType.BossKilled:
                Icon.sprite = IconSprite[Fuel.Gunpower];
                IconBackground.sprite = IconBackgroundSprites[0];
                Quantity.SetText(GameConfig.BossKilledBaseReward.ToString());
                Description.SetText(GameConfig.BossKilledDes);
                ProgressSlider.SetValueWithoutNotify(claimQuantity * 1f / GameConfig.BossKilledTarget);
                ProgressText.SetText($"{claimQuantity}/{GameConfig.BossKilledTarget}");
                if (claimQuantity == GameConfig.BossKilledTarget) Gonow.gameObject.SetActive(false);

                break;
            case DailyMissionType.AliveInSurvival:
                Icon.sprite = IconSprite[Fuel.Vitamin];
                IconBackground.sprite = IconBackgroundSprites[0];
                Quantity.SetText(GameConfig.AliveInSurvivalBaseReward.ToString());
                Description.SetText(GameConfig.AliveInSurvivalDes);
                ProgressSlider.SetValueWithoutNotify(claimQuantity * 1f / GameConfig.AliveInSurvivalTarget);
                ProgressText.SetText($"{claimQuantity}/{GameConfig.AliveInSurvivalTarget}");
                if (claimQuantity == GameConfig.AliveInSurvivalTarget) Gonow.gameObject.SetActive(false);
                break;


            case DailyMissionType.HardMissionCompleteTimes:
                Icon.sprite = IconSprite[Fuel.Uranium];
                IconBackground.sprite = IconBackgroundSprites[2];
                Quantity.SetText(GameConfig.HardMissionCompleteTimesBaseReward.ToString());
                Description.SetText(GameConfig.HardMissionCompleteTimesDes);
                ProgressSlider.SetValueWithoutNotify(claimQuantity * 1f / GameConfig.HardMissionCompleteTimesTarget);
                ProgressText.SetText($"{claimQuantity}/{GameConfig.HardMissionCompleteTimesTarget}");
                if (claimQuantity == GameConfig.HardMissionCompleteTimesTarget) Gonow.gameObject.SetActive(false);

                break;
            case DailyMissionType.Reroll1Time:
                Icon.sprite = IconSprite[Fuel.Coal];
                IconBackground.sprite = IconBackgroundSprites[2];
                Quantity.SetText(GameConfig.RerollMissionBaseReward.ToString());
                Description.SetText(GameConfig.RerollMissionDes);
                ProgressSlider.SetValueWithoutNotify(claimQuantity * 1f / GameConfig.RerollMissionTarget);
                ProgressText.SetText($"{claimQuantity}/{GameConfig.RerollMissionTarget}");
                if (claimQuantity == GameConfig.RerollMissionTarget) Gonow.gameObject.SetActive(false);

                break;
            case DailyMissionType.GetStreak:
                Icon.sprite = IconSprite[Fuel.Steel];
                IconBackground.sprite = IconBackgroundSprites[1];
                Quantity.SetText(GameConfig.GetStreakBaseReward.ToString());
                Description.SetText(GameConfig.GetStreakDes);
                ProgressSlider.SetValueWithoutNotify(claimQuantity * 1f / GameConfig.GetStreakTarget);
                ProgressText.SetText($"{claimQuantity}/{GameConfig.GetStreakTarget}");
                if (claimQuantity == GameConfig.GetStreakTarget) Gonow.gameObject.SetActive(false);

                break;

        }
        Icon.SetNativeSize();
        if (claimState == 1) Claim.gameObject.SetActive(false);

    }

    public void OnGoNowClickF()
    {
        OnClickGoNow?.Invoke(Type);
    }
    public void OnClaimClickF()
    {
        OnClickClaim?.Invoke(Type);
    }

}

public enum Fuel
{
    Gold,
    Coal,
    Gunpower,
    Steel,
    Vitamin,
    Uranium

}