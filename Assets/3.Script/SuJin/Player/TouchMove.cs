using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Tcp;
using Protocol;
using System;
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

    [Header("PlyerSpeed")]
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    [HideInInspector] public Rigidbody2D rb2D;

    public bool isHost = false;


    [Header("Interaction data")]
    public bool canMove = true;
    public bool isInteraction = false;
    [SerializeField] private GameObject interactionObject;

    private void Start()
    {
        Invoke("StartGame", 0.1f);
        StartCoroutine(BlinkFace());
    }

    public void FixedUpdate()
    {
        if (!canMove) return;
        PlayerMove(direction);
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

    public void SetInteractionObject_true()
    {
        interactionObject.SetActive(true);
    }

    public void SetInteractionObject_false()
    {
        interactionObject.SetActive(false);
    }


    private void Update()
    {
        if (!canMove)
        {
            MoveRotation();
            return;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            interactionControl.doAnimatorArray[0].Invoke();
        }

        if (!isHost) return;
        SetTouchPosition();
    }

    public void SetCanMove_true()
    {
        canMove = true;
    }

    public void SetCanMove_false()
    {
        canMove = false;
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
                    }
                    else
                    {
                        isInteraction = false;
                    }
                }

                //========================================================================================================


                touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                touchPosition.z = 0;
                direction = (touchPosition - transform.position).normalized;


                Debug.Log(direction);
                if(Utils.Instance.nowScene == SceneNames.MatchRoom)
                {
                    PlayerMoveMessage msg = new PlayerMoveMessage(Backend.Match.GetMySessionId(), touchPosition, direction);
                    BackEndManager.Instance.GetMatchSystem().SendDataToInGame<PlayerMoveMessage>(msg);
                }
            }
        }
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }

    private void PlayerMove(Vector3 target)
    {
        if (Vector3.Distance(transform.position, touchPosition) > 0.6f)
        {
            //transform.position += direction * speed * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, touchPosition, speed * Time.deltaTime);
            //rb2D.velocity = target * speed * Time.fixedDeltaTime;

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

    IEnumerator BlinkFace()
    {
        while (true)
        {
            basicFace.SetActive(true);
            blinkFace.SetActive(false);
            yield return new WaitForSeconds(4f);

            basicFace.SetActive(false);
            blinkFace.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void StartGame()
    {
        //gameStart = true;
    }

    private void OnDestroy()
    {
        if(Utils.Instance.nowScene == SceneNames.MatchRoom)
        {
            GetComponent<MatchChat>().DestroyChatBox();
        }
    }
}
