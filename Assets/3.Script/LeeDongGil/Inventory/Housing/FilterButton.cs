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

    private void OnEnable()
    {
        Debug.Log("필터 버튼이 활성화 될 때 이 디버그가 표시됨");
        if (TestManager.instance.isAll)
        {
            AllButton();
        }
        else if (TestManager.instance.isFront)
        {
            FrontButton();
        }
        else if (TestManager.instance.isBack)
        {
            BackButton();
        }
        else if (TestManager.instance.isBuilding)
        {
            BuildingButton();
        }
        else if (TestManager.instance.isInteration)
        {
            InteractionButton();
        }

    }

    private void OnDisable()
    {
        Debug.Log("필터 버튼이 비활성화 될 때 이 디버그가 표시됨");

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
            if (slot.isSlotUse)
            {
                if (slot.housingObject.Value.type.Equals("전경"))
                {
                    slot.transform.SetSiblingIndex(setindex);
                    slot.housingInven.image.color = Color.white;
                    slot.housingInven.countObject.SetActive(true);
                    slot.housingInven.button.interactable = true;
                    setindex++;
                }
                else
                {
                    slot.housingInven.image.color = new Color(1, 1, 1, 0.1f);
                    slot.housingInven.countObject.SetActive(false);
                    slot.housingInven.button.interactable = false;
                }
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
            if (slot.isSlotUse)
            {
                if (slot.housingObject.Value.type.Equals("후경"))
                {
                    slot.transform.SetSiblingIndex(setindex);
                    slot.housingInven.image.color = Color.white;
                    slot.housingInven.countObject.SetActive(true);
                    slot.housingInven.button.interactable = true;
                    setindex++;
                }
                else
                {
                    slot.housingInven.image.color = new Color(1, 1, 1, 0.1f);
                    slot.housingInven.countObject.SetActive(false);
                    slot.housingInven.button.interactable = false;
                }
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
            if (slot.isSlotUse)
            {
                if (slot.housingObject.Value.type.Equals("건물"))
                {
                    slot.transform.SetSiblingIndex(setindex);
                    slot.housingInven.image.color = Color.white;
                    slot.housingInven.countObject.SetActive(true);
                    slot.housingInven.button.interactable = true;
                    setindex++;
                }
                else
                {
                    slot.housingInven.image.color = new Color(1, 1, 1, 0.1f);
                    slot.housingInven.countObject.SetActive(false);
                    slot.housingInven.button.interactable = false;
                }
            }
        }
    }

    public void ConstellationButton()
    {
        isFilter = true;
        HousingSlot[] slots = GetComponentsInChildren<HousingSlot>();
        int[] currentindexs = new int[20];
        int index = 0;
        int setindex = 0;
        foreach (HousingSlot slot in slots)
        {
            currentindexs[index] = slot.transform.GetSiblingIndex();
            if (slot.isSlotUse)
            {
                if (slot.itemInfomation.housingType == HousingType.constellation)
                {
                    slot.transform.SetSiblingIndex(setindex);
                    slot.housingInven.image.color = Color.white;
                    slot.housingInven.countObject.SetActive(true);
                    slot.housingInven.button.interactable = true;
                    setindex++;
                }
                else
                {
                    slot.housingInven.image.color = new Color(1, 1, 1, 0.1f);
                    slot.housingInven.countObject.SetActive(false);
                    slot.housingInven.button.interactable = false;
                }
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
            if (slot.isSlotUse)
            {
                if (slot.housingObject.Value.type.Equals("상호작용"))
                {
                    slot.transform.SetSiblingIndex(setindex);
                    slot.housingInven.image.color = Color.white;
                    slot.housingInven.countObject.SetActive(true);
                    slot.housingInven.button.interactable = true;
                    setindex++;
                }
                else
                {
                    slot.housingInven.image.color = new Color(1, 1, 1, 0.1f);
                    slot.housingInven.countObject.SetActive(false);
                    slot.housingInven.button.interactable = false;
                }
            }
        }
    }

    #endregion

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
                    slot.itemInfo.image.color = new Color(1, 1, 1, 0.1f);
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
                    slot.itemInfo.image.color = new Color(1, 1, 1, 0.1f);
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
