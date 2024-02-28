using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Tcp;
using Protocol;
using System;
using UnityEngine.EventSystems;

public class TestTouchMove : MonoBehaviour
{
    [Header("PlyerFace")]
    [SerializeField] private GameObject basicFace;
    [SerializeField] private GameObject blinkFace;

    [Header("Animator")]
    public InteractionControl interactionControl;


    private Vector3 touchPosition;
    public Vector3 TouchPosition => touchPosition;
    private Vector3 direction = Vector3.zero;
    public Vector3 Direction => direction;
    Vector3 deceleration;

    [Header("PlyerSpeed")]
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    public Rigidbody2D rb2D;

    public bool isHost = false;
    public float distance = 0;

    [Header("Interaction data")]
    public bool canMove = true;
    public bool isInteraction = false;
    public GameObject interactionObject;

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

    /*private void OnTriggerEnter2D(Collider2D collision)
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
    }*/

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

        if (interactionObject != null)
        {
            InteractionPlayer();
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
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && EventSystem.current.IsPointerOverGameObject(0) == false)
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
                    }
                }

                //========================================================================================================


                touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                touchPosition.z = 0;
                direction = (touchPosition - transform.position).normalized;

                Debug.Log(direction);
                if (Utils.Instance.nowScene == SceneNames.MatchRoom)
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
        if (Vector3.Distance(transform.position, touchPosition) > 0.2f)
        {
            rb2D.velocity = target * speed * Time.fixedDeltaTime;

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
        if (Utils.Instance.nowScene == SceneNames.MatchRoom)
        {
            GetComponent<MatchChat>().DestroyChatBox();
        }
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
            if (housingDrag.data.housingType == HousingType.interactionable)
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
            }
        }
    }
}
