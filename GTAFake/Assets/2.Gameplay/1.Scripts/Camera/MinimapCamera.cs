using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform Player;
    public Transform AimZoneInminimap;


    private void Update()
    {
        transform.position = new Vector3(Player.position.x, transform.position.y, Player.position.z);
        AimZoneInminimap.eulerAngles = Vector3.up * Camera.main.transform.eulerAngles.y;
    }
}
