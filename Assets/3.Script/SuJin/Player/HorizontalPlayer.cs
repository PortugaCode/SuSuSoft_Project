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
    private float speed;

    public float maxSpeed;
    public float minSpeed;

    public float baseAcceleration;
    public float currentAcceleration;

    public float initialSpeed;
    public  float currentSpeed;

    private bool isSpeed = false;
    public int speedDuration;

    //Player Location
    [SerializeField] private float rotationSpeed;

    [Header("  ")]
    public Horizon_Joystick horizon_Joystick;
    public Rigidbody2D rb2D;
    private bool gameStart = false;

    //Player Light
    [SerializeField] private Transform player;
    [SerializeField] private Light playerLight;
    private float lightRangeOuter;
    private float lightRangeInner;

    private void Awake()
    {
        currentAcceleration = baseAcceleration;
    }

    private void Start()
    {
        Invoke("StartGame", 3f);
        StartCoroutine(BlinkFace());

        playerLight = GetComponent<Light>();
    }

    public void FixedUpdate()
    {
        if (gameStart)
        {
            PlayerUp();
        }
    }

    private void PlayerUp()         //플레이어 위로 이동
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;
            touchPosition.y = transform.position.y - 1;
            direction = (touchPosition - transform.position).normalized;

            // 터치 했을 때 플레이어 속력 증가
            currentSpeed += currentAcceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, initialSpeed, maxSpeed);
            rb2D.velocity = new Vector2 (direction.x * currentSpeed, 0) * Time.deltaTime + Vector2.up * currentSpeed * Time.deltaTime;

            //Player Rotation
            PlayerRotation();

            if (touch.phase == TouchPhase.Ended && isSpeed == false)     //터치가 끝난 상태
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

  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SpeedItem"))     //Player Speed Item
        {
            IncreaseSpeed(); // 속도 증가
            Destroy(collision.gameObject);
            StartCoroutine(Speed_Co());
        }
    }

    public void IncreaseSpeed()     //Player Speed Method
    {
        isSpeed = true;
        maxSpeed = maxSpeed * 2f;
        currentAcceleration = baseAcceleration * 10f;
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
    }       //Player Location Method

    private void PlayerLight()
    {
        //터치 했을 때 Light Range Outer 2.72
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 playerlightOn = Camera.main.ScreenToViewportPoint(player.position);
            playerlightOn.z = 0;
            
          //  float lightRange = Light.

        }

        //터치 안했을 때 Light Range Inner 1

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

    IEnumerator Speed_Co()
    {
        yield return new WaitForSeconds(speedDuration);

        isSpeed = false;
        maxSpeed = maxSpeed / 2f;
        currentAcceleration = baseAcceleration;
    }

    private void StartGame()
    {
        gameStart = true;
    }
}