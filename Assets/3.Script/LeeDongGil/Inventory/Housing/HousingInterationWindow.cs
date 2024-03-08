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
    public Button insertBTN;
    public Button setBTN;

    [Header("PopUp")]
    public GameObject PopupObject;                                      //윈도우 오브젝트
    public Image P_housingImage;                                        //하우징 스프라이트 이미지
    public TextMeshProUGUI P_housingName;                               //하우징 이름
    public TextMeshProUGUI P_housingCount;                              //하우징 개수 텍스트
    public int P_housingCountInt;                                       //하우징 개수
    public TextMeshProUGUI P_housingInfo;                               //하우징 설명
    public TextMeshProUGUI P_housingSetName;                            //하우징 세트 이름
    public TextMeshProUGUI P_housingSetItem;                            //하우징 세트 아이템 리스트
    public List<string> P_housingSetItemList = new List<string>();
    public TextMeshProUGUI P_housingSetInfo;                            //하우징 세트효과 설명
    public TextMeshProUGUI P_housingEnhanceInfo;                        //하우징 세트 강화효과 정보


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
                        //housingData = housingObject.GetComponent<HousingDrag>().data;
                        Debug.Log($"{housingObj.index}번 {housingObj.name_e}");
                        housingObject = hit.collider.gameObject;
                        housingObj = this.housingObject.GetComponent<HousingDrag>().housingObject;
                        housingName.text = housingObj.name_k;
                        if (housingObject.GetComponent<HousingDrag>().isCanBuild)
                        {
                            setBTN.interactable = true;
                        }
                        else
                        {
                            setBTN.interactable = false;
                        }
                    }
                }

            }

        }
    }

    public void UpdatePopUpText()           //수정
    {
        P_housingSetItemList.Clear();
        foreach (HousingObject housing in ChartManager.instance.housingObjectDatas)        //같은 세트 아이템 리스트 추가
        {
            if (housing.setType.Equals(housingObjWindow.setType))
            {
                if (housing.setType == string.Empty) continue;
                P_housingSetItemList.Add(housing.name_k);
            }
        }
        StringBuilder listAdd = new StringBuilder();

        P_housingImage.sprite = SpriteManager.instance.sprites[housingObjWindow.imageIndex];
        P_housingName.text = string.Format("{0} +{1}", housingObjWindow.name_k, "임시");
        //P_housingCount.text = string.Format("{0}", DBManager.instance.user.housingObject[housingObjWindow.name_e]);
        P_housingCount.text = string.Format("{0}", P_housingCountInt);
        P_housingInfo.text = housingObjWindow.text_k;

        P_housingSetName.text = HousingSetNameToKR();

        for (int i = 0; i < P_housingSetItemList.Count; i++)
        {
            listAdd.Append($"{P_housingSetItemList[i]} ");
            if (DBManager.instance.user.housingObject.ContainsKey(housingObjWindow.name_e))
            {
                listAdd.Append("보유");
            }
            else if (LoadHousing.instance.localHousingObject.ContainsValue(housingObjWindow))       //여기 DBManager로 바꿔야함
            {
                listAdd.Append("배치");
            }
            listAdd.Append("\n");
            P_housingSetItem.text = listAdd.ToString();
        }
        if (housingObjWindow.setType != string.Empty)
        {
            P_housingSetInfo.text = string.Format("{0} 이/가 {1}만큼 증가합니다.", HousingSetEffectNameToKR(), "10(임시)");
            P_housingEnhanceInfo.text = string.Format("{0} 이/가 {1}만큼 증가합니다.", HousingSetEffectNameToKR(), "10(임시)");
        }
        else
        {
            P_housingSetInfo.text = string.Empty;
            P_housingEnhanceInfo.text = string.Empty;
        }
    }

    public string HousingSetNameToKR()
    {
        switch (housingObjWindow.setType)
        {
            case "Wood":
                return "나무 세트";
            case "Sports":
                return "스포츠 세트";
            default:
                return string.Empty;
        }
    }

    public string HousingSetEffectNameToKR()
    {
        switch (housingObjWindow.setType)
        {
            case "Wood":
                return "골드 획득량";
            case "Sports":
                return "최대 체력";
            default:
                return string.Empty;
        }
    }

    public void InsertHousingInventory()
    {
        housingInvenSys.GetHousingItem(housingObj.index, 1);
        housingObject.GetComponent<HousingDrag>().isInsertInven = true;
        Destroy(housingObject);
        TestManager.instance.isShowUI = true;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void SetHousingPosition()
    {
        housingObject.GetComponent<HousingDrag>().isSetBuild = true;
        transform.GetChild(0).gameObject.SetActive(false);
        TestManager.instance.isShowUI = true;
    }

}
