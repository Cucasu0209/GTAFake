using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Apple;

public class InitEverythingWhileLoading : MonoBehaviour
{
    private void Start()
    {
        LoadingManager.Instance.CreateNewProcess();

        LoadingManager.Instance.RegisterAction(
            LoadingDataLabel.LoadUserData.ToString(),
            CustomAuthentication.Instance.LoadUserData);

        LoadingManager.Instance.RegisterAction(
            LoadingDataLabel.Login.ToString(),
            CustomAuthentication.Instance.Login,
             LoadingDataLabel.LoadUserData.ToString());

        LoadingManager.Instance.RegisterAction(
            LoadingDataLabel.LoadGameData.ToString(),
            GameDataManager.Instance.LoadGameData,
            LoadingDataLabel.Login.ToString());
        LoadingManager.Instance.OnLoadingComplete += OnLoadShopDataDone;
        LoadingManager.Instance.StartLoading();
    }
    private void OnDestroy()
    {
        LoadingManager.Instance.OnLoadingComplete -= OnLoadShopDataDone;
    }

    private void OnLoadShopDataDone()
    {
        SceneManager.LoadSceneAsync(GameConfig.HOME_SCENE);
    }
}
