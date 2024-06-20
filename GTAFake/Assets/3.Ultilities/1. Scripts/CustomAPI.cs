using UnityEngine;
using BestHTTP;
using System;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class CustomAPI: SerializedMonoBehaviour
{
    Action<string> CurrentCallback;
    protected void SendGetRequest(string url, Action<string> callback, bool useToken = true)
    {
        // check + display nếu ko có mạng
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (SceneManager.GetActiveScene().name == GameConfig.LOADING_SCENE)
            {
                // TOAN TOAN TOAN
            }
            else
            {
            
                return;
            }
        }

        //Debug.LogError(transform.name);
        CurrentCallback = callback;
        HTTPRequest request = new HTTPRequest(new Uri(url), OnRequestFinished);
        if (string.IsNullOrEmpty(GameConfig.Token) == false && useToken)
        {
            request.SetHeader("Authorization", $"Bearer {GameConfig.Token}");
        }
        request.Send();
    }

    protected void SendPostRequest(string url, string requestRawData, Action<string> callback)
    {
        //Debug.LogError(transform.name);
        CurrentCallback = callback;
        HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Post, OnRequestFinished);

        if (string.IsNullOrEmpty(GameConfig.Token) == false)
        {
            request.SetHeader("Authorization", $"Bearer {GameConfig.Token}");
        }
        if (string.IsNullOrEmpty(requestRawData) == false)
        {
            request.RawData = System.Text.Encoding.UTF8.GetBytes(requestRawData);
        }
        request.Send();
    }

    protected void SetPutRequest(Action<string> callback)
    {
        // TOAN TOAN TOAN

    }

    private void OnRequestFinished(HTTPRequest req, HTTPResponse resp)
    {

        switch (req.State)
        {
            // The request finished without any problem.
            case HTTPRequestStates.Finished:
                if (resp.IsSuccess)
                {
                    // Everything went as expected!
                    //Debug.LogError("Request Finished! Text received: " + resp.DataAsText);
                    CurrentCallback(resp.DataAsText);
                }
                else
                {
                    Debug.LogError(transform.name);
                    OnRequestFail(resp.Message);
                    Debug.LogError(string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}",
                                                    resp.StatusCode,
                                                    resp.Message,
                                                    resp.DataAsText));
                }
                break;

            // The request finished with an unexpected error. The request's Exception property may contain more info about the error.
            case HTTPRequestStates.Error:
                OnRequestFail("Request Finished with Error!");
                Debug.LogError("Request Finished with Error! " + (req.Exception != null ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception"));
                break;

            // The request aborted, initiated by the user.
            case HTTPRequestStates.Aborted:
                Debug.LogError("Request Aborted!");
                OnRequestFail("Request Aborted!");
                break;

            // Connecting to the server is timed out.
            case HTTPRequestStates.ConnectionTimedOut:
                Debug.LogError("Connection Timed Out!");
                OnRequestFail("Connection Timed Out!");
                break;

            // The request didn't finished in the given time.
            case HTTPRequestStates.TimedOut:
                Debug.LogError("Processing the request Timed Out!");
                OnRequestFail("Processing the request Timed Out!");
                break;
        }
    }

    protected virtual void OnRequestFail(string errorMessage)
    {

    }
}
