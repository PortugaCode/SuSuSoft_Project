using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public bool checkObjectEqual = false;

    [Header("Housing Inventory")]
    public Transform inventoryWindow;
    public Transform inventoryScroll;

    [Header("Token Inventory")]
    public Transform inventoryCreate;
    public Transform inventoryUpgrade;

    private void OnEnable()
    {
        //유저 정보 로드
        if (DBManager.instance == null) return;
        Debug.Log($"DB Count : {DBManager.instance.user.housingObject.Count}");

        if (!TestManager.instance.isHousingInventoryLoad)
        {
            if (DBManager.instance.user.housingObject.Count > 0)
            {
                foreach (var key in DBManager.instance.user.housingObject.Keys)
                {
                    for (int i = 0; i < ChartManager.instance.housingObjectDatas.Count; i++)
                    {
                        if (ChartManager.instance.housingObjectDatas[i].name_e == key)
                        {
                            Debug.Log($"{i}번째 Key : {key}");
                            int housingIndex = ChartManager.instance.housingObjectDatas[i].index;
                            LoadHousingInventory(housingIndex, DBManager.instance.user.housingObject[key]);
                            break;
                        }
                    }
                }
            }
            //TestManager.instance.isHousingInventoryLoad = true;
        }

        if (!TestManager.instance.isInventoryLoad)
        {
            for (int i = 0; i < DBManager.instance.user.tokens.Length; i++)
            {
                if (DBManager.instance.user.tokens[i] != 0)
                {
                    LoadTokenItem(i, DBManager.instance.user.tokens[i]);
                }
            }
            //TestManager.instance.isInventoryLoad = true;
        }

    }

    public void LoadHousingInventory(int housingIndex, int count)
    {
        HousingSlot[] slots = GetComponentsInChildren<HousingSlot>();
        HousingObject housingObject = ChartManager.instance.housingObjectDatas[housingIndex];
        foreach (HousingSlot slot in slots)
        {
            if (!slot.isSlotUse)
            {
                SetItemInfo(housingObject, slot);
                slot.GetComponentInChildren<HousingInventory>().count = count;
                slot.isSlotUse = true;
                Debug.Log(slot.isSlotUse);
                break;

            }
            else
            {
                if (slot.housingObject.GetValueOrDefault().index == housingObject.index)
                {
                    Debug.Log("여기로 들어오면 그냥 넘어가");
                    break;
                }
            }
        }
    }

    public void LoadTokenItem(int tokenIndex, int count)
    {
        Slot[] slots = GetComponentsInChildren<Slot>();
        foreach (Slot slot in slots)
        {
            if (!slot.isSlotUse)
            {
                SetItemInfo(tokenIndex, slot);
                slot.itemInfo._itemCount = count;
                slot.isSlotUse = true;
                break;
            }
            else
            {
                if (slot.itemData.itemID == tokenIndex) break;
            }
        }
    }

    //공방 인벤토리
    public void GetItem(ItemData itemData, int getCount)
    {
        Slot[] slots = GetComponentsInChildren<Slot>();
        bool isChecked = false;
        int currentIndex = 0;
        int index = 0;
        foreach (Slot slot in slots)
        {
            if (slot.isSlotUse)
            {
                if (slot.slotItemName.Equals(itemData.itemName))
                {
                    checkObjectEqual = true;
                    slot.GetComponentInChildren<ItemInfo>()._itemCount += getCount;
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

    public void GetHousingItem(int housingIndex, int getCount)          //데이터 연동용 테스트
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
                    DBManager.instance.user.housingObject[housingObject.name_e] += getCount;
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
                    //Debug.Log($"Current : {currentIndex}");
                }
            }
            index++;
            //Debug.Log(index);
        }

        if (!checkObjectEqual)
        {
            SetItemInfo(housingObject, slots[currentIndex]);
            DBManager.instance.user.housingObject.Add(housingObject.name_e, getCount);          //DB매니저에 추가
            slots[currentIndex].GetComponentInChildren<HousingInventory>().count = getCount;
        }

        checkObjectEqual = false;
    }

    public void GetHousingItem_Local(int housingIndex, int getCount)          //DB에 안넣고 바로 인벤토리에 넣기
    {
        HousingSlot[] slots = GetComponentsInChildren<HousingSlot>();
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
                }
            }
            index++;
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

    public void GotoCreate()
    {
        Slot[] slots = GetComponentsInChildren<Slot>();
        foreach (Slot slot in slots)
        {
            slot.transform.SetParent(inventoryCreate);
            slot.isUpgradeSlot = false;
        }
    }

    public void GotoUpgrade()
    {
        Slot[] slots = GetComponentsInChildren<Slot>();
        foreach (Slot slot in slots)
        {
            slot.transform.SetParent(inventoryUpgrade);
            slot.isUpgradeSlot = true;
        }
    }

    private void OnDestroy()
    {
        TestManager.instance.isHousingInventoryLoad = false;
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

    public void SetItemInfo(int tokenIndex, Slot _slot)      //공방 인벤토리에 토큰이미지와 데이터 이름 넣기
    {
        _slot.transform.GetChild(0).GetComponent<Image>().sprite = TestManager.instance.tokenList[tokenIndex].sprite;
        _slot.transform.GetComponentInChildren<ItemInfo>()._itemData = TestManager.instance.tokenList[tokenIndex];
        _slot.transform.GetComponentInChildren<ItemInfo>()._itemName = TestManager.instance.tokenList[tokenIndex].itemName;
    }
}
