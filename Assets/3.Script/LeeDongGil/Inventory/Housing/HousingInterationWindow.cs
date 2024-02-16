using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HousingInterationWindow : MonoBehaviour
{
    [Header("Window")]
    public InventorySystem housingInvenSys;
    public TextMeshProUGUI housingName;
    public GameObject housingObject;
    public HousingItemData housingData;


    [Header("PopUp")]
    public Image P_housingImage;
    public TextMeshProUGUI P_housingName;
    public TextMeshProUGUI P_housingCount;
    public int P_housingCountInt;
    public TextMeshProUGUI P_housingInfo;
    public TextMeshProUGUI P_housingSetName;
    public TextMeshProUGUI P_housingSetItem;
    public TextMeshProUGUI P_housingSetInfo;
    public TextMeshProUGUI P_housingEnhanceInfo;

    private void Update()
    {
        CheckHousingObjectToTouch();
        if (!TestManager.instance.isEditMode)
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
                        housingName.text = housingData.housingKRName;
                    }
                }

            }

        }
    }

    public void UpdatePopUpText()
    {
        P_housingImage.sprite = housingObject.GetComponent<SpriteRenderer>().sprite;
        P_housingName.text = string.Format("{0} +{1}", housingData.housingKRName, housingData.enhanceLevel);
        P_housingCount.text = "1";
        P_housingInfo.text = housingData.Info;
        P_housingSetName.text = housingData.SetEffectName;
        P_housingSetItem.text = "11";
        P_housingSetInfo.text = string.Format("~~가 {0}만큼 증가합니다.", housingData.SetEffectValue);
        P_housingEnhanceInfo.text = string.Format("~~가 {0}만큼 증가합니다.", housingData.enhanceValue);





    }

    public void InsertHousingInventory()
    {
        housingInvenSys.GetHousingItem_test(housingData, 1);
        Destroy(housingObject);
        transform.GetChild(0).gameObject.SetActive(false);
    }

}
