using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HousingDrag : MonoBehaviour
{
    public bool isDragging = false;
    public bool isCanBuild = false;
    public Vector3 offset;
    public Transform previousParent;

    public GameObject space;
    public float spaceX;
    public float spaceY;
    private int checkMinusY = 1;
    private int checkMinusX = 1;

    private SpriteRenderer check;
    public LayerMask canBuild;
    public LayerMask cannotBuild;
    public LayerMask buildingLayer;

    private void Start()
    {
        check = transform.GetChild(0).GetComponent<SpriteRenderer>();
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        previousParent = transform.parent;

        space.transform.localScale = new Vector3(spaceX, spaceY, 1);
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
                check.gameObject.SetActive(true);
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                Vector3 newPosition = ray.GetPoint(offset.z);

                checkMinusX = newPosition.x >= 0 ? 1 : -1;
                checkMinusY = newPosition.y >= 0 ? 1 : -1;
                float moveX = spaceX % 2 == 0 ? Mathf.RoundToInt(Mathf.Abs(newPosition.x)) * checkMinusX : Mathf.FloorToInt(newPosition.x) + 0.5f;
                float moveY = spaceY % 2 == 0 ? Mathf.RoundToInt(Mathf.Abs(newPosition.y)) * checkMinusY : Mathf.FloorToInt(newPosition.y) + 0.5f;
                
                transform.position = new Vector3(moveX, moveY, -1);
            }
        }
        else
        {
            isDragging = false;
        }
    }

    private void CheckToBuild()
    {
        Vector3 point_RT = new Vector3(transform.position.x - 0.01f + spaceX * 0.5f, transform.position.y - 0.01f + spaceY * 0.5f, transform.position.z);
        Vector3 point_LT = new Vector3(transform.position.x + 0.01f - spaceX * 0.5f, transform.position.y - 0.01f + spaceY * 0.5f, transform.position.z);
        Vector3 point_LB = new Vector3(transform.position.x + 0.01f - spaceX * 0.5f, transform.position.y + 0.01f - spaceY * 0.5f, transform.position.z);
        Vector3 point_RB = new Vector3(transform.position.x - 0.01f + spaceX * 0.5f, transform.position.y + 0.01f - spaceY * 0.5f, transform.position.z);

        Ray ray_RT = new Ray(point_RT, Vector3.forward);
        Ray ray_LT = new Ray(point_LT, Vector3.forward);
        Ray ray_LB = new Ray(point_LB, Vector3.forward);
        Ray ray_RB = new Ray(point_RB, Vector3.forward);
        Ray ray_Center = new Ray(transform.position, Vector3.forward);

        float distance_RT = 20f;
        float distance_LT = 20f;
        float distance_LB = 20f;
        float distance_RB = 20f;
        float distance_Center = 20f;
        float distance_Box = 20f;

        RaycastHit hit_RT, hit_LT, hit_LB, hit_RB, hit_Center, hit_Box;

        bool isHitRT = Physics.Raycast(ray_RT, out hit_RT, distance_RT, ~(1 << LayerMask.NameToLayer("Default")));
        bool isHitLT = Physics.Raycast(ray_LT, out hit_LT, distance_LT, ~(1 << LayerMask.NameToLayer("Default")));
        bool isHitLB = Physics.Raycast(ray_LB, out hit_LB, distance_LB, ~(1 << LayerMask.NameToLayer("Default")));
        bool isHitRB = Physics.Raycast(ray_RB, out hit_RB, distance_RB, ~(1 << LayerMask.NameToLayer("Default")));
        bool isHitCenter = Physics.Raycast(ray_Center, out hit_Center, distance_Center, ~(1 << LayerMask.NameToLayer("Default")));
        bool isHitBox = Physics.BoxCast(transform.position, new Vector3(spaceX * 0.495f, spaceY * 0.495f, 0.05f), Vector3.forward, out hit_Box, Quaternion.identity, distance_Box, ~(1 << LayerMask.NameToLayer("Default")));

        
        distance_RT = hit_RT.distance;
        distance_LT = hit_LT.distance;
        distance_LB = hit_LB.distance;
        distance_RB = hit_RB.distance;
        distance_Center = hit_Center.distance;
        distance_Box = hit_Box.distance;

        if (hit_RT.collider.CompareTag("CanBuild") &&
            hit_LT.collider.CompareTag("CanBuild") &&
            hit_LB.collider.CompareTag("CanBuild") &&
            hit_RB.collider.CompareTag("CanBuild") &&
            hit_Center.collider.CompareTag("CanBuild") &&
            !hit_Box.collider.gameObject.CompareTag("Building"))
        {
            isCanBuild = true;
            check.color = new Color32(0, 255, 0, 100);
            if (!isDragging)
            {
                transform.SetParent(hit_Center.collider.transform.parent);
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                check.gameObject.SetActive(false);
            }
        }
        else
        {
            isCanBuild = false;
            check.color = new Color32(255, 0, 0, 100);
            transform.SetParent(previousParent);
        }

        #region Draw Ray
        Debug.DrawRay(point_RT, Vector3.forward * distance_RT, Color.red);
        Debug.DrawRay(point_LT, Vector3.forward * distance_LT, Color.red);
        Debug.DrawRay(point_LB, Vector3.forward * distance_LB, Color.red);
        Debug.DrawRay(point_RB, Vector3.forward * distance_RB, Color.red);
        Debug.DrawRay(transform.position, Vector3.forward * distance_Center, Color.red);

        //Debug.Log("RT : " + distance_RT);
        //Debug.Log("LT : " + distance_LT);
        //Debug.Log("LB : " + distance_LB);
        //Debug.Log("RB : " + distance_RB);
        #endregion
    }


}
