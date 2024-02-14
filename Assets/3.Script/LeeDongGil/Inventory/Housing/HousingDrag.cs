using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HousingDrag : MonoBehaviour
{
    public bool isDragging = false;
    public bool isCanBuild = false;
    public Vector3 offset;
    public Transform previousParent;
    public CanvasGroup group;

    [Header("Build Setting")]
    public HousingItemData data;
    public SpriteRenderer buildSprite;
    public GameObject space;
    public float spaceX;
    public float spaceY;
    public int id;
    public LayerMask canBuild;
    public LayerMask cannotBuild;
    public LayerMask buildingLayer;


    private int checkMinusY = 1;
    private int checkMinusX = 1;
    public HousingGrid grid;

    private SpriteRenderer check;
    private BoxCollider boxCollider;
    private BoxCollider subCollider;

    public float mouseX = 0;
    public float mouseY = 0;

    #region Gizmos parameter
    /*
        private Vector3 gizmosPosition;
        private Vector3 gizmosDirection;
        private float gizmosDistance;
        private Ray gizmosRay;
        private RaycastHit gizmosHit;
    */
    #endregion

    private void Start()
    {
        previousParent = transform.parent;
        data = TestManager.instance.testHousing[id];
        spaceX = data.housingWidth;
        spaceY = data.housingHeight;

        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        space.transform.localScale = new Vector3(spaceX, spaceY, 1);

        check = transform.GetChild(0).GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        subCollider = transform.GetChild(1).GetComponent<BoxCollider>();
        group = FindObjectOfType<TestNavigationBar>().transform.root.GetComponent<CanvasGroup>();
        grid = FindAnyObjectByType<HousingGrid>();

        boxCollider.size = new Vector3(spaceX, spaceY, 0.2f);
        subCollider.enabled = false;
    }

    private void Update()
    {
        if (TestManager.instance.isEditMode)
        {
            DragObject();
            CheckToBuild();
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            check.gameObject.SetActive(false);
            subCollider.enabled = false;
        }
    }


    private void DragObject()
    {
        Ray ray;
        RaycastHit hit;
        if (Input.touchCount > 0)
        {
            Touch touch;
            touch = Input.GetTouch(0);
            ray = Camera.main.ScreenPointToRay(touch.position);


            if (touch.phase == TouchPhase.Began)
            {
                //Debug.Log("Began 실행");
                if (Physics.Raycast(ray, out hit))
                {
                    if (EventSystem.current.IsPointerOverGameObject(0) == false && hit.collider.gameObject == gameObject)
                    {
                        Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
                        Debug.Log("Began 실행2");
                        offset = transform.position - ray.GetPoint(hit.distance);
                        isDragging = true;
                    }

                    #region Gizmos
                    /*
                                        gizmosHit = hit;
                                        gizmosPosition = ray.origin;
                                        gizmosDirection = ray.direction;
                                        gizmosDistance = hit.distance;
                    */
                    #endregion
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                //Debug.Log("End");
                isDragging = false;
                subCollider.enabled = false;
                if (isCanBuild)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, -0.8f);
                }
            }
            else
            {
                //Debug.Log("Moved 실행");
                if (Physics.Raycast(ray, out hit))
                {
                    if (EventSystem.current.IsPointerOverGameObject() == false && hit.collider.gameObject == gameObject)
                    {
                        Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);

                        group.alpha = 0;
                    }

                }
            }

            if (isDragging)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -1);
                check.gameObject.SetActive(true);
                subCollider.enabled = true;
                Vector3 newPosition = ray.GetPoint(offset.z);
                checkMinusX = newPosition.x >= 0 ? 1 : -1;
                checkMinusY = newPosition.y >= 0 ? 1 : -1;
                float moveX = spaceX % 2 == 0 ? Mathf.RoundToInt(Mathf.Abs(newPosition.x)) * checkMinusX : Mathf.FloorToInt(newPosition.x) + 0.5f;
                float moveY = spaceY % 2 == 0 ? Mathf.RoundToInt(Mathf.Abs(newPosition.y)) * checkMinusY : Mathf.FloorToInt(newPosition.y) + 0.5f;

                float clampX = Mathf.Clamp(moveX, -(grid.boundX / 2) + (spaceX / 2) + grid.posX, (grid.boundX / 2) - (spaceX / 2) + grid.posX);
                float clampY = Mathf.Clamp(moveY, -(grid.boundY / 2) + (spaceY / 2) + grid.posY, (grid.boundY / 2) - (spaceY / 2) + grid.posY);

                //Debug.Log(-(grid.boundX / 2) + spaceX / 2);
                //Debug.Log(-(grid.boundY / 2) + spaceY / 2);

                transform.position = new Vector3(clampX, clampY, transform.position.z);

            }
        }
        else
        {
            //Debug.Log("else End");
            isDragging = false;
            subCollider.enabled = false;
            group.alpha = 1.0f;
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
        int defaultLayer = ~(1 << LayerMask.NameToLayer("Default"));

        bool isHitRT = Physics.Raycast(ray_RT, out hit_RT, distance_RT, defaultLayer);
        bool isHitLT = Physics.Raycast(ray_LT, out hit_LT, distance_LT, defaultLayer);
        bool isHitLB = Physics.Raycast(ray_LB, out hit_LB, distance_LB, defaultLayer);
        bool isHitRB = Physics.Raycast(ray_RB, out hit_RB, distance_RB, defaultLayer);
        bool isHitCenter = Physics.Raycast(ray_Center, out hit_Center, distance_Center, defaultLayer);
        bool isHitBox = Physics.BoxCast(transform.position, new Vector3(spaceX * 0.495f, spaceY * 0.495f, 0.05f), Vector3.forward, out hit_Box, Quaternion.identity, distance_Box, defaultLayer);


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
            !hit_Box.collider.gameObject.CompareTag("Building"))    //올바른 위치에 하우징이 되었을 때
        {
            check.color = new Color32(0, 255, 0, 100);
            transform.SetParent(hit_Center.collider.transform.parent);
            isCanBuild = true;
        }
        else
        {
            check.color = new Color32(255, 0, 0, 100);
            check.gameObject.SetActive(true);
            transform.SetParent(previousParent);
            isCanBuild = false;
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

    public void SetBuild()
    {

    }


    #region OnDrawGizmos
    //OnDrawGizmos
    /*
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            // 함수 파라미터 : 현재 위치, Box의 절반 사이즈, Ray의 방향, RaycastHit 결과, Box의 회전값, BoxCast를 진행할 거리
            if (true == Physics.BoxCast(gizmosPosition, new Vector3(0.495f, 0.495f, 0.2f), gizmosDirection, out gizmosHit, Quaternion.identity))
            {
                // Hit된 지점까지 ray를 그려준다.
                Gizmos.DrawRay(gizmosPosition, gizmosDirection * gizmosHit.distance);

                // Hit된 지점에 박스를 그려준다.
                Gizmos.DrawWireCube(gizmosPosition + gizmosDirection * gizmosHit.distance, new Vector3(0.495f, 0.495f, 0.2f) * 2);
            }
            else
            {
                // Hit가 되지 않았으면 최대 검출 거리로 ray를 그려준다.
                Gizmos.DrawRay(gizmosPosition, gizmosDirection * 20.0f);
            }
        }
    */
    #endregion
}
