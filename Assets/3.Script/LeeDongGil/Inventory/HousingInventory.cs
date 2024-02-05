using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HousingInventory : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public float touchTime;
    public float minTouchTime = 0.3f;
    public bool isTouch = false;
    public Transform canvas;
    public Transform previousParent;
    public RectTransform rect;
    public CanvasGroup canvasGroup;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        previousParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Start Drag");
        transform.SetParent(canvas);
        transform.SetAsLastSibling();

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

    }

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
        }

        if (previousParent.childCount == 0)
        {
            previousParent.transform.SetAsLastSibling();
        }
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer Down");
        isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Pointer Up");
        isTouch = false;
    }
    private void Update()
    {
        if (isTouch)
        {
            touchTime += Time.deltaTime;
        }
        else
        {
            touchTime = 0;
        }
    }


}
