using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerDownHandler
{
    public bool isUpgradeSlot = false;
    [SerializeField] public bool IsSlotUse
    {
        get
        {
            return transform.GetComponentInChildren<ItemInfo>()._itemCount >= 1;
        }
    }
    public Transform UpgradeSlot;
    public InventorySystem invenSys;
    public string slotItemName = string.Empty;
    public ItemData itemInfomation;


    private void Update()
    {
        if (IsSlotUse)
        {
            slotItemName = GetComponentInChildren<ItemInfo>()._itemName;
            itemInfomation = GetComponentInChildren<ItemInfo>()._itemData;
        }
        else
        {
            slotItemName = string.Empty;
            itemInfomation = null;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //if (!IsSlotUse) return;
        Debug.Log(IsSlotUse);
        if (!isUpgradeSlot)
        {
            Slot[] slots = UpgradeSlot.GetComponentsInChildren<Slot>();
            foreach (Slot upgradeSlot in slots)
            {
                if (upgradeSlot.IsSlotUse)
                {
                    if (upgradeSlot.slotItemName.Equals(itemInfomation.itemName))
                    {
                        Debug.Log("아이템 추가");
                        upgradeSlot.GetComponentInChildren<ItemInfo>()._itemCount++;
                        eventData.pointerCurrentRaycast.gameObject.transform.GetComponentInChildren<ItemInfo>()._itemCount--;
                        break;
                    }
                    else continue;
                }
                else
                {
                    SetUpgradeItemInfo(itemInfomation, upgradeSlot);
                    upgradeSlot.GetComponentInChildren<ItemInfo>()._itemCount = 1;
                    eventData.pointerCurrentRaycast.gameObject.transform.GetComponentInChildren<ItemInfo>()._itemCount--;
                    Debug.Log("아이템 생성");
                    break;
                }
            }
        }
        else
        {
            eventData.pointerCurrentRaycast.gameObject.transform.GetComponentInChildren<ItemInfo>()._itemCount--;
        }
    }

    public void SetUpgradeItemInfo(ItemData itemData, Slot _slot)
    {
        _slot.transform.GetChild(0).GetComponent<Image>().sprite = itemData.sprite;
        _slot.transform.GetComponentInChildren<ItemInfo>()._itemData = itemData;
        _slot.transform.GetComponentInChildren<ItemInfo>()._itemName = itemData.itemName;
    }
}
