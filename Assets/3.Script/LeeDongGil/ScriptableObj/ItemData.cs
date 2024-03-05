using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObject/ItemData")]
public class ItemData : ScriptableObject
{
    public ItemType type;
    public TokenType tokenType;
    public Sprite sprite;
    public string itemName;
    public int itemID;
}
public enum TokenType
{
    sports = 1,
    fruits = 2,

    None = 256
}
