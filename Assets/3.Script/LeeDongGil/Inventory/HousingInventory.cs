using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HousingInventory : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Touch Time")]
    public float touchTime;
    public float minTouchTime = 0.3f;
    public bool isTouch = false;

    [Header("Drag System")]
    public Transform canvas;
    public Transform previousParent;
    public RectTransform rect;
    public CanvasGroup canvasGroup;
    public Image image;
    public HousingDrag drag;

    [Header("Spacing")]
    public GameObject Building;
    public Transform buildingSpace;

    [Space(30f)]
    public PointerEventData EventData;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    private void Start()
    {
        previousParent = transform.parent;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        /*
        transform.SetParent(canvas);
        transform.SetAsLastSibling();

        space.SetActive(true);
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        transform.localScale = new Vector3(sizeX * 0.01f, sizeY * 0.01f, 1);        //todo... scale값은 나중에 space값만 변하게 바꾸기
        */

        Debug.Log("Start Drag");
        image.color = new Color(1, 1, 1, 0);
        Instantiate(Building, Camera.main.ScreenToWorldPoint(eventData.position), Quaternion.identity, buildingSpace);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Pointer Up");
        /*if (buildingSpace.childCount > 0)
        {
            image.color = Color.white;
            Destroy(buildingSpace.GetChild(0).gameObject);
        }
        else
        {
            transform.SetAsLastSibling();
            gameObject.SetActive(false);
        }*/
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
        if(image.color.a <= 0 && buildingSpace.childCount == 0)
        {
            transform.parent.SetAsLastSibling();
            gameObject.SetActive(false);
        }
    }
}
