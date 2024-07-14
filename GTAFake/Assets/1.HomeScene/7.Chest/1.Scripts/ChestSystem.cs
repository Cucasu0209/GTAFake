using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSystem : SerializedMonoBehaviour
{
    [SerializeField] private List<ChestItemInfo> ChestInfo;
    [SerializeField] private ChestItemUI ChestInfoUIPrefab;
    [SerializeField] private RectTransform GridParent;

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        for (int i = 0; i < ChestInfo.Count; i++)
        {
            ChestItemUI newCell = Instantiate(ChestInfoUIPrefab, GridParent);
            newCell.Setup(ChestInfo[i]);
        }
    }
}
