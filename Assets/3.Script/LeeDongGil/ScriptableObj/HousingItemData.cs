using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HousingItemData", menuName = "ScriptableObject/HousingItemData")]
public class HousingItemData : ScriptableObject
{
    [Header("Housing Infomation")]
    public HousingType housingType;
    public Sprite housingSprite;
    public string housingENName;
    public string housingKRName;
    public int housingID;
    public float housingWidth;
    public float housingHeight;

    [Header("Housing Enhance")]
    public int enhanceLevel;
    public float enhanceValue;
    public bool isPercentage_enhance;

    [Header("Housing Set")]
    public string SetName;
    public string SetEffectName;
    public float SetEffectValue;
    public bool isPercentage_set;

    [TextArea]
    public string Info;
}


