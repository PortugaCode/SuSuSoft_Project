using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{

    private Vector3 touchPosition;
    private Rigidbody2D rigidbody2D;
    private Vector3 direction;
    [SerializeField]private float speed;
    private bool gameStart = false;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        Invoke("GameStart", 3f);
    }

    private void Update()
    {
        if(gameStart)
        {
            PlayerMove();
        }
    }
    private void PlayerMove()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;
            touchPosition.y = transform.position .y - 1;
            direction = (touchPosition - transform.position).normalized;

            // + 부분 부터 빼면 안올라감
            rigidbody2D.velocity = new Vector2(direction.x, 0) * speed * Time.deltaTime + Vector2.up * (speed / 4) * Time.deltaTime;

            if (touch.phase == TouchPhase.Ended)     //터치가 끝난 상태
            {
                rigidbody2D.velocity = Vector2.zero;
                rigidbody2D.velocity = Vector2.up * (speed / 6) * Time.deltaTime;
            }

        }
    }
 

    private void GameStart()
    {
        gameStart = true;
    }
}
