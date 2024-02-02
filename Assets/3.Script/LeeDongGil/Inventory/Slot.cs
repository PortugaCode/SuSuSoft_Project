using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerDownHandler
{
    public bool isUpgradeSlot = false;
    public bool IsSlotUse
    {
        get
        {
            return transform.childCount == 1;
        }
    }
    public Transform UpgradeSlot;
    public InventorySystem invenSys;
    public string slotItemName = string.Empty;
    public GameObject itemInfomation;


    private void Update()
    {
        if (IsSlotUse)
        {
            slotItemName = GetComponentInChildren<ItemInfo>()._itemName;
            itemInfomation = GetComponentInChildren<ItemInfo>().gameObject;
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

        if (!isUpgradeSlot)
        {
            Slot[] slots = UpgradeSlot.GetComponentsInChildren<Slot>();
            foreach (Slot upgradeSlot in slots)
            {
                if (upgradeSlot.IsSlotUse)
                {
                    if (upgradeSlot.slotItemName.Equals(itemInfomation.transform.GetComponent<ItemInfo>()._itemName))
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
                    Instantiate(itemInfomation, upgradeSlot.transform);
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


}
