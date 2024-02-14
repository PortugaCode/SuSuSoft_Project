using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class HousingInterationWindow : MonoBehaviour
{
    public InventorySystem housingInvenSys;
    public TextMeshProUGUI housingName;
    public GameObject housingObject;
    public HousingItemData housingData;


    private void Update()
    {
        if (TestManager.instance.isEditMode)
        {
            CheckHousingObjectToTouch();
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void CheckHousingObjectToTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray touchRay = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (Physics.Raycast(touchRay, out hit))
            {
                if (!EventSystem.current.IsPointerOverGameObject(0))
                {
                    if (hit.collider.gameObject.CompareTag("Building"))
                    {
                        housingObject = hit.collider.gameObject;
                        housingData = housingObject.GetComponent<HousingDrag>().data;
                        housingName.text = housingObject.GetComponent<HousingDrag>().data.housingKRName;
                    }
                    else
                    {
                        Debug.Log("여기 들어오면 안되는데");
                    }
                }

            }

        }
    }

    public void InsertHousingInventory()
    {
        housingInvenSys.GetHousingItem_test(housingData, 1);
        Destroy(housingObject);
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
