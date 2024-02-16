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

    private Vector3 touchPosition;
    public Vector3 TouchPosition => touchPosition;
    private Vector3 direction = Vector3.zero;
    public Vector3 Direction => direction;
    Vector3 deceleration;

    [Header("PlyerSpeed")]
    [SerializeField] private float speed;

    //private float smoothing = 50f;
    [SerializeField] private float rotationSpeed;

    [Header("  ")]
    public Rigidbody2D rb2D;
    //private bool gameStart = false; //지금 안써서 지움

    public bool isHost = false;

    private void Start()
    {
        Invoke("StartGame", 0.1f);
        StartCoroutine(BlinkFace());
    }

    public void FixedUpdate()
    {
        PlayerMove(direction);
    }

    private void Update()
    {
        //MoveRotation();

        if (!isHost) return;
        SetTouchPosition();

    }

    private void SetTouchPosition()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began && EventSystem.current.IsPointerOverGameObject(0) == false)
            {
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
        if (Vector3.Distance(transform.position, touchPosition) > 0.2f)
        {
            rb2D.velocity = target * speed * Time.fixedDeltaTime;

            if(direction != Vector3.zero)
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
        if (Input.touchCount > 0)
        {
            if (direction.magnitude > 0)
            {
                // 플레이어가 바라보는 각도 계산
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // 플레이어를 각도에 따라 회전
                //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle - 90f)), rotationSpeed * Time.deltaTime);
            }
        }
     
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
