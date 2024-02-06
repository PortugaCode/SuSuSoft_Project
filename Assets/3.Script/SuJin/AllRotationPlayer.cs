using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllRotationPlayer : MonoBehaviour
{
    public VariableJoystick variableJoystick;
    public Rigidbody2D rigidbody2D;

    private Vector3 touchPosition;
    private Vector3 direction;

    [Header("PlyerSpeed")]
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    public void FixedUpdate()
    {

        PlayerUp();

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

            rigidbody2D.velocity = new Vector2(direction.x, 0) * speed * Time.deltaTime;

            //Player Rotation
            PlayerRotation();

            if (touch.phase == TouchPhase.Ended)     //터치가 끝난 상태
            {
                rigidbody2D.velocity = Vector2.zero;
                rigidbody2D.velocity = Vector2.up * 1000f / 2f * Time.deltaTime;

                //터치 끝났을 때 일정속도 유지
                rigidbody2D.velocity = new Vector2(direction.x, 0) * speed * Time.deltaTime;
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
           // transform.rotation = Quaternion.Euler(0, 0, rotationZ + -90f);
        }
    }

}