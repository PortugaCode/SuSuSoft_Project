using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalPlayer : MonoBehaviour
{
    [Header("PlyerFace")]
    [SerializeField] private GameObject basicFace;
    [SerializeField] private GameObject blinkFace;

    private Vector3 touchPosition;
    private Vector3 direction;

    [Header("PlyerSpeed")]
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float acceleration;
    [SerializeField] private float initialSpeed;
    [SerializeField] private float maxSpeed;
    private float currentSpeed;

    [Header("  ")]
    public Horizon_Joystick horizon_Joystick;
    public Rigidbody2D rigidbody2D;
    private bool gameStart = false;

    float dragLength;

    private void Start()
    {
        Invoke("StartGame", 3f);
        StartCoroutine(BlinkFace());
    }

    public void FixedUpdate()
    {
        if (gameStart)
        {
            PlayerUp();
        }
    }

    private void PlayerUp()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;
            touchPosition.y = transform.position.y - 1;
            direction = (touchPosition - transform.position).normalized;

            // 터치 했을 때 플레이어 속력 증가
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, initialSpeed, maxSpeed);
            rigidbody2D.velocity = new Vector2(direction.x * currentSpeed, 0) * Time.deltaTime + Vector2.up * currentSpeed * Time.deltaTime;

            //Player Rotation
            PlayerRotation();

            if (touch.phase == TouchPhase.Ended)     //터치가 끝난 상태
            {
                // 속도 초기화
                currentSpeed = initialSpeed;
                rigidbody2D.velocity = Vector2.zero;
                rigidbody2D.velocity = Vector2.up * currentSpeed * Time.deltaTime;

                //터치 끝났을 때 일정속도 유지
                rigidbody2D.velocity = new Vector2(direction.x, 0) * speed * Time.deltaTime + Vector2.up * 1000f * Time.deltaTime;
            }
        }
    }

    public void PlayerRotation()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            Vector3 touchPositionToRotate = Camera.main.ScreenToWorldPoint(touch.position);

            touchPositionToRotate.z = 0;
            touchPositionToRotate.y = transform.position.y + 3f;

            Vector3 directiontToRotate = (touchPositionToRotate - transform.position).normalized;
            float rotationZ = Mathf.Atan2(directiontToRotate.y, directiontToRotate.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotationZ + -90f);
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
        gameStart = true;
    }
}