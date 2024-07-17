
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChangingModelSystem : SerializedMonoBehaviour
{
    public Animator CurrentAnim;
    public Dictionary<CharacterType, GameObject> CharactorDictionary;
    private void Start()
    {
        UserInputController.Instance.OnSwitchModel += OnSwitchModel;

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5)) SwitchToPlayer(CharacterType.Gangster);
        if (Input.GetKeyDown(KeyCode.Alpha6)) SwitchToPlayer(CharacterType.Soldier);
        if (Input.GetKeyDown(KeyCode.Alpha7)) SwitchToPlayer(CharacterType.SpiderMan1);
        if (Input.GetKeyDown(KeyCode.Alpha8)) SwitchToPlayer(CharacterType.SpiderMan2);
        if (Input.GetKeyDown(KeyCode.Alpha9)) SwitchToPlayer(CharacterType.Mech1);
        if (Input.GetKeyDown(KeyCode.Alpha0)) SwitchToPlayer(CharacterType.Mech2);
    }
    private void OnDestroy()
    {
        UserInputController.Instance.OnSwitchModel -= OnSwitchModel;
    }
    private void OnSwitchModel(CharacterType type)
    {
        SwitchToPlayer(type);
    }
    private void SwitchToPlayer(CharacterType type)
    {
        Destroy(CurrentAnim.gameObject);
        GameObject newmodel = Instantiate(CharactorDictionary[type], transform);
        newmodel.transform.localPosition = Vector3.zero;
        CurrentAnim = newmodel.GetComponent<Animator>();
        gameObject.GetComponent<PlayerController>().PlayerAnimator = newmodel.GetComponent<Animator>();
        gameObject.GetComponent<PlayerWeaponManager>().WeaponPosition = newmodel.GetComponent<PlayerAnimationCallbacks>().RightHand;
        foreach (var weapon in gameObject.GetComponent<PlayerWeaponManager>().CurrentWeapons)
        {
            weapon.transform.parent = newmodel.GetComponent<PlayerAnimationCallbacks>().RightHand;
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}
