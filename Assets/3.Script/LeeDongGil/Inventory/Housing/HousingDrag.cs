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
    public bool isTouch = false;
    public float touchTime = 0;

    [Header("Build Setting")]
    public HousingItemData data;  //중근아 여기 참조해
    public HousingObject housingObject;
    public SpriteRenderer buildSprite;
    public GameObject space;

    [HideInInspector] public float spaceX;
    [HideInInspector] public float spaceY;
    public int id;
    public int primaryIndex;

    private int checkMinusY = 1;
    private int checkMinusX = 1;
    public HousingGrid grid;

    private SpriteRenderer check;
    private BoxCollider boxCollider;
    private BoxCollider subCollider;

    [HideInInspector] public float moveX;
    [HideInInspector] public float moveY;
    [HideInInspector] public float clampX;
    [HideInInspector] public float clampY;
    private float currentClampX = 0;
    private float currentClampY = 0;
    private float cameraX = 0;
    private float cameraY = 0;
    private Camera mainCam;
    [SerializeField] private float cameraMoveStartPos = 1.6f;
    [SerializeField] private float camSpeed = 0.01f;

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
        mainCam = Camera.main;
        if (!LoadHousing.instance.isLoading)
        {
            Debug.Log("순서 2");
            previousParent = transform.parent;

            primaryIndex = LoadHousing.instance.primaryKey;             //test해보기 2
            LoadHousing.instance.localHousing.Add(primaryIndex, (housingObject, transform.position));

            LoadHousing.instance.primaryKey += 1;

            transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        }
        else
        {
            transform.position = LoadHousing.instance.localHousing[primaryIndex].Item2;
        }
        //LoadHousing.instance.isLoading = false;
        //spaceX = data.housingWidth;
        //spaceY = data.housingHeight;

        SetWidthHeight();

        SetLayer();



        #region Scriptable Object(연동 전)
        /*
                switch (data.housingType)       //수정 1
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
                    case HousingType.interactionable:
                        int layer_Interactionable = LayerMask.NameToLayer("Interactionable");
                        gameObject.layer = layer_Interactionable;
                        break;
                    default:
                        break;
                }
        */
        #endregion




        space.transform.localScale = new Vector3(spaceX, spaceY, 1);

        check = transform.GetChild(0).GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        subCollider = transform.GetChild(1).GetComponent<BoxCollider>();
        group = FindObjectOfType<EditModeButton>().GetComponent<CanvasGroup>();
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
            check.gameObject.SetActive(false);
            subCollider.enabled = false;
        }

        if (isTouch)
        {
            touchTime += Time.deltaTime;
            if (touchTime > 0.5f)
            {
                isDragging = true;
            }
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
                        isTouch = true;
                        Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
                        Debug.Log("Began 실행2");
                        offset = transform.position - ray.GetPoint(hit.distance);
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
                transform.position = new Vector3(clampX, clampY, transform.position.z);
                subCollider.enabled = false;
                isTouch = false;
                touchTime = 0;
                int currentLayer = gameObject.layer;
                if (isCanBuild)
                {
                    switch (currentLayer)
                    {
                        case 24:
                            transform.position = new Vector3(transform.position.x, transform.position.y, -0.6f);
                            break;
                        case 23:
                            transform.position = new Vector3(transform.position.x, transform.position.y, -0.4f);
                            break;
                        case 25:
                            transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);
                            break;
                        case 22:
                            transform.position = new Vector3(transform.position.x, transform.position.y, -0.8f);
                            break;
                        default:
                            break;
                    }

                }
                else
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, -0.9f);
                }


            }

            if (isDragging)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -1);
                transform.SetParent(previousParent);
                check.gameObject.SetActive(true);
                subCollider.enabled = true;
                Vector3 newPosition = ray.GetPoint(offset.z);

                

                checkMinusX = newPosition.x >= 0 ? 1 : -1;
                checkMinusY = newPosition.y >= 0 ? 1 : -1;

                moveX = spaceX % 2 == 0 ? Mathf.RoundToInt(Mathf.Abs(newPosition.x)) * checkMinusX : Mathf.FloorToInt(newPosition.x) + 0.5f;
                moveY = spaceY % 2 == 0 ? Mathf.RoundToInt(Mathf.Abs(newPosition.y)) * checkMinusY : Mathf.FloorToInt(newPosition.y) + 0.5f;

                clampX = Mathf.Clamp(moveX, -(grid.boundX / 2) + (spaceX / 2) + grid.posX, (grid.boundX / 2) - (spaceX / 2) + grid.posX);
                clampY = Mathf.Clamp(moveY, -(grid.boundY / 2) + (spaceY / 2) + grid.posY, (grid.boundY / 2) - (spaceY / 2) + grid.posY);

                //Debug.Log(-(grid.boundX / 2) + spaceX / 2);
                //Debug.Log(-(grid.boundY / 2) + spaceY / 2);
                currentClampX = Mathf.Clamp(newPosition.x, -(grid.boundX / 2) + (spaceX / 2) + grid.posX, (grid.boundX / 2) - (spaceX / 2) + grid.posX);
                currentClampY = Mathf.Clamp(newPosition.y, -(grid.boundY / 2) + (spaceY / 2) + grid.posY, (grid.boundY / 2) - (spaceY / 2) + grid.posY);

                #region 가장자리 이동시 카메라 이동

                Vector3 mainCamPos = mainCam.transform.position;

                //카메라 각 꼭지점 좌표
                Vector3 cam_RT = mainCam.ViewportToWorldPoint(new Vector3(1, 1, mainCam.nearClipPlane));
                Vector3 cam_LT = mainCam.ViewportToWorldPoint(new Vector3(0, 1, mainCam.nearClipPlane));
                Vector3 cam_LB = mainCam.ViewportToWorldPoint(new Vector3(0, 0, mainCam.nearClipPlane));
                Vector3 cam_RB = mainCam.ViewportToWorldPoint(new Vector3(1, 0, mainCam.nearClipPlane));

                cameraX = Mathf.Clamp(mainCam.transform.position.x, -(grid.boundX / 2) + ((cam_RT.x - cam_LT.x) / 2) + grid.posX, (grid.boundX / 2) - ((cam_RT.x - cam_LT.x) / 2) + grid.posX);
                cameraY = Mathf.Clamp(mainCam.transform.position.y, -(grid.boundY / 2) + ((cam_RT.y - cam_RB.y) / 2) + grid.posY, (grid.boundY / 2) - ((cam_RT.y - cam_RB.y) / 2) + grid.posY);



                if (newPosition.x > cam_RT.x - cameraMoveStartPos)
                {
                    if (newPosition.y > cam_LT.y - cameraMoveStartPos)
                    {
                        Debug.Log("오른쪽 대각선 위쪽 이동");
                        mainCam.transform.position = new Vector3(cameraX + camSpeed, cameraY + camSpeed, mainCamPos.z);
                    }
                    else if (newPosition.y < cam_RB.y + cameraMoveStartPos)
                    {
                        Debug.Log("오른쪽 대각선 아래쪽 이동");
                        mainCam.transform.position = new Vector3(cameraX + camSpeed, cameraY - camSpeed, mainCamPos.z);
                    }
                    else
                    {
                        Debug.Log("오른쪽 이동");
                        mainCam.transform.position = new Vector3(cameraX + camSpeed, cameraY, mainCamPos.z);
                    }
                }

                if (newPosition.x < cam_LB.x + cameraMoveStartPos)
                {
                    if (newPosition.y > cam_LT.y - cameraMoveStartPos)
                    {
                        Debug.Log("왼쪽 대각선 위쪽 이동");
                        mainCam.transform.position = new Vector3(cameraX - camSpeed, cameraY + camSpeed, mainCamPos.z);
                    }
                    else if (newPosition.y < cam_RB.y + cameraMoveStartPos)
                    {
                        Debug.Log("왼쪽 대각선 아래쪽 이동");
                        mainCam.transform.position = new Vector3(cameraX - camSpeed, cameraY - camSpeed, mainCamPos.z);
                    }
                    else
                    {
                        Debug.Log("왼쪽 이동");
                        mainCam.transform.position = new Vector3(cameraX - camSpeed, cameraY, mainCamPos.z);
                    }
                }

                if (newPosition.y > cam_LT.y - cameraMoveStartPos)
                {
                    if (newPosition.x > cam_RT.x - cameraMoveStartPos)
                    {
                        Debug.Log("위쪽 대각선 오른쪽 이동");
                        mainCam.transform.position = new Vector3(cameraX + camSpeed, cameraY + camSpeed, mainCamPos.z);
                    }
                    else if (newPosition.x < cam_LB.x + cameraMoveStartPos)
                    {
                        Debug.Log("위쪽 대각선 왼쪽 이동");
                        mainCam.transform.position = new Vector3(cameraX - camSpeed, cameraY + camSpeed, mainCamPos.z);
                    }
                    else
                    {
                        Debug.Log("위쪽 이동");
                        mainCam.transform.position = new Vector3(cameraX, cameraY + camSpeed, mainCamPos.z);
                    }
                }

                if (newPosition.y < cam_RB.y + cameraMoveStartPos)
                {
                    if (newPosition.x > cam_RT.x - cameraMoveStartPos)
                    {
                        Debug.Log("아래쪽 대각선 오른쪽 이동");
                        mainCam.transform.position = new Vector3(cameraX + camSpeed, cameraY - camSpeed, mainCamPos.z);
                    }
                    else if (newPosition.x < cam_LB.x + cameraMoveStartPos)
                    {
                        Debug.Log("아래쪽 대각선 왼쪽 이동");
                        mainCam.transform.position = new Vector3(cameraX - camSpeed, cameraY - camSpeed, mainCamPos.z);
                    }
                    else
                    {
                        Debug.Log("아래쪽 이동");
                        mainCam.transform.position = new Vector3(cameraX, cameraY - camSpeed, mainCamPos.z);
                    }
                }

                #endregion

                transform.position = new Vector3(currentClampX, currentClampY, transform.position.z);

                group.alpha = 0;
            }
            else
            {
                group.alpha = 1.0f;

                LoadHousing.instance.localHousing[primaryIndex] = (housingObject, transform.position);      //test해보기 3
                //LoadHousing.instance.saveLocal[primaryIndex] = (gameObject, transform.position);
                Debug.Log($"현재 오브젝트 : {LoadHousing.instance.localHousing[primaryIndex].Item1.name_k}");
                Debug.Log($"현재 포지션 : {LoadHousing.instance.localHousing[primaryIndex].Item2}");
            }
        }
        else
        {
            //Debug.Log("else End");
            isDragging = false;
            subCollider.enabled = false;
        }
    }

    private void CheckToBuild()
    {
        #region Housing Object Ray
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
        #endregion


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

        if (hit_RT.collider.gameObject.layer != currentLayer &&
             hit_LT.collider.gameObject.layer != currentLayer &&
             hit_LB.collider.gameObject.layer != currentLayer &&
             hit_RB.collider.gameObject.layer != currentLayer &&
             hit_Center.collider.gameObject.layer != currentLayer &&
             hit_Box.collider.gameObject.layer != currentLayer)
        {
            if (currentLayer == LayerMask.NameToLayer("Building") && transform.position.y > grid.boundRB.y + (spaceY / 2) + 0.5f)   //test해보기 1
            {
                check.color = new Color32(255, 0, 0, 100);
                check.gameObject.SetActive(true);
                transform.SetParent(previousParent);
                isCanBuild = false;
            }
            else
            {
                check.color = new Color32(0, 255, 0, 100);
                isCanBuild = true;
                if (!isDragging)
                {
                    transform.SetParent(hit_Center.collider.transform.parent);
                }
            }
        }
        else
        {
            check.color = new Color32(255, 0, 0, 100);
            check.gameObject.SetActive(true);
            //transform.SetParent(previousParent);
            isCanBuild = false;
        }

    }

    private void SetWidthHeight()
    {
        switch (housingObject.index)
        {
            case 0:
                spaceX = 4;
                spaceY = 5;
                break;
            case 1:
                spaceX = 3;
                spaceY = 5;
                break;
            case 2:
                spaceX = 4;
                spaceY = 3;
                break;
            case 3:
                spaceX = 4;
                spaceY = 3;
                break;
            case 4:
                spaceX = 3;
                spaceY = 5;
                break;
            case 5:
                spaceX = 1;
                spaceY = 1;
                break;
            case 6:
                spaceX = 1;
                spaceY = 1;
                break;
            case 7:
                spaceX = 1;
                spaceY = 1;
                break;
        }
    }

    private void SetLayer()
    {
        switch (housingObject.type)
        {
            case "전경":
                int layer_Front = LayerMask.NameToLayer("Front");
                gameObject.layer = layer_Front;
                break;
            case "후경":
                int layer_Back = LayerMask.NameToLayer("Back");
                gameObject.layer = layer_Back;
                break;
            case "건물":
                int layer_Building = LayerMask.NameToLayer("Building");
                gameObject.layer = layer_Building;
                break;
            case "상호작용":
                int layer_Interactionable = LayerMask.NameToLayer("Interactionable");
                gameObject.layer = layer_Interactionable;
                break;
            default:
                break;
        }
    }

    private void OnDestroy()
    {
        //LoadHousing.instance.isLoading = false;
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
