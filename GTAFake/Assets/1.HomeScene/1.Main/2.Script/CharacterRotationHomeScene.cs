using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotationHomeScene : MonoBehaviour
{


    private Vector2 Angle = new Vector2(-90 * Mathf.Deg2Rad, 0);

    [SerializeField] private Transform TransformFollow;

    [SerializeField] private float Sensitivity;//độ nhạy

    // Start is called before the first frame update
    void Start()
    {
        UserInputController.Instance.OnCameraAxisChange += FollowPlayer;

    }



    private void FollowPlayer(float hor, float ver)
    {
        //transform.position = TransformFollow.position - Vector3.forward * MaxDistance;


        Angle.x -= hor * Mathf.Deg2Rad * Sensitivity;
        transform.rotation = Quaternion.Euler(-Angle.y / Mathf.Deg2Rad, Angle.x / Mathf.Deg2Rad - 90, 0);

    }
}
