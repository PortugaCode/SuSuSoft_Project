using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public bool checkObjectEqual = false;
    public Transform inventoryWindow;
    public Transform inventoryScroll;

    public void GetItem(ItemData itemData, int getCount)
    {
        Slot[] slots = GetComponentsInChildren<Slot>();
        int currentIndex = 0;
        foreach (Slot slot in slots)
        {
            if (slot.IsSlotUse)
            {
                if (slot.slotItemName.Equals(itemData.itemName))
                {
                    checkObjectEqual = true;
                    slot.GetComponentInChildren<ItemInfo>()._itemCount += getCount;
                    break;
                }
                else
                {
                    currentIndex++;
                }
            }
            else
            {
                checkObjectEqual = false;
            }
        }

        if (!checkObjectEqual)
        {
            SetItemInfo(itemData, slots[currentIndex]);
            slots[currentIndex].GetComponentInChildren<ItemInfo>()._itemCount = getCount;
        }

        checkObjectEqual = false;
    }

    public void GetHousingItem_test(HousingItemData housingItemData, int getCount)
    {
        HousingSlot[] slots = GetComponentsInChildren<HousingSlot>();
        foreach (HousingSlot slot in slots)
        {
            if (slot.isSlotUse)
            {
                if (slot.slotItemName.Equals(housingItemData.housingENName))
                {
                    slot.GetComponentInChildren<HousingInventory>().count += getCount;
                    slot.housingInven.image.color = Color.white;
                    break;
                }
                else continue;
            }
            else
            {
                SetItemInfo(housingItemData, slot);
                slot.GetComponentInChildren<HousingInventory>().count = getCount;
                slot.housingInven.image.color = Color.white;
                break;
            }
        }
    }

    public void GetHousingItem(int housingIndex, int getCount)          //데이터 연동용
    {
        HousingSlot[] slots = GetComponentsInChildren<HousingSlot>();
        //int listIndex = TestManager.instance.GetIndex(housingIndex);
        HousingObject housingObject = ChartManager.instance.housingObjectDatas[housingIndex];
        bool isChecked = false;
        int currentIndex = 0;
        int index = 0;
        foreach (HousingSlot slot in slots)
        {
            if (slot.isSlotUse)
            {
                if (slot.slotItemName.Equals(housingObject.name_e))
                {
                    checkObjectEqual = true;
                    slot.GetComponentInChildren<HousingInventory>().count += getCount;
                    break;
                }
            }
            else
            {
                if (!isChecked)
                {
                    currentIndex = index;
                    checkObjectEqual = false;
                    isChecked = true;
                    Debug.Log($"Current : {currentIndex}");
                }
            }
            index++;
            Debug.Log(index);
        }

        if (!checkObjectEqual)
        {
            SetItemInfo(housingObject, slots[currentIndex]);
            slots[currentIndex].GetComponentInChildren<HousingInventory>().count = getCount;
        }

        checkObjectEqual = false;
    }

    public void GotoWindow()
    {
        HousingSlot[] slots = GetComponentsInChildren<HousingSlot>();
        foreach (HousingSlot slot in slots)
        {
            slot.transform.SetParent(inventoryWindow);
            slot.isWindow = true;
        }
    }

    public void GotoScroll()
    {
        HousingSlot[] slots = GetComponentsInChildren<HousingSlot>();
        foreach (HousingSlot slot in slots)
        {
            slot.transform.SetParent(inventoryScroll);
            slot.isWindow = false;
        }
    }


    public void SetItemInfo(ItemData itemData, Slot _slot)      //공방 인벤토리에 이미지와 데이터 이름 넣기
    {
        _slot.transform.GetChild(0).GetComponent<Image>().sprite = itemData.sprite;
        _slot.transform.GetComponentInChildren<ItemInfo>()._itemData = itemData;
        _slot.transform.GetComponentInChildren<ItemInfo>()._itemName = itemData.itemName;
    }

    public void SetItemInfo(HousingItemData housingData, HousingSlot _slot)     //하우징 인벤토리에 이미지와 데이터 이름 넣기(Test)
    {
        _slot.transform.GetChild(0).GetComponent<Image>().sprite = housingData.housingSprite;
        _slot.transform.GetComponentInChildren<HousingInventory>().housingData = housingData;
        _slot.transform.GetComponentInChildren<HousingInventory>().housingName = housingData.housingENName;
    }

    public void SetItemInfo(HousingObject housing, HousingSlot _slot)     //하우징 인벤토리에 이미지와 데이터 이름 넣기(연동)
    {
        _slot.transform.GetChild(0).GetComponent<Image>().sprite = SpriteManager.instance.sprites[housing.imageIndex];
        _slot.transform.GetComponentInChildren<HousingInventory>().housingObj = housing;
        _slot.transform.GetComponentInChildren<HousingInventory>().housingName = housing.name_e;
    }
}
