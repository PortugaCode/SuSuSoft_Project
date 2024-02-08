using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject item;

    public Transform previousParent;
    public Transform canvasTransform;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        previousParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(canvasTransform);
        transform.SetAsLastSibling();
        Debug.Log("Drag");
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == canvasTransform)
        {
            transform.SetParent(previousParent);
            rect.position = previousParent.GetComponent<RectTransform>().position;
        }
        Debug.Log("End Drag");
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
    }
}
