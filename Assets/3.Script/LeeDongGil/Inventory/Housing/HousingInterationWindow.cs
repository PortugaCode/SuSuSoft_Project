using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text;

public class HousingInterationWindow : MonoBehaviour
{
    [Header("Window")]
    public InventorySystem housingInvenSys;
    public TextMeshProUGUI housingName;
    public GameObject housingObject;
    public HousingItemData housingData;
    public HousingItemData housingDataWindow;
    public HousingObject housingObj;
    public HousingObject housingObjWindow;

    [Header("PopUp")]
    public GameObject PopupObject;
    public Image P_housingImage;
    public TextMeshProUGUI P_housingName;
    public TextMeshProUGUI P_housingCount;
    public int P_housingCountInt;
    public TextMeshProUGUI P_housingInfo;
    public TextMeshProUGUI P_housingSetName;
    public TextMeshProUGUI P_housingSetItem;
    public List<string> P_housingSetItemList = new List<string>();
    public TextMeshProUGUI P_housingSetInfo;
    public TextMeshProUGUI P_housingEnhanceInfo;


    private void Update()
    {
        CheckHousingObjectToTouch();
        if (!TestManager.instance.isEditMode)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        if (PopupObject.activeSelf)
        {
            UpdatePopUpText();
        }
    }

    public void CheckHousingObjectToTouch()
    {
        // 중근아 여기도 봐
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
                        //housingData = housingObject.GetComponent<HousingDrag>().data;
                        housingObj = this.housingObject.GetComponent<HousingDrag>().housingObject;
                        housingName.text = housingObj.name_k;
                    }
                }

            }

        }
    }

    public void UpdatePopUpText()           //수정
    {
        P_housingSetItemList.Clear();
        foreach (HousingItemData data in TestManager.instance.testHousing)
        {
            if (data.SetName.Equals(housingDataWindow.SetName))
            {
                P_housingSetItemList.Add(data.housingKRName);
            }
        }
        StringBuilder listAdd = new StringBuilder();

        P_housingImage.sprite = housingDataWindow.housingSprite;
        P_housingName.text = string.Format("{0} +{1}", housingDataWindow.housingKRName, housingDataWindow.enhanceLevel);
        P_housingCount.text = string.Format("{0}", P_housingCountInt);
        P_housingInfo.text = housingDataWindow.Info;
        P_housingSetName.text = housingDataWindow.SetName;
        for (int i = 0; i < P_housingSetItemList.Count; i++)
        {
            listAdd.Append(P_housingSetItemList[i]).Append("\n");
            P_housingSetItem.text = listAdd.ToString();
        }
        P_housingSetInfo.text = string.Format("{0} 이/가 {1}만큼 증가합니다.", housingDataWindow.SetEffectName, housingDataWindow.SetEffectValue);
        P_housingEnhanceInfo.text = string.Format("{0} 이/가 {0}만큼 증가합니다.", housingDataWindow.SetEffectName, housingDataWindow.enhanceValue);
    }

    public void InsertHousingInventory()
    {
        housingInvenSys.GetHousingItem(housingObj.index, 1);
        Destroy(housingObject);
        transform.GetChild(0).gameObject.SetActive(false);
    }

}
