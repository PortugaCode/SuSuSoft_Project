using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HousingInventory : MonoBehaviour
{

    [HideInInspector] public Transform canvas;
    [HideInInspector] public Transform previousParent;
    [HideInInspector] public RectTransform rect;
    [HideInInspector] public CanvasGroup canvasGroup;
    [HideInInspector] public Image image;
    [HideInInspector] public Button button;
    [HideInInspector] public HousingDrag drag;
    [HideInInspector] public HousingSlot slot;
    [HideInInspector] public FilterButton filter;

    [Header("Inventory Info")]
    public HousingItemData housingData;         //데이터 연동 이전
    public HousingObject housingObj;
    public string housingName;
    public int count = 0;
    public TextMeshProUGUI countText;
    public GameObject countObject;

    [Header("Building Info")]
    public GameObject Building;
    public Transform buildingSpace;

    private GameObject thisBuilding;

    [Space(30f)]
    public PointerEventData EventData;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        slot = GetComponentInParent<HousingSlot>();
    }

    private void Start()
    {
        
        previousParent = transform.parent;
        if (Building == null)
        {
            button.interactable = false;
            gameObject.SetActive(false);
        }
    }

    #region UI Drag
    /*
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        rect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");
        
        if (transform.parent == canvas)
        {
            transform.SetParent(previousParent);
            rect.position = previousParent.GetComponent<RectTransform>().position;
            transform.localScale = new Vector3(2, 2, 1);        //todo... scale값은 나중에 space값만 변하게 바꾸기
        }

        if (previousParent.childCount == 0)
        {
            previousParent.transform.SetAsLastSibling();
        }
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
        space.SetActive(false);
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer Down");
        isTouch = true;
    }

    
    */
    #endregion

    private void Update()
    {
        ShowSlot();
    }

    private void ShowSlot()
    {
        filter = GetComponentInParent<FilterButton>();

        if (image.color.a <= 0)             //슬롯 알파값이 0보다 작으면 아이템이 없으므로
        {
            button.interactable = false;    //비활성화
            if (!TestManager.instance.isEditMode)
            {
                transform.parent.SetAsLastSibling();
                canvasGroup.alpha = 1.0f;
            }
        }
        else
        {
            if (!filter.isFilter)
            {
                button.interactable = true;
            }
        }

        if (count < 100)
        {
            if (count <= 0)
            {
                image.color = new Color(1, 1, 1, 0);
                countObject.SetActive(false);
            }
            else
            {
                if (!filter.isFilter)
                {
                    image.color = Color.white;
                    countObject.SetActive(true);
                }
                else
                {
                    image.color = new Color(1, 1, 1, image.color.a);
                }
            }
            countText.text = string.Format("{0}", count);
        }
        else
        {
            countText.text = "99+";
        }
    }

    public void BuildSet()
    {
        if (!slot.isWindow)
        {
            image.color = new Color(1, 1, 1, 0);
            DBManager.instance.user.housingObject[housingObj.name_e]--;
            count--;
            if(DBManager.instance.user.housingObject[housingObj.name_e] == 0)
            {
                DBManager.instance.user.housingObject.Remove(housingObj.name_e);
            }
            Vector3 createPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
            thisBuilding = Instantiate(Building, createPos, Quaternion.identity, buildingSpace);
            HousingDrag buildSetting = thisBuilding.GetComponent<HousingDrag>();
            buildSetting.housingObject = housingObj;
            buildSetting.id = housingObj.index;
            buildSetting.buildSprite.sprite = SpriteManager.instance.sprites[housingObj.imageIndex];



            TestManager.instance.isEditMode = true;
        }
    }
}
