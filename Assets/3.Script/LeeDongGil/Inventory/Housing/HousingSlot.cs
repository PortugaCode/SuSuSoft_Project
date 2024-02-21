using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HousingSlot : MonoBehaviour
{
    public HousingInventory housingInven;


    public bool isSlotUse;
    public string slotItemName = string.Empty;
    public int slotItemCount;
    [Header("Window Inventory")]
    public HousingItemData itemInfomation;                  //데이터 연동 이전
    public HousingObject? housingObject;                     //데이터 연동 이후
    public HousingInterationWindow housingInterationWindow;
    public GameObject popUP;
    public float gameTime = 0;
    public bool isWindow = false;


    private void Start()
    {
        housingInven = GetComponentInChildren<HousingInventory>();
    }
    private void Update()
    {
        if(housingInven.count >= 1)
        {
            isSlotUse = true;
        }
        else
        {
            isSlotUse = false;
        }

        if (isSlotUse)
        {
            slotItemName = GetComponentInChildren<HousingInventory>().housingName;
            slotItemCount = GetComponentInChildren<HousingInventory>().count;
            //itemInfomation = GetComponentInChildren<HousingInventory>().housingData;
            housingObject = GetComponentInChildren<HousingInventory>().housingObj;
        }
        else
        {
            slotItemName = string.Empty;
            itemInfomation = null;
            housingObject = null;
        }
    }


    public void InteractionPopUp()
    {
        if (isWindow)
        {
            popUP.transform.position = transform.position;
            housingInterationWindow.housingDataWindow = itemInfomation;
            housingInterationWindow.P_housingCountInt = slotItemCount;
            popUP.SetActive(true);
        }
    }
}
