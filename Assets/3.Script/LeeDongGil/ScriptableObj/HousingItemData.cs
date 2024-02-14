using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HousingItemData", menuName = "ScriptableObject/HousingItemData")]
public class HousingItemData : ScriptableObject
{
    [Header("Materials order == MaterialCount order")]
    public Sprite housingSprite;
    public string housingName;
    public int housingID;
    public int enhanceLevel;

    public int housingWidth;
    public int housingHeight;
    public HousingType housingType;

    [TextArea]
    public string Info;
}


