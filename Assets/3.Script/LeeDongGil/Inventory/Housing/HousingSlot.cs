using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HousingSlot : MonoBehaviour
{
    public HousingInventory housingInven;
    [SerializeField]
    public bool IsSlotUse
    {
        get
        {
            return transform.GetComponentInChildren<HousingInventory>().count >= 1;
        }
    }
    public string slotItemName = string.Empty;
    public int slotItemCount;
    [Header("Window Inventory")]
    public HousingItemData itemInfomation;
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
        if (IsSlotUse)
        {
            slotItemName = GetComponentInChildren<HousingInventory>().housingName;
            slotItemCount = GetComponentInChildren<HousingInventory>().count;
            itemInfomation = GetComponentInChildren<HousingInventory>().housingData;
        }
        else
        {
            slotItemName = string.Empty;
            itemInfomation = null;
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
