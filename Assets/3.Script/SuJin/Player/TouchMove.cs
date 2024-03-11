using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Tcp;
using Protocol;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TouchMove : MonoBehaviour
{
    [Header("PlyerFace")]
    [SerializeField] private GameObject basicFace;
    [SerializeField] private GameObject blinkFace;

    [Header("Animator")]
    [SerializeField] private InteractionControl interactionControl;

    private Vector3 touchPosition;
    public Vector3 TouchPosition => touchPosition;
    private Vector3 direction = Vector3.zero;
    public Vector3 Direction => direction;
    Vector3 deceleration;
    private bool isRight;
    
    [Header("PlyerSpeed")]
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    [HideInInspector] public Rigidbody2D rb2D;

    public bool isHost = false;


    [Header("Interaction data")]
    public bool canMove = true;
    public bool isInteraction = false;
    [SerializeField] private GameObject interactionObject;


    [Header("FX Manager")]
    [SerializeField] private EffectManager effectManager;

    [Header("Camera Control")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] Transform player;
    [SerializeField] float smothing = 2f;
    [SerializeField] Vector2 minCameraPos;
    [SerializeField] Vector2 maxCameraPos;

    [SerializeField] private Animator animator;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("EffectManager").TryGetComponent<EffectManager>(out effectManager);
        mainCamera = Camera.main;
        StartBGM();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Building") && isInteraction)
        {
            HousingDrag housingDrag = collision.GetComponent<HousingDrag>();
            Debug.Log(housingDrag.gameObject.name);
            if (housingDrag.data.housingType == HousingType.interactionable)
            {
                Debug.Log("housingType 들어옴");
                switch (housingDrag.data.housingID)
                {
                    case 5001:
                        Debug.Log("5001 들어옴");
                        interactionObject = collision.gameObject;
                        interactionControl.doAnimatorArray[0].Invoke();
                        break;
                    default:
                        Debug.Log("Unknown housingID type");
                        return;
                }
            }
        }
    }


    private void Update()
    {

        if (!canMove)
        {
            MoveRotation();
            return;
        }


        PlayerMove(direction);



        if (!isHost) return;
        SetTouchPosition();


        if (interactionObject != null)
        {
            InteractionPlayer();
        }
    }

    private void LateUpdate()
    {
        if (SceneManager.GetActiveScene().name != "CharacterTest" || SceneManager.GetActiveScene().name != "OnGame")
        {
            CameraPos();
        }
    }

    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name == "CharacterTest" || SceneManager.GetActiveScene().name == "OnGame")
        {
            CameraPos_Stage();
        }
    }

    private void ShakeCamera(object sender, EventArgs e)
    {
        animator.SetTrigger("Shake");
    }

    public void SetPlayer(GameObject a)
    {
        player = a.transform;
    }

    private void StartBGM()
    {
        //스테이지 씬이라면 조건 추가 해야함
        if (Utils.Instance.nowScene == SceneNames.OnGame)
        {
            player.gameObject.GetComponent<PlayerProperty>().onDamage = ShakeCamera;
            AudioManager.Instance.PlayBGM(BGM_Name.Stage);
        }
        else
        {
            AudioManager.Instance.PlayBGM(BGM_Name.Main);
        }
    }



    private void CameraPos_Stage()
    {
        if (player == null) return;
        Vector3 targetPos = new Vector3(player.position.x, player.position.y, mainCamera.transform.position.z);
        targetPos.y = Mathf.Clamp(targetPos.y, minCameraPos.y, maxCameraPos.y);
        //targetPos.x = Mathf.Clamp(targetPos.x, minCameraPos.x, maxCameraPos.x);

        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, smothing * Time.deltaTime);
    }

    private void CameraPos()
    {
        if (player == null) return;
        Vector3 targetPos = new Vector3(player.position.x, player.position.y, mainCamera.transform.position.z);
        targetPos.y = Mathf.Clamp(targetPos.y, minCameraPos.y, maxCameraPos.y);
        mainCamera.transform.position = targetPos;
    }




    public void SetInteractionObject_true()
    {
        interactionObject.SetActive(true);
    }

    public void SetInteractionObject_false()
    {
        interactionObject.SetActive(false);
    }

    public void SetCanMove_true()
    {
        canMove = true;
    }

    public void SetCanMove_false()
    {
        canMove = false;
    }

    public void SetShowUI()
    {
        Debug.Log("Show UI");
        TestManager.instance.isShowUI = true;
        TestManager.instance.isShowUI_win = true;
    }

    public void SetHideUI()
    {
        Debug.Log("Hide UI");
        TestManager.instance.isShowUI = false;
        TestManager.instance.isShowUI_win = false;
    }

    private void SetTouchPosition()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began && EventSystem.current.IsPointerOverGameObject(0) == false)
            {

                Ray touchRay = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(touchRay, out hit))
                {
                    if (hit.collider.gameObject.CompareTag("Building"))
                    {
                        isInteraction = true;
                        interactionObject = hit.collider.gameObject;
                    }
                    else
                    {
                        isInteraction = false;
                        interactionObject = null;
                    }
                }

                //========================================================================================================

                

                touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                touchPosition.z = 0;
                direction = (touchPosition - transform.position).normalized;

                effectManager.PlayTouchFX(touchPosition);

                SetIsRight();

                Debug.Log(direction);
                if(Utils.Instance.nowScene == SceneNames.MatchRoom)
                {
                    PlayerMoveMessage msg = new PlayerMoveMessage(Backend.Match.GetMySessionId(), touchPosition, direction);
                    BackEndManager.Instance.GetMatchSystem().SendDataToInGame<PlayerMoveMessage>(msg);
                }
            }
        }
    }

    public void SetIsRight()
    {
        if (touchPosition.x < transform.position.x)
        {
            isRight = false;
        }
        else if (touchPosition.x > transform.position.x)
        {
            isRight = true;
        }
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }


    private void PlayerMove(Vector3 target)
    {
        #region [Repeat BG]
        if (transform.position.x >= 14.97f && isRight)
        {
            transform.position = new Vector2(transform.position.x * -1f, transform.position.y);

            float a = touchPosition.x - 14.97f;
            float b = -15.2f + Mathf.Abs(a);
            float c = -14.97f + Mathf.Abs(a);

            touchPosition = new Vector2(b, touchPosition.y);
            effectManager.GetTouchFX().gameObject.transform.position = new Vector3(c, touchPosition.y);
            return;
        }
        else if (transform.position.x <= -14.97f && !isRight)
        {
            transform.position = new Vector2(transform.position.x * -1f, transform.position.y);

            float a = touchPosition.x + 14.97f;
            float b = 15.2f - Mathf.Abs(a);
            float c = 14.97f - Mathf.Abs(a);

            touchPosition = new Vector2(b, touchPosition.y);
            effectManager.GetTouchFX().gameObject.transform.position = new Vector3(c, touchPosition.y);
            return;
        }
        #endregion

        if (Vector3.Distance(transform.position, touchPosition) > 0.3f)
        {
            transform.position = Vector3.Lerp(transform.position, touchPosition, speed * Time.deltaTime);

            if (direction != Vector3.zero)
            {
                // 플레이어가 바라보는 각도 계산
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // 플레이어를 각도에 따라 회전
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle - 90f)), rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            rb2D.velocity = Vector3.zero;
        }
    }


    private void MoveRotation()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 0)), 10 * Time.deltaTime);
    }

    internal void SetPosition(Vector3 movePosition)
    {
        touchPosition = movePosition;
    }


    private void InteractionPlayer()
    {
        float width = interactionObject.GetComponent<HousingDrag>().spaceX;
        float height = interactionObject.GetComponent<HousingDrag>().spaceY;
        Vector3 targetPosition = interactionObject.transform.position;
        if (Vector2.SqrMagnitude(targetPosition - transform.position) < Mathf.Pow(width / 2, 2) + Mathf.Pow(height / 2, 2) && isInteraction)
        {
            HousingDrag housingDrag = interactionObject.GetComponent<HousingDrag>();
            Debug.Log(housingDrag.gameObject.name);

            if(housingDrag.housingObject.type == "상호작용")
            {
                Debug.Log("housingType 들어옴");
                switch (housingDrag.housingObject.index)
                {
                    case 7:
                        Debug.Log("농구공 : 7 들어옴");
                        interactionControl.doAnimatorArray[0].Invoke();
                        touchPosition = transform.position;
                        isInteraction = false;
                        break;
                    default:
                        Debug.Log("Unknown housingID type");
                        return;
                }
            }
            
            #region [type 비교]
            /*            if (housingDrag.data.housingType == HousingType.interactionable)
                        {
                            Debug.Log("housingType 들어옴");
                            switch (housingDrag.data.housingID)
                            {
                                case 5001:
                                    Debug.Log("5001 들어옴");
                                    interactionControl.doAnimatorArray[0].Invoke();
                                    isInteraction = false;
                                    break;
                                default:
                                    Debug.Log("Unknown housingID type");
                                    return;
                            }
                        }*/
            #endregion
        }
    }


    private void OnDestroy()
    {
        if(Utils.Instance.nowScene == SceneNames.MatchRoom)
        {
            GetComponent<MatchChat>().DestroyChatBox();
        }
    }
}
