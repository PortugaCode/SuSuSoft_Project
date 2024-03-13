using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilterButton : MonoBehaviour
{
    [Header("Housing Inventory")]
    public Transform scrollInventory;
    public Transform windowInventory;
    public bool isFilter = false;

    [Header("Token Inventory")]
    public Transform createInventory;
    public Transform upgradeInventory;
    public bool isWorkShopFilter = false;


    public Button btn;


    public void Filtering()
    {
        if (TestManager.instance.isAll)
        {
            AllButton();
        }
        else if (TestManager.instance.isFront)
        {
            Filter((int)housingType.front);
        }
        else if (TestManager.instance.isBack)
        {
            Filter((int)housingType.back);
        }
        else if (TestManager.instance.isBuilding)
        {
            Filter((int)housingType.building);
        }
        else if (TestManager.instance.isInteration)
        {
            Filter((int)housingType.interaction);
        }
    }

    #region Housing Filter

    public void AllButton()
    {
        isFilter = false;
        TestManager.instance.isAll = true;
        TestManager.instance.isFront = false;
        TestManager.instance.isBack = false;
        TestManager.instance.isBuilding = false;
        TestManager.instance.isInteration = false;
        HousingSlot[] slots = GetComponentsInChildren<HousingSlot>();
        foreach (HousingSlot slot in slots)
        {
            if (slot.isSlotUse)
            {
                slot.housingInven.image.color = Color.white;
                slot.housingInven.countObject.SetActive(true);
            }
        }
    }

    public void FrontButton()
    {
        isFilter = true;
        TestManager.instance.isAll = false;
        TestManager.instance.isFront = true;
        TestManager.instance.isBack = false;
        TestManager.instance.isBuilding = false;
        TestManager.instance.isInteration = false;
        HousingSlot[] slots = GetComponentsInChildren<HousingSlot>();
        int[] currentindexs = new int[20];
        int index = 0;
        int setindex = 0;
        foreach (HousingSlot slot in slots)
        {
            currentindexs[index] = slot.transform.GetSiblingIndex();
            if (slot.housingObject.layer == 6)
            {
                slot.transform.SetSiblingIndex(setindex);
                slot.housingInven.image.color = Color.white;
                slot.housingInven.countObject.SetActive(true);
                slot.housingInven.button.interactable = true;
                setindex++;
            }
            else
            {
                slot.housingInven.image.color = new Color(1, 1, 1, 0.5f);
                slot.housingInven.countObject.SetActive(false);
                slot.housingInven.button.interactable = false;
            }

        }
    }

    public void BackButton()
    {
        isFilter = true;
        TestManager.instance.isAll = false;
        TestManager.instance.isFront = false;
        TestManager.instance.isBack = true;
        TestManager.instance.isBuilding = false;
        TestManager.instance.isInteration = false;
        HousingSlot[] slots = GetComponentsInChildren<HousingSlot>();
        int[] currentindexs = new int[20];
        int index = 0;
        int setindex = 0;
        foreach (HousingSlot slot in slots)
        {
            currentindexs[index] = slot.transform.GetSiblingIndex();
            if (slot.housingObject.layer == 2)
            {
                slot.transform.SetSiblingIndex(setindex);
                slot.housingInven.image.color = Color.white;
                slot.housingInven.countObject.SetActive(true);
                slot.housingInven.button.interactable = true;
                setindex++;
            }
            else
            {
                slot.housingInven.image.color = new Color(1, 1, 1, 0.5f);
                slot.housingInven.countObject.SetActive(false);
                slot.housingInven.button.interactable = false;
            }

        }
    }

    public void BuildingButton()
    {
        isFilter = true;
        TestManager.instance.isAll = false;
        TestManager.instance.isFront = false;
        TestManager.instance.isBack = false;
        TestManager.instance.isBuilding = true;
        TestManager.instance.isInteration = false;
        HousingSlot[] slots = GetComponentsInChildren<HousingSlot>();
        int[] currentindexs = new int[20];
        int index = 0;
        int setindex = 0;
        foreach (HousingSlot slot in slots)
        {
            currentindexs[index] = slot.transform.GetSiblingIndex();
            if (slot.housingObject.layer == 3)
            {
                slot.transform.SetSiblingIndex(setindex);
                slot.housingInven.image.color = Color.white;
                slot.housingInven.countObject.SetActive(true);
                slot.housingInven.button.interactable = true;
                setindex++;
            }
            else
            {
                slot.housingInven.image.color = new Color(1, 1, 1, 0.5f);
                slot.housingInven.countObject.SetActive(false);
                slot.housingInven.button.interactable = false;
            }

        }
    }

    public void InteractionButton()
    {
        isFilter = true;
        TestManager.instance.isAll = false;
        TestManager.instance.isFront = false;
        TestManager.instance.isBack = false;
        TestManager.instance.isBuilding = false;
        TestManager.instance.isInteration = true;
        HousingSlot[] slots = GetComponentsInChildren<HousingSlot>();
        int[] currentindexs = new int[20];
        int index = 0;
        int setindex = 0;
        foreach (HousingSlot slot in slots)
        {
            currentindexs[index] = slot.transform.GetSiblingIndex();
            if (slot.housingObject.layer == 8)
            {
                slot.transform.SetSiblingIndex(setindex);
                slot.housingInven.image.color = Color.white;
                slot.housingInven.countObject.SetActive(true);
                slot.housingInven.button.interactable = true;
                setindex++;
            }
            else
            {
                slot.housingInven.image.color = new Color(1, 1, 1, 0.5f);
                slot.housingInven.countObject.SetActive(false);
                slot.housingInven.button.interactable = false;
            }

        }
    }
    #endregion

    public void Filter(int value)
    {
        isFilter = true;
        TestManager.instance.isAll = false;
        switch (value)
        {
            case 2:     //back
                TestManager.instance.isFront = false;
                TestManager.instance.isBack = true;
                TestManager.instance.isBuilding = false;
                TestManager.instance.isInteration = false;
                break;
            case 3:     //building
                TestManager.instance.isFront = false;
                TestManager.instance.isBack = false;
                TestManager.instance.isBuilding = true;
                TestManager.instance.isInteration = false;
                break;
            case 6:     //front
                TestManager.instance.isFront = true;
                TestManager.instance.isBack = false;
                TestManager.instance.isBuilding = false;
                TestManager.instance.isInteration = false;
                break;
            case 8:     //interaction
                TestManager.instance.isFront = false;
                TestManager.instance.isBack = false;
                TestManager.instance.isBuilding = false;
                TestManager.instance.isInteration = true;
                break;
        }
        HousingSlot[] slots = GetComponentsInChildren<HousingSlot>();
        //int setindex = 0;
        foreach (HousingSlot slot in slots)
        {
            if (slot.housingObject.layer == value)
            {
                slot.transform.SetAsFirstSibling();
                slot.housingInven.image.color = Color.white;
                slot.housingInven.countObject.SetActive(true);
                slot.housingInven.button.interactable = true;
            }
            else
            {
                slot.housingInven.image.color = new Color(1, 1, 1, 0.5f);
                slot.housingInven.countObject.SetActive(false);
                slot.housingInven.button.interactable = false;
            }

        }
    }


    #region Token Filter

    public void AllTokenButton()
    {
        isWorkShopFilter = false;
        TestManager.instance.isAllToken = true;
        TestManager.instance.isSportsToken = false;
        TestManager.instance.isFruitsToken = false;
        Slot[] slots = GetComponentsInChildren<Slot>();
        foreach (Slot slot in slots)
        {
            if (slot.isSlotUse)
            {
                slot.itemInfo.image.color = Color.white;
                slot.itemInfo.countObject.SetActive(true);
            }
        }
    }

    public void SportsTokenButton()
    {
        isWorkShopFilter = true;
        TestManager.instance.isAllToken = false;
        TestManager.instance.isSportsToken = true;
        TestManager.instance.isFruitsToken = false;
        Slot[] slots = GetComponentsInChildren<Slot>();
        int[] currentindexs = new int[12];
        int index = 0;
        int setindex = 0;
        foreach (Slot slot in slots)
        {
            currentindexs[index] = slot.transform.GetSiblingIndex();
            if (slot.isSlotUse)
            {
                if ((slot.itemInfo._itemData.tokenType & TokenType.sports) != 0)
                {
                    slot.transform.SetSiblingIndex(setindex);
                    slot.itemInfo.image.color = Color.white;
                    slot.itemInfo.countObject.SetActive(true);
                    slot.itemInfo.button.interactable = true;
                    setindex++;
                }
                else
                {
                    slot.itemInfo.image.color = new Color(1, 1, 1, 0.5f);
                    slot.itemInfo.countObject.SetActive(false);
                    slot.itemInfo.button.interactable = false;
                }
            }
        }
    }

    public void FruitsTokenButton()
    {
        isWorkShopFilter = true;
        TestManager.instance.isAllToken = false;
        TestManager.instance.isSportsToken = false;
        TestManager.instance.isFruitsToken = true;
        Slot[] slots = GetComponentsInChildren<Slot>();
        int[] currentindexs = new int[12];
        int index = 0;
        int setindex = 0;
        foreach (Slot slot in slots)
        {
            currentindexs[index] = slot.transform.GetSiblingIndex();
            if (slot.isSlotUse)
            {
                if ((slot.itemInfo._itemData.tokenType & TokenType.fruits) != 0)
                {
                    slot.transform.SetSiblingIndex(setindex);
                    slot.itemInfo.image.color = Color.white;
                    slot.itemInfo.countObject.SetActive(true);
                    slot.itemInfo.button.interactable = true;
                    setindex++;
                }
                else
                {
                    slot.itemInfo.image.color = new Color(1, 1, 1, 0.5f);
                    slot.itemInfo.countObject.SetActive(false);
                    slot.itemInfo.button.interactable = false;
                }
            }
        }
    }

    #endregion

    public void ReturnButton()
    {
        isFilter = false;
        TestManager.instance.isAll = true;
        TestManager.instance.isFront = false;
        TestManager.instance.isBack = false;
        TestManager.instance.isBuilding = false;
        TestManager.instance.isInteration = false;
        HousingSlot[] slots = GetComponentsInChildren<HousingSlot>();
        foreach (HousingSlot slot in slots)
        {
            if (slot.isSlotUse)
            {
                slot.housingInven.image.color = Color.white;
                slot.housingInven.button.interactable = true;
            }
        }
    }

    public void TokenReturnButton()
    {
        isFilter = false;
        TestManager.instance.isAllToken = true;
        TestManager.instance.isSportsToken = false;
        TestManager.instance.isFruitsToken = false;
        Slot[] slots = GetComponentsInChildren<Slot>();
        foreach (Slot slot in slots)
        {
            if (slot.isSlotUse)
            {
                slot.itemInfo.image.color = Color.white;
                slot.itemInfo.button.interactable = true;
            }
        }
    }
}

public enum housingType
{
    back = 2,
    building = 3,
    front = 6,
    interaction = 8
}
