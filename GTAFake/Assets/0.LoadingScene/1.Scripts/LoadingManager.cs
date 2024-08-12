using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Collections;

public enum LoadingDataLabel
{
    LoadUserData,
    Login,
    LoadGameData,

}
public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance;
    private LoadingProcess Currentprocess;
    public Action OnLoadingComplete;
    public Action OnStartLoading;
    public Action OnActionComplete;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    public float GetCurrentProgress() => Currentprocess.GetProgress();
    public void CreateNewProcess() => Currentprocess = new LoadingProcess();
    public void OnLoadingActionDone(string label)
    {
        Currentprocess?.OnLoadingActionDone(label);
        OnActionComplete?.Invoke();
        Debug.Log($"[LoadingManager]:Done {label} in {Currentprocess.GetTimeRunning(label)}s - progress: {Currentprocess.GetProgress() * Currentprocess.GetActionCount()}/{Currentprocess.GetActionCount()}");

    }
    public void RegisterAction(string label, Action action, params string[] labelConditions)
    {
        Currentprocess?.RegisterAction(label, action, labelConditions);
    }
    public void StartLoading()
    {
        Debug.Log($"[LoadingManager]:Start loading with progress: {Currentprocess.GetProgress() * Currentprocess.GetActionCount()}/{Currentprocess.GetActionCount()}");
        OnStartLoading?.Invoke();
        Currentprocess.RegisterCompletedAction(() =>
        {
            StartCoroutine(IOnCompletedLoading());
        });
        Currentprocess.StartRunableAction();

    }
    public IEnumerator IOnCompletedLoading()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log($"[LoadingManager]:{Currentprocess.GetActionCount()} Process Completed in total {Currentprocess.GetTimeRunning()}s");
        OnLoadingComplete?.Invoke();

    }
}
public enum LoadingProgressType
{
    HavenNotStartYet,
    Loading,
    Completed,
}
public class LoadingProcess
{
    private Action OnEndProcess;
    private Dictionary<string, Action> ActionList;
    private Dictionary<string, LoadingProgressType> CompletedActionMark;
    private Dictionary<string, List<string>> ConstraintLabel;
    private Dictionary<string, Stopwatch> ActionCounters;
    public LoadingProcess()
    {
        ActionList = new Dictionary<string, Action>();
        ConstraintLabel = new Dictionary<string, List<string>>();
        CompletedActionMark = new Dictionary<string, LoadingProgressType>();
        ActionCounters = new Dictionary<string, Stopwatch>();
    }
    #region Get
    public float GetProgress() => (float)CompletedActionMark.Sum(a => a.Value == LoadingProgressType.Completed ? 1 : 0) / ActionList.Count;
    public int GetActionCount() => ActionList.Count;
    public float GetTimeRunning(string label) => ActionCounters[label].ElapsedMilliseconds / 1000f;
    public float GetTimeRunning() => ActionCounters.Sum(a => a.Value.ElapsedMilliseconds) / 1000f;
    #endregion

    #region Progress
    public void StartRunableAction()
    {
        foreach (var action in ActionList)
        {
            if (CompletedActionMark[action.Key] == LoadingProgressType.HavenNotStartYet)
            {
                if (ConstraintLabel[action.Key] == null || ConstraintLabel[action.Key].Count == 0)
                {
                    action.Value?.Invoke();
                    CompletedActionMark[action.Key] = LoadingProgressType.Loading;
                    ActionCounters[action.Key].Start();
                }
            }
        }
    }
    public void RegisterAction(string label, Action action, params string[] labelConditions)
    {
        ConstraintLabel.Add(label, new List<string>(labelConditions));
        ActionList.Add(label, action);
        CompletedActionMark.Add(label, LoadingProgressType.HavenNotStartYet);
        ActionCounters.Add(label, new Stopwatch());
    }
    public void RegisterCompletedAction(Action OnFinish)
    {
        OnEndProcess = OnFinish;
    }
    public void OnLoadingActionDone(string label)
    {
        if (ActionList.ContainsKey(label))
        {
            CompletedActionMark[label] = LoadingProgressType.Completed;
            ActionCounters[label].Stop();

            foreach (var constraint in ConstraintLabel)
            {
                constraint.Value.Remove(label);
            }
            StartRunableAction();
            if (CompletedActionMark.All(a => a.Value == LoadingProgressType.Completed)) OnFinish();
        }
    }
    private void OnFinish()
    {
        OnEndProcess?.Invoke();
    }
    #endregion
}