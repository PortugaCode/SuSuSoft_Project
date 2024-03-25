using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text;

public class HousingInterationWindow : MonoBehaviour
{
    public bool isFirstSet = false;

    [Header("Window")]
    public InventorySystem housingInvenSys;
    public TextMeshProUGUI housingName;
    public TextMeshProUGUI firstHousingName;
    public TextMeshProUGUI housingSetTag;
    public TextMeshProUGUI firstHousingSetTag;
    public GameObject housingObject;
    public HousingObject housingObj;
    public HousingObject housingObjWindow;
    public Button insertBTN;
    public Button setBTN;
    public Button firstSetBTN;
    public GameObject firstWindow;
    public GameObject window;
    public HousingDrag housingDrag;

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

    [Header("Housing UI")]
    public ToggleHousingInventory housingToggle;


    private void Update()
    {
        CheckHousingObjectToTouch();
        if (!TestManager.instance.isEditMode)
        {
            window.SetActive(false);
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
                        housingObject = hit.collider.gameObject;
                        housingDrag = housingObject.GetComponent<HousingDrag>();
                        housingObj = housingDrag.housingObject;
                        housingName.text = housingObj.name_k;
                        firstHousingName.text = housingObj.name_k;
                    }
                }

            }

        }
        if (housingObject != null)
        {
            if (housingObject.GetComponent<HousingDrag>().isCanBuild)
            {
                setBTN.interactable = true;
                firstSetBTN.interactable = true;
            }
            else
            {
                setBTN.interactable = false;
                firstSetBTN.interactable = false;
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
        P_housingName.text = string.Format("{0} +{1}", housingObjWindow.name_k, housingObjWindow.level);
        //P_housingCount.text = string.Format("{0}", DBManager.instance.user.housingObject[housingObjWindow.name_e]);
        P_housingCount.text = string.Format("{0}개", P_housingCountInt);
        P_housingInfo.text = housingObjWindow.text_k;

        P_housingSetName.text = HousingSetNameToKR();

        for (int i = 0; i < P_housingSetItemList.Count; i++)
        {
            listAdd.Append($"{P_housingSetItemList[i]} ");
            if (DBManager.instance.user.housingObject.ContainsKey(housingObjWindow.name_e))
            {
                listAdd.Append("   보유");
            }
            else if (LoadHousing.instance.localHousingObject.ContainsValue(housingObjWindow))       //여기 DBManager로 바꿔야함
            {
                listAdd.Append("   배치");
            }
            else
            {
                listAdd.Append("   미보유");
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
        if (DBManager.instance.user.housingObject.ContainsKey(housingObj.name_e))
        {
            DBManager.instance.user.housingObject[housingObj.name_e] += 1;
        }
        else
        {
            DBManager.instance.user.housingObject.Add(housingObj.name_e, 1);
        }
        DBManager.instance.RemoveMyHousingObject(housingObj.index, housingDrag.new_x, housingDrag.new_y);
        housingInvenSys.LoadHousingInventory();
        //housingInvenSys.GetHousingItem(housingObj.index, 1);
        housingObject.GetComponent<HousingDrag>().isInsertInven = true;
        Destroy(housingObject);
        //TestManager.instance.isShowUI = true;
        isFirstSet = false;
        housingToggle.openButton.interactable = true;
        window.SetActive(false);
        TestManager.instance.housingInven.SetActive(true);
    }
    public void SetHousingPosition()
    {
        if (housingObject.GetComponent<HousingDrag>().isSetBuild)
        {
            housingObject.GetComponent<HousingDrag>().isSetBuild = false;
            housingSetTag.text = "배치 완료";
        }
        else
        {
            housingSetTag.text = "배치 옮기기";
            housingObject.GetComponent<HousingDrag>().isSetBuild = true;
            housingObject.GetComponent<HousingDrag>().space.SetActive(false);
            housingObject.GetComponent<HousingDrag>().subCollider.enabled = false;
            housingObject.GetComponent<HousingDrag>().SetZ(housingObject.GetComponent<HousingDrag>().currentLayer_);
            DBManager.instance.MoveMyHousingObject(housingObj.index, housingDrag.original_x, housingDrag.original_y, housingDrag.new_x, housingDrag.new_y);
            isFirstSet = false;
            housingToggle.openButton.interactable = true;
            window.SetActive(false);
            housingToggle.housingInventory.SetActive(true);
            //TestManager.instance.isShowUI = true;
        }
    }

    public void FirstInsertHousingInventory()
    {
        housingInvenSys.LoadHousingInventory();
        //housingInvenSys.GetHousingItem(housingObj.index, 1);
        housingObject.GetComponent<HousingDrag>().isInsertInven = true;
        Destroy(housingObject);
        //TestManager.instance.isShowUI = true;
        isFirstSet = false;
        housingToggle.openButton.interactable = true;
        firstWindow.SetActive(false);
        TestManager.instance.housingInven.SetActive(true);
    }


    public void FirstSetHousingPosition()
    {
        housingObject.GetComponent<HousingDrag>().isSetBuild = true;
        housingObject.GetComponent<HousingDrag>().space.SetActive(false);
        housingObject.GetComponent<HousingDrag>().subCollider.enabled = false;
        housingObject.GetComponent<HousingDrag>().SetZ(housingObject.GetComponent<HousingDrag>().currentLayer_);
        DBManager.instance.user.housingObject[housingObj.name_e]--;
        if (DBManager.instance.user.housingObject[housingObj.name_e] == 0)
        {
            DBManager.instance.user.housingObject.Remove(housingObj.name_e);
        }
        DBManager.instance.AddMyHousingObject(housingObj.index, housingObject.transform.position.x, housingObject.transform.position.y);
        isFirstSet = false;
        housingToggle.openButton.interactable = true;
        firstWindow.SetActive(false);
        housingToggle.housingInventory.SetActive(true);
        //TestManager.instance.isShowUI = true;
    }

}
