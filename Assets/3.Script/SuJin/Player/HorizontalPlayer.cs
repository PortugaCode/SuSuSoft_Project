using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using TMPro;
using System.Collections.Generic;

public class HorizontalPlayer : MonoBehaviour
{
    public EventHandler OnLaver;

    [SerializeField] private GameManager gameManager;
    public GameManager GameControl => gameManager;
    [SerializeField] private PlayerProperty playerProperty;
    [SerializeField] private TextMeshProUGUI starCountTmpPro;

    //PlayerMove
    private Vector3 touchPosition;
    private Vector3 direction;
    public Vector3 playerPosition;

    public bool isPlayerMove = false;

    public Canvas canvas;
    GraphicRaycaster graphicRay;
    public Coroutine coroutine;


    [Header("PlyerSpeed")]
    [SerializeField] float currentSpeed;
    public float maxSpeed;
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

    [Header("Player Light")]
    [SerializeField] private Transform player;
    [SerializeField] private Light2D playerLight;
    [SerializeField] private float maxSightRange = 2.72f;      //lightRangeOuter
    [SerializeField] private float minSightRange = 1.3f;      //lightRangeBaseOuter
    private float sightRangeSpeed = 0.5f;

    //+
    //private int ActiveSkill;
    //private int PassiveSkill;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    private void Awake()
    {
        currentAcceleration = baseAcceleration;
    }

    private void Start()
    {
        playerProperty = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerProperty>();
        playerLight.pointLightOuterRadius = maxSightRange;
        graphicRay = canvas.GetComponent<GraphicRaycaster>();
    }

    private void Update()
    {
        StartGame();
        starCountTmpPro.text = $"{playerProperty.stars.Count} / {playerProperty.maxStar}";
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
        if (gameStart) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                
                StartCoroutine(StartCorutine_Co());
            }
        }
    }

    public void PlayerUp()         //플레이어 위로 이동
    {
        if (Input.touchCount > 0)
        {
            animator.SetBool("Move", true);

            Touch touch = Input.GetTouch(0);

            //Player Skill Active Button
            CheckUI(touch);

            if(isPlayerMove)
            {
                touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                touchPosition.z = 0;
                touchPosition.y = transform.position.y - 1;
                direction = (touchPosition - transform.position).normalized;

                // 터치 했을 때 플레이어 속력 증가
                currentSpeed += currentAcceleration * Time.deltaTime;
                currentSpeed = Mathf.Clamp(currentSpeed, initialSpeed, maxSpeed);
                rb2D.velocity = new Vector2(direction.x * currentSpeed, 0) * Time.deltaTime + Vector2.up * currentSpeed * Time.deltaTime;

                playerPosition = transform.position;

                //Player Rotation
                PlayerRotation();
            }
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

    public Rigidbody2D GetRigidbody2D()
    {
        return rb2D;
    }


    //Skill Active UI 판단
    //BG Image Alpha
    //Item Image fill Amount
    private void CheckUI(Touch touch)
    {
        var pointerevent = new PointerEventData(null);
        pointerevent.position = touch.position;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        graphicRay.Raycast(pointerevent, raycastResults);

        if (raycastResults.Count > 0)
        {
            GameObject obj = raycastResults[0].gameObject;
            if (obj.CompareTag("UI"))
            {
                isPlayerMove = false;
                if(playerProperty.skillActive.isItemOn)
                {
                    playerProperty.SkillActive();

                    //CoolTime 초기화
                    playerProperty.skillActive.shieldFillImage.fillAmount = 1.0f;
                }
            }
            else
            {
                isPlayerMove = true;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SpeedItem"))     //Player Speed Item
        {
            IncreaseSpeed(); // 속도 증가
            Destroy(collision.gameObject);
            //StartCoroutine(Speed_Co());
        }
        else if(collision.CompareTag("Lever"))
        {
            OnLaver?.Invoke(this, EventArgs.Empty);
        }
    }

    private void IncreaseSpeed()     //Player Speed Method
    {
        if (isSpeed)
        {
            maxSpeed = maxSpeed * 2f;
            currentAcceleration = baseAcceleration * 10f;
            StartCoroutine(Speed_Co());  // 안되면 주석처리에 넣기
        }
    }

    public void ActiveSpeedUp()
    {
        playerProperty.skillActive.isItemOn = false;
        playerProperty.isCanSkill = true;

        if(playerProperty.isCanSkill)
        {
            maxSpeed = maxSpeed * 2f;
            currentAcceleration = baseAcceleration * 10f;
            StartCoroutine(playerProperty.SpeedUpSkillDuration_Co());
        }
    }

    public void PlayerRotation()
    {
        if (Input.touchCount > 0)
        {
            isPlayerMove = true;
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

    private void OnDestroy()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    #region [IEnumerator]
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
        if(gameStart)
        {
            this.coroutine = playerProperty.skillActive.StartCoroutine(playerProperty.skillActive.CoolTime_Co());
        }
    }
    #endregion
}