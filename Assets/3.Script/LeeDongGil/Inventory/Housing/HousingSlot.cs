using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HousingSlot : MonoBehaviour
{
    public HousingInventory housingInven;
    public bool isSlotUse;
    public string slotItemName = string.Empty;
    public int slotItemCount;
    [Header("Window Inventory")]
    public HousingItemData itemInfomation;                  //데이터 연동 이전
    public HousingObject housingObject;                     //데이터 연동 이후
    public HousingInterationWindow housingInterationWindow;
    public GameObject popUP;
    public float gameTime = 0;
    public bool isWindow = false;
    public GraphicRaycaster gRay;
    private PointerEventData eventData;

    private void Start()
    {
        gRay = transform.root.GetComponent<GraphicRaycaster>();
        housingInven = GetComponentInChildren<HousingInventory>();

        if (housingInven.count >= 1)
        {
            isSlotUse = true;
        }
        else
        {
            isSlotUse = false;
        }
    }
    private void Update()
    {
        if (housingInven.count >= 1)
        {
            isSlotUse = true;
        }
        else
        {
            isSlotUse = false;
        }

        if (isSlotUse)
        {
            slotItemName = housingInven.housingName;
            slotItemCount = housingInven.count;
            if (TestManager.instance.isAll)
            {
                housingInven.image.color = Color.white;
            }
            else if (TestManager.instance.isFront)
            {
                if (housingInven.housingObj.type.Equals("전경"))
                {
                    housingInven.image.color = Color.white;
                }
            }
            else if (TestManager.instance.isBack)
            {
                if (housingInven.housingObj.type.Equals("후경"))
                {
                    housingInven.image.color = Color.white;
                }
            }
            else if (TestManager.instance.isBuilding)
            {
                if (housingInven.housingObj.type.Equals("건물"))
                {
                    housingInven.image.color = Color.white;
                }
            }
            else if (TestManager.instance.isInteration)
            {
                if (housingInven.housingObj.type.Equals("상호작용"))
                {
                    housingInven.image.color = Color.white;
                }
            }
            //itemInfomation = housingInven.housingData;
            housingObject = housingInven.housingObj;
        }
        else
        {
            slotItemName = string.Empty;
            slotItemCount = housingInven.count;
            housingInven.image.color = new Color(1, 1, 1, 0);
            itemInfomation = null;
        }

        //if (popUP.activeSelf)
        //{
        //    PopUpClose();
        //}
    }


    public void InteractionPopUp()
    {
        if (isWindow)
        {
            popUP.transform.position = transform.position;
            housingInterationWindow.housingObjWindow = housingObject;
            housingInterationWindow.P_housingCountInt = slotItemCount;
            popUP.SetActive(true);
        }
    }

    private void PopUpClose()
    {
        if (Input.touchCount > 0)
        {
            Touch touch;
            touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (EventSystem.current.IsPointerOverGameObject(0) == false)
                {
                    popUP.SetActive(false);
                }
            }
        }
    }
}
