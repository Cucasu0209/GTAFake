using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlying : MonoBehaviour
{
    private PlayerController Controller;

    private void Start()
    {
        Controller = GetComponent<PlayerController>();
        UserInputController.Instance.OnStartFlying += OnStartFlying;
        UserInputController.Instance.OnEndFlying += OnEndFlying;
    }

    private void OnDestroy()
    {
        UserInputController.Instance.OnStartFlying -= OnStartFlying;
        UserInputController.Instance.OnEndFlying -= OnEndFlying;
    }
    private void OnStartFlying()
    {

    }
    private void OnEndFlying()
    {

    }
}
