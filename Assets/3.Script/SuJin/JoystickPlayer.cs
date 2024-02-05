using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlayer : MonoBehaviour
{
    private Vector3 touchPosition;
    private Vector3 direction;

    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    public Horizon_Joystick horizon_Joystick;
    public Rigidbody2D rigidbody2D;
    private bool gameStart = false;

    private void Start()
    {
        Invoke("StartGame", 3f);
    }

    public void Update()
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

            //Player Rotation
            Vector3 touchPositionToRotate = Camera.main.ScreenToWorldPoint(touch.position);

            touchPositionToRotate.z = 0;
            touchPositionToRotate.y = transform.position.y + 3f;
            Vector3 directiontToRotate = (touchPositionToRotate - transform.position).normalized;
            float rotationZ = Mathf.Atan2(directiontToRotate.y, directiontToRotate.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotationZ + -90f);


            // + 부분 부터 빼면 안올라감
            rigidbody2D.velocity = new Vector2(direction.x, 0) * speed * Time.deltaTime + Vector2.up * (speed / 2) * Time.deltaTime;

            if (touch.phase == TouchPhase.Ended)     //터치가 끝난 상태
            {
                rigidbody2D.velocity = Vector2.zero;
                rigidbody2D.velocity = Vector2.up * (speed / 4) * Time.deltaTime;
            }
        }
    }

    private void StartGame()
    {
        gameStart = true;
    }
}