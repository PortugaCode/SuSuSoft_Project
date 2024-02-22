using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HorizontalPlayer : MonoBehaviour
{
    public EventHandler OnLaver;



    private Vector3 touchPosition;
    private Vector3 direction;

    [Header("PlyerSpeed")]
    [SerializeField] float currentSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float minSpeed;

    public float initialSpeed;

    public int speedDuration;
    public float currentAcceleration;
    public float baseAcceleration;

    private float speed;
    private bool isSpeed = false;

    [Header("Player Location")]
    [SerializeField] private float rotationSpeed;

    [Header("  ")]
    [SerializeField] Horizon_Joystick horizon_Joystick;
    [SerializeField] Rigidbody2D rb2D;
    private bool gameStart = false;

    //Player Light
    [SerializeField] private Transform player;
    [SerializeField] private Light2D playerLight;
    private float maxSightRange = 2.72f;      //lightRangeOuter
    private float minSightRange = 1.3f;      //lightRangeBaseOuter
    private float sightRangeSpeed = 0.5f;

    //+
    private int ActiveSkill;
    private int PassiveSkill;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    private void Awake()
    {
        currentAcceleration = baseAcceleration;
    }

    private void Start()
    {
        //Invoke("StartGame", 3f);
        playerLight.pointLightOuterRadius = maxSightRange;
    }
    private void Update()
    {
        StartGame();
    }

    public void FixedUpdate()
    {
        if (gameStart)
        {
            PlayerUp();
            PlayerLight();
        }
    }

    private void StartGame()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                StartCoroutine(StartCorutine_Co());
            }
        }
    }

    private void PlayerUp()         //플레이어 위로 이동
    {
        if (Input.touchCount > 0)
        {
            animator.SetBool("Move", true);

            Touch touch = Input.GetTouch(0);

            touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;
            touchPosition.y = transform.position.y - 1;
            direction = (touchPosition - transform.position).normalized;

            // 터치 했을 때 플레이어 속력 증가
            currentSpeed += currentAcceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, initialSpeed, maxSpeed);
            rb2D.velocity = new Vector2(direction.x * currentSpeed, 0) * Time.deltaTime + Vector2.up * currentSpeed * Time.deltaTime;

            //Player Rotation
            PlayerRotation();

/*            if (touch.phase == TouchPhase.Ended && isSpeed == false)     //터치가 끝난 상태
            {

            }*/
        }
        else
        {
            animator.SetBool("Move", false);
            // 속도 초기화
            currentSpeed = initialSpeed;
            rb2D.velocity = Vector2.zero;
            rb2D.velocity = Vector2.up * currentSpeed * Time.deltaTime;

            //터치 끝났을 때 일정속도 유지
            rb2D.velocity = new Vector2(direction.x, 0) * currentSpeed * Time.deltaTime + Vector2.up * currentSpeed * Time.deltaTime;
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
        else if(collision.CompareTag("Lever"))
        {
            OnLaver?.Invoke(this, EventArgs.Empty);
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
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            //playerLight.pointLightOuterRadius = maxSightRange;
            StartCoroutine(MaxSight_Co());
        }
        else
        {
            StartCoroutine(MinSight_Co());
        }
    }

    IEnumerator MaxSight_Co()
    {
        while (playerLight.pointLightOuterRadius < maxSightRange)
        {
            playerLight.pointLightOuterRadius += sightRangeSpeed * Time.deltaTime;

            yield return null;
        }
    }

    IEnumerator MinSight_Co()
    {
        while (playerLight.pointLightOuterRadius > minSightRange)
        {
            playerLight.pointLightOuterRadius -= sightRangeSpeed * Time.deltaTime;

            yield return null;
        }
    }


    IEnumerator Speed_Co()
    {
        yield return new WaitForSeconds(speedDuration);

        isSpeed = false;
        maxSpeed = maxSpeed / 2f;
        currentAcceleration = baseAcceleration;
    }

    private IEnumerator StartCorutine_Co()
    {
        float touchStart = Time.time;

        while(Time.time - touchStart <= 1.5f)    //터치가 3초 이상 지속되지 않았을 때 반복
        {
            if (Input.touchCount == 0) yield break;

            yield return null;
        }
        gameStart = true;
    }
}