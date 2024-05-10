using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerChangingModelSystem : MonoBehaviour
{
    public Animator CurrentAnim;
    public int currentIndex = -1;
    public GameObject[] Model;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5)) SwitchToPlayer(0);
        if (Input.GetKeyDown(KeyCode.Alpha6)) SwitchToPlayer(1);
        if (Input.GetKeyDown(KeyCode.Alpha7)) SwitchToPlayer(2);
        if (Input.GetKeyDown(KeyCode.Alpha8)) SwitchToPlayer(3);
        if (Input.GetKeyDown(KeyCode.Alpha9)) SwitchToPlayer(4);
        if (Input.GetKeyDown(KeyCode.Alpha0)) SwitchToPlayer(5);
    }
    private void SwitchToPlayer(int playerIndex)
    {
        if (currentIndex == playerIndex) return;
        if (playerIndex >= Model.Length) return;
        Destroy(CurrentAnim.gameObject);
        currentIndex = playerIndex;
        GameObject newmodel = Instantiate(Model[currentIndex], transform);
        newmodel.transform.localPosition = Vector3.zero;
        CurrentAnim = newmodel.GetComponent<Animator>();
        gameObject.GetComponent<PlayerController>().PlayerAnimator = newmodel.GetComponent<Animator>();
        gameObject.GetComponent<PlayerWeaponManager>().WeaponPosition = newmodel.GetComponent<PlayerAnimationCallbacks>().RightHand;
        foreach (var weapon in gameObject.GetComponent<PlayerWeaponManager>().CurrentWeapons)
        {
            weapon.transform.parent = newmodel.GetComponent<PlayerAnimationCallbacks>().RightHand;
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation =Quaternion.Euler(0f, 0f, 0f);
        }
    }
}
