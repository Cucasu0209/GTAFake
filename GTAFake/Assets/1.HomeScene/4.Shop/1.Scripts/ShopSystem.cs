using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopSystem : CustomAPI
{
    public Button RemoveAdsButton;
    public Button ClaimChestByAds;
    public Button ClaimChestByKeys;
    public Button ClaimStarterPack;
    public Button ClaimSmallGoldPack;
    public Button ClaimMediumGoldPack;
    public Button ClaimLargeGoldPack;

    private void Start()
    {
        if (GameDataManager.Instance.playerdataresult.claim_remove_ads) RemoveAdsButton.gameObject.SetActive(false);
        if (GameDataManager.Instance.playerdataresult.claim_starter_pack) ClaimStarterPack.gameObject.SetActive(false);
        RemoveAdsButton.onClick.AddListener(OnRemoveAdsClick);
        ClaimChestByAds.onClick.AddListener(OnClaimChestByAdsClick);
        ClaimChestByKeys.onClick.AddListener(OnClaimChestByKeyClick);
        ClaimStarterPack.onClick.AddListener(OnClaimStaterPackPackClick);
        ClaimSmallGoldPack.onClick.AddListener(OnClaimSmallGoldPackClick);
        ClaimMediumGoldPack.onClick.AddListener(OnClaimMediumGoldPackClick);
        ClaimLargeGoldPack.onClick.AddListener(OnClaimLargeGoldPackClick);
    }

    private void OnRemoveAdsClick()
    {
        string data = JsonConvert.SerializeObject(new PackageInfo()
        {
            PackageName = "",
            ProductId = "",
            PurchaseToken = ""
        }
                , Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
        SendPostRequest($"{GameConfig.ServerURL}/api/android/claim_remove_ads?id={GameDataManager.Instance.UserData.UserId}", data, OnRemoveAdsResult);
    }
    private void OnRemoveAdsResult(string result)
    {
        Debug.LogError(result);
        RemoveAdsButton.gameObject.SetActive(false);
    }
    public void OnClaimChestByAdsClick()
    {
        string data = JsonConvert.SerializeObject(new PackageInfo()
        {
            PackageName = "",
            ProductId = "",
            PurchaseToken = ""
        }
                     , Formatting.Indented, new JsonSerializerSettings
                     {
                         NullValueHandling = NullValueHandling.Ignore
                     });
        SendPostRequest($"{GameConfig.ServerURL}/api/data/claim_chest_ads?id={GameDataManager.Instance.UserData.UserId}", data, OnClaimChestResult);
    }
    private void OnClaimChestResult(string result)
    {
        Debug.LogError(result);
    }
    public void OnClaimChestByKeyClick()
    {
        string data = JsonConvert.SerializeObject(new PackageInfo()
        {
            PackageName = "",
            ProductId = "",
            PurchaseToken = ""
        }
                      , Formatting.Indented, new JsonSerializerSettings
                      {
                          NullValueHandling = NullValueHandling.Ignore
                      });
        SendPostRequest($"{GameConfig.ServerURL}/api/data/claim_chest?id={GameDataManager.Instance.UserData.UserId}", data, OnClaimChestResult);
    }
    public void OnClaimStaterPackPackClick()
    {
        string data = JsonConvert.SerializeObject(new PackageInfo()
        {
            PackageName = "",
            ProductId = "",
            PurchaseToken = ""
        }
               , Formatting.Indented, new JsonSerializerSettings
               {
                   NullValueHandling = NullValueHandling.Ignore
               });
        SendPostRequest($"{GameConfig.ServerURL}/api/android/claim_starter_pack?id={GameDataManager.Instance.UserData.UserId}", data, OnStarterPackResult);
    }
    private void OnStarterPackResult(string result)
    {
        Debug.LogError(result);
        ClaimStarterPack.gameObject.SetActive(false);
    }
    public void OnClaimSmallGoldPackClick()
    {
        string data = JsonConvert.SerializeObject(new PackageInfo()
        {
            PackageName = "pack_1",
            ProductId = "",
            PurchaseToken = ""
        }
               , Formatting.Indented, new JsonSerializerSettings
               {
                   NullValueHandling = NullValueHandling.Ignore
               });
        SendPostRequest($"{GameConfig.ServerURL}/api/data/claim_gold_package?id={GameDataManager.Instance.UserData.UserId}", data, OnClaimGoldPackResult);
    }
    private void OnClaimGoldPackResult(string result)
    {
        Debug.LogError(result);
    }
    public void OnClaimMediumGoldPackClick()
    {
        string data = JsonConvert.SerializeObject(new PackageInfo()
        {
            PackageName = "pack_2",
            ProductId = "",
            PurchaseToken = ""
        }
       , Formatting.Indented, new JsonSerializerSettings
       {
           NullValueHandling = NullValueHandling.Ignore
       });
        SendPostRequest($"{GameConfig.ServerURL}/api/data/claim_gold_package?id={GameDataManager.Instance.UserData.UserId}", data, OnClaimGoldPackResult);
    }
    public void OnClaimLargeGoldPackClick()
    {
        string data = JsonConvert.SerializeObject(new PackageInfo()
        {
            PackageName = "pack_3",
            ProductId = "",
            PurchaseToken = ""
        }
       , Formatting.Indented, new JsonSerializerSettings
       {
           NullValueHandling = NullValueHandling.Ignore
       });
        SendPostRequest($"{GameConfig.ServerURL}/api/data/claim_gold_package?id={GameDataManager.Instance.UserData.UserId}", data, OnClaimGoldPackResult);
    }
}
