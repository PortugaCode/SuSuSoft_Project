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
    public float maxSpeed;
    public float minSpeed;
    public float SpeedItemAmount = 20;

    public float acceleration;
    public float initialSpeed;
    private float currentSpeed;
    [SerializeField] private float rotationSpeed;

    [Header("  ")]
    public Horizon_Joystick horizon_Joystick;
    public Rigidbody2D rb2D;
    private bool gameStart = false;

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
            rb2D.velocity = new Vector2(direction.x * currentSpeed, 0) * Time.deltaTime + Vector2.up * currentSpeed * Time.deltaTime;

            //Player Rotation
            PlayerRotation();

            if (touch.phase == TouchPhase.Ended)     //터치가 끝난 상태
            {
                // 속도 초기화
                currentSpeed = initialSpeed;
                rb2D.velocity = Vector2.zero;
                rb2D.velocity = Vector2.up * currentSpeed * Time.deltaTime;

                //터치 끝났을 때 일정속도 유지
                rb2D.velocity = new Vector2(direction.x, 0) * speed * Time.deltaTime + Vector2.up * speed * Time.deltaTime;
            }
        }
    }

    //Player Speed Item
/*    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SpeedItem"))
        {
            IncreaseSpeed(SpeedItemAmount); // 아이템에서 받아온 속도 증가량을 전달
            Destroy(collision.gameObject);
        }
    }

    public void IncreaseSpeed(float speedIncrease)
    {
        currentSpeed += speedIncrease * acceleration + 30f * Time.deltaTime; // 속도 증가량을 매개변수로 전달받아 현재 속도에 추가
        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);
        transform.Translate(Vector2.up * currentSpeed * Time.deltaTime);

        Debug.Log($"{currentSpeed}:    ");
        // rb2D.velocity = new Vector2(direction.x * currentSpeed, 0) * Time.deltaTime + Vector2.up * currentSpeed * Time.deltaTime;
    }
*/

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