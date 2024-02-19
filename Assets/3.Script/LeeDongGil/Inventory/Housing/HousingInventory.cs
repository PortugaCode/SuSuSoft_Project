using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HousingInventory : MonoBehaviour
{
    [HideInInspector]
    public Transform canvas;
    public Transform previousParent;
    public RectTransform rect;
    public CanvasGroup canvasGroup;
    public Image image;
    public Button button;
    public HousingDrag drag;
    public HousingSlot slot;
    public FilterButton filter;

    [Header("Inventory Info")]
    public HousingItemData housingData;
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



        /*if (TestManager.instance.isEditMode)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }*/
    }

    private void ShowSlot()
    {
        filter = GetComponentInParent<FilterButton>();

        if (image.color.a <= 0)
        {
            button.interactable = false;
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
            count--;
            Vector3 createPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
            thisBuilding = Instantiate(Building, createPos, Quaternion.identity, buildingSpace);
            HousingDrag buildSetting = thisBuilding.GetComponent<HousingDrag>();
            buildSetting.id = housingData.housingID;
            buildSetting.buildSprite.sprite = housingData.housingSprite;



            TestManager.instance.isEditMode = true;
        }
    }
}
