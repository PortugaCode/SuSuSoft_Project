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
    public Transform InventorySlot;
    public InventorySystem invenSys;
    public string slotItemName = string.Empty;
    public int slotItemCount = 0;
    public ItemData itemInfomation;


    private void Update()
    {
        if (IsSlotUse)
        {
            slotItemName = GetComponentInChildren<ItemInfo>()._itemName;
            slotItemCount = GetComponentInChildren<ItemInfo>()._itemCount;
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
        if (!IsSlotUse) return;
        Debug.Log(IsSlotUse);
        if (!isUpgradeSlot)         //�κ��丮 ������ ��
        {
            Slot[] upgradeSlots = UpgradeSlot.GetComponentsInChildren<Slot>();
            foreach (Slot upgradeSlot in upgradeSlots)         //���׷��̵� ���� ����ִ� ���� �˻�
            {
                if (upgradeSlot.IsSlotUse)
                {
                    if (upgradeSlot.slotItemName.Equals(itemInfomation.itemName))
                    {
                        Debug.Log("������ �߰�");
                        upgradeSlot.GetComponentInChildren<ItemInfo>()._itemCount++;
                        eventData.pointerCurrentRaycast.gameObject.transform.GetComponentInChildren<ItemInfo>()._itemCount--;
                        break;
                    }
                    else continue;
                }
                else
                {
                    SetItem(itemInfomation, upgradeSlot);
                    upgradeSlot.GetComponentInChildren<ItemInfo>()._itemCount = 1;
                    eventData.pointerCurrentRaycast.gameObject.transform.GetComponentInChildren<ItemInfo>()._itemCount--;
                    Debug.Log("������ ����");
                    break;
                }
            }
        }
        else
        {   
            Slot[] invenSlots = InventorySlot.GetComponentsInChildren<Slot>();
            foreach(Slot invenSlot in invenSlots)
            {
                if(invenSlot.IsSlotUse)
                {
                    if (invenSlot.slotItemName.Equals(itemInfomation.itemName))
                    {
                        invenSlot.GetComponentInChildren<ItemInfo>()._itemCount++;
                        eventData.pointerCurrentRaycast.gameObject.transform.GetComponentInChildren<ItemInfo>()._itemCount--;
                        break;
                    }
                    else continue;
                }
                else
                {
                    SetItem(itemInfomation, invenSlot);
                    invenSlot.GetComponentInChildren<ItemInfo>()._itemCount = 1;
                    eventData.pointerCurrentRaycast.gameObject.transform.GetComponentInChildren<ItemInfo>()._itemCount--;
                    break;
                }
            }
        }
    }


    public void SetItem(ItemData itemData, Slot _slot)
    {
        _slot.transform.GetChild(0).GetComponent<Image>().sprite = itemData.sprite;
        _slot.transform.GetComponentInChildren<ItemInfo>()._itemData = itemData;
        _slot.transform.GetComponentInChildren<ItemInfo>()._itemName = itemData.itemName;
    }

}
