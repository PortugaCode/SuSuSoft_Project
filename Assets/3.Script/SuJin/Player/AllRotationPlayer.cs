using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllRotationPlayer : MonoBehaviour
{

    private Vector3 touchPosition;
    private Vector3 direction;

    public float speed;
    [SerializeField]private float deceleration = 10f;

    public VariableJoystick variableJoystick;
    public Rigidbody2D rigidbody2D;

    public void FixedUpdate()
    {
        AllRotationMove();
        MoveRotation();
    }
    private void AllRotationMove()
    {
        Vector2 direction = new Vector2(variableJoystick.Horizontal, variableJoystick.Vertical).normalized;
        rigidbody2D.velocity = direction * speed * Time.fixedDeltaTime;

        if (direction.magnitude == 0 && rigidbody2D.velocity.magnitude > 0)
        {
            rigidbody2D.velocity -= direction * deceleration * Time.fixedDeltaTime;

            if (rigidbody2D.velocity.magnitude > 5f)
            {
                rigidbody2D.velocity = Vector2.zero;
            }
        }
    }

    private void MoveRotation()
    {
        // 플레이어 이동 방향 설정
        Vector2 direction = new Vector2(variableJoystick.Horizontal, variableJoystick.Vertical).normalized;

        // 플레이어의 속도 설정
        rigidbody2D.velocity = direction * speed * Time.fixedDeltaTime;

        // 플레이어 회전 설정
        if (direction.magnitude > 0)
        {
            // 플레이어가 바라보는 각도 계산
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // 플레이어를 각도에 따라 회전
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
        }
    }
}