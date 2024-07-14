using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestItemUI : SerializedMonoBehaviour
{
    [Header("BG Sprites")]
    [SerializeField] private Image BG;
    [SerializeField] private Dictionary<Fuel, Sprite> BGSprites;

    [Header("Infomation")]
    [SerializeField] private TextMeshProUGUI ItemName;
    [SerializeField] private TextMeshProUGUI Percentage;

    public void Setup(ChestItemInfo data)
    {
        BG.sprite = BGSprites[data.Type];
        ItemName.SetText($"{data.Type} x{data.Quantity}");
        Percentage.SetText(data.Percentage + "%");

    }
}
[Serializable]
public class ChestItemInfo
{
    public Fuel Type;
    public int Quantity;
    public float Percentage; //stand for Percentage%
}
