using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMove : MonoBehaviour
{
    [Header("PlyerFace")]
    [SerializeField] private GameObject basicFace;
    [SerializeField] private GameObject blinkFace;

    private Vector3 touchPosition;
    public Vector3 TouchPosition => touchPosition;
    private Vector3 direction;
    Vector3 deceleration;

    [Header("PlyerSpeed")]
    [SerializeField] private float speed;

    //private float smoothing = 50f;
    [SerializeField] private float rotationSpeed;

    [Header("  ")]
    public Rigidbody2D rb2D;
    //private bool gameStart = false; //지금 안써서 지움

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
        SetTouchPosition();
        MoveRotation();
    }

    private void SetTouchPosition()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                touchPosition.z = 0;
                direction = (touchPosition - transform.position).normalized;
            }
        }
    }

    private void PlayerMove(Vector3 target)
    {
        if (target.magnitude > 0)
        {
            rb2D.velocity = target * speed * Time.fixedDeltaTime;

            if (Vector3.Distance(transform.position, touchPosition) < 0.1f)
            {
                rb2D.velocity = Vector3.zero;
                target = Vector3.zero; // 움직임을 중지하고 방향을 초기화
            }
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
}
