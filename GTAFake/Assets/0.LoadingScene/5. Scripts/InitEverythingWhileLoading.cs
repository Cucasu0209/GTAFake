using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Apple;

public class InitEverythingWhileLoading : MonoBehaviour
{
    private void Start()
    {
        //LocalizationManager.Instance.OnLoadStringTableDone += OnLocalizeDone;
        // TOAN TOAN TOAN
        // KHI NÀO TÍCH HỢP LOCALIZATION THÌ BỎ CÁI NÀY ĐI
        LoadUserData();
    }

    private void OnDestroy()
    {
        //LocalizationManager.Instance.OnLoadStringTableDone -= OnLocalizeDone;
    }
    private void OnLocalizeDone()
    {
        LoadUserData();
    }

    private void LoadUserData()
    {
        CustomAuthentication.Instance.OnLoadUserInfoFinish += LoadGameData;
        CustomAuthentication.Instance.LoadUserData();
    }

    private void LoadGameData()
    {
        CustomAuthentication.Instance.OnLoadUserInfoFinish -= LoadGameData;
        GameDataManager.Instance.OnLoadGameDataFinish += OnLoadShopDataDone;
        GameDataManager.Instance.LoadGameData();
    }

    private void LoadShopData()
    {
        // GameDataManager.Instance.OnLoadGameDataFinish -= LoadShopData;

    }

    private void OnLoadShopDataDone()
    {
        GameDataManager.Instance.OnLoadGameDataFinish -= OnLoadShopDataDone;
        SceneManager.LoadSceneAsync(GameConfig.HOME_SCENE);
    }
}
