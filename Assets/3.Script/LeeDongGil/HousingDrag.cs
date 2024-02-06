using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HousingDrag : MonoBehaviour
{
    public bool isDragging = false;
    public bool isCanBuild = false;
    public Vector3 offset;
    public Transform previousParent;


    public SpriteRenderer check;
    public LayerMask canBuild;
    public LayerMask cannotBuild;

    private void Start()
    {
        check = transform.GetChild(0).GetComponent<SpriteRenderer>();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        previousParent = transform.parent;
    }

    private void Update()
    {
        DragObject();
        CheckToBuild();

    }


    private void DragObject()
    {
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            Touch touch;
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);
            }
            else
            {
                touch = new Touch { position = Input.mousePosition, phase = TouchPhase.Began };
            }
            Debug.Log(touch.phase);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
                {
                    isDragging = true;
                    offset = transform.position - ray.GetPoint(hit.distance);
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                Debug.Log("End");
                isDragging = false;
            }

            if (isDragging)
            {
                Debug.Log("Dragging");
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                Vector3 newPosition = ray.GetPoint(offset.z);
                transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
            }
        }
        else
        {
            isDragging = false;
        }
    }

    private void CheckToBuild()
    {
        Ray ray = new Ray(transform.position, Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2.0f, canBuild))
        {
            isCanBuild = true;
            check.color = new Color32(0, 255, 0, 100);
            transform.SetParent(hit.collider.transform.parent);
        }
        else
        {
            isCanBuild = false;
            check.color = new Color32(255, 0, 0, 100);
            transform.SetParent(previousParent);
        }
    }
}
