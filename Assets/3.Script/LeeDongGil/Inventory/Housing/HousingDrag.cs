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

        switch (data.housingType)
        {
            case HousingType.front:
                int layer_Front = LayerMask.NameToLayer("Front");
                gameObject.layer = layer_Front;
                break;
            case HousingType.back:
                int layer_Back = LayerMask.NameToLayer("Back");
                gameObject.layer = layer_Back;
                break;
            case HousingType.building:
                int layer_Building = LayerMask.NameToLayer("Building");
                gameObject.layer = layer_Building;
                break;
            case HousingType.constellation:
                int layer_Constellation = LayerMask.NameToLayer("Constellation");
                gameObject.layer = layer_Constellation;
                break;
            case HousingType.special:
                int layer_Special = LayerMask.NameToLayer("Special");
                gameObject.layer = layer_Special;
                break;
            default:
                break;
        }


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
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);        //todo... 종류에 따라 z값 다르게 설정하기
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
                    switch (data.housingType)
                    {
                        case HousingType.front:
                            transform.position = new Vector3(transform.position.x, transform.position.y, -0.6f);
                            break;
                        case HousingType.back:
                            transform.position = new Vector3(transform.position.x, transform.position.y, -0.4f);
                            break;
                        case HousingType.building:
                            transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);
                            break;
                        case HousingType.constellation:
                            transform.position = new Vector3(transform.position.x, transform.position.y, -0.3f);
                            break;
                        case HousingType.special:
                            transform.position = new Vector3(transform.position.x, transform.position.y, -0.7f);
                            break;
                        default:
                            break;
                    }

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
        int currentLayer = gameObject.layer;
        int defaultLayer = LayerMask.NameToLayer("Default");
        int exceptLayer = ~(1 << defaultLayer);

        Debug.Log("현재 레이어 : " + currentLayer);

        bool isHitRT = Physics.Raycast(ray_RT, out hit_RT, distance_RT, exceptLayer);
        bool isHitLT = Physics.Raycast(ray_LT, out hit_LT, distance_LT, exceptLayer);
        bool isHitLB = Physics.Raycast(ray_LB, out hit_LB, distance_LB, exceptLayer);
        bool isHitRB = Physics.Raycast(ray_RB, out hit_RB, distance_RB, exceptLayer);
        bool isHitCenter = Physics.Raycast(ray_Center, out hit_Center, distance_Center, exceptLayer);
        bool isHitBox = Physics.BoxCast(transform.position, new Vector3(spaceX * 0.495f, spaceY * 0.495f, 0.05f), Vector3.forward, out hit_Box, Quaternion.identity, distance_Box, exceptLayer);


        distance_RT = hit_RT.distance;
        distance_LT = hit_LT.distance;
        distance_LB = hit_LB.distance;
        distance_RB = hit_RB.distance;
        distance_Center = hit_Center.distance;
        distance_Box = hit_Box.distance;

        #region Draw Ray
        Color color = new Color();
        switch (currentLayer)
        {
            case 23:                //back
                color = Color.blue;
                break;
            case 24:                //front
                color = Color.red;
                break;
            case 25:                //building
                color = Color.white;
                break;
            case 26:                //constrellation
                color = Color.green;
                break;
            case 27:
                color = Color.cyan;
                break;
            default:
                break;
        }
        Debug.DrawRay(point_RT, Vector3.forward * distance_RT, color);
        Debug.DrawRay(point_LT, Vector3.forward * distance_LT, color);
        Debug.DrawRay(point_LB, Vector3.forward * distance_LB, color);
        Debug.DrawRay(point_RB, Vector3.forward * distance_RB, color);
        Debug.DrawRay(transform.position, Vector3.forward * distance_Center, color);

        //Debug.Log("RT : " + distance_RT);
        //Debug.Log("LT : " + distance_LT);
        //Debug.Log("LB : " + distance_LB);
        //Debug.Log("RB : " + distance_RB);
        #endregion

        if ((hit_RT.collider.CompareTag("CanBuild") &&
             hit_LT.collider.CompareTag("CanBuild") &&
             hit_LB.collider.CompareTag("CanBuild") &&
             hit_RB.collider.CompareTag("CanBuild") &&
             hit_Center.collider.CompareTag("CanBuild") &&
            !hit_Box.collider.gameObject.CompareTag("Building")) ||
            ((hit_RT.collider.gameObject.layer & ~(1 << currentLayer)) != 0 &&
            (hit_LT.collider.gameObject.layer & ~(1 << currentLayer)) != 0 &&
            (hit_LB.collider.gameObject.layer & ~(1 << currentLayer)) != 0 &&
            (hit_RB.collider.gameObject.layer & ~(1 << currentLayer)) != 0 &&
            (hit_Center.collider.gameObject.layer & ~(1 << currentLayer)) != 0 &&
            (hit_Box.collider.gameObject.layer & ~(1 << currentLayer)) != 0))    //올바른 위치에 하우징이 되었을 때
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
