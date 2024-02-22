using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilterButton : MonoBehaviour
{
    public Transform scrollInventory;
    public Transform windowInventory;
    public bool isFilter = false;
    public Button btn;
    //전체적으로 수정
    public void AllButton()
    {
        isFilter = false;
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

        HousingSlot[] slots = GetComponentsInChildren<HousingSlot>();
        int[] currentindexs = new int[20];
        int index = 0;
        int setindex = 0;
        foreach (HousingSlot slot in slots)
        {
            currentindexs[index] = slot.transform.GetSiblingIndex();
            if (slot.isSlotUse)
            {
                if(slot.housingObject.Value.type.Equals("전경"))
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

    public void SpecialButton()
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
                if (slot.itemInfomation.housingType == HousingType.special)
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

    public void TokenButton()
    {
        isFilter = true;
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

    public void ReturnButton()
    {
        isFilter = false;
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

}
