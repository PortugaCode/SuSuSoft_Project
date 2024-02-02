using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUpgradeRecipe : MonoBehaviour
{
    public ItemInfo item;
    public InventorySystem invenSys;
    public GameObject tier2Item;
    private void Awake()
    {
        invenSys = FindObjectOfType<InventorySystem>();
    }

    public bool Tier2Gem()
    {
        if(item._itemName.Equals("º¸¼®1") && item._itemCount >= 10)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
